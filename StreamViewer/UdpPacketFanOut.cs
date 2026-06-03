using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MissionPlanner.StreamViewer
{
    public sealed class UdpPacketFanOut : IDisposable
    {
        private readonly string _name;
        private readonly int _listenPort;
        private readonly int _receiveBufferBytes;
        private readonly bool _softDropEnabled;
        private readonly int _backlogDropThresholdBytes;
        private readonly int _maxDrainPackets;

        private readonly object _endpointLock = new object();
        private readonly List<IPEndPoint> _outputEndpoints = new List<IPEndPoint>();

        private Socket _receiveSocket;
        private Socket _sendSocket;
        private Task _workerTask;

        private volatile bool _stopRequested;

        private long _receivedPacketCount;
        private long _forwardedPacketCount;
        private long _droppedPacketCount;

        public event EventHandler<string> StatusChanged;
        public event EventHandler<Exception> ErrorOccurred;

        public bool IsRunning
        {
            get
            {
                return _workerTask != null && !_workerTask.IsCompleted;
            }
        }

        public long ReceivedPacketCount
        {
            get
            {
                return Interlocked.Read(ref _receivedPacketCount);
            }
        }

        public long ForwardedPacketCount
        {
            get
            {
                return Interlocked.Read(ref _forwardedPacketCount);
            }
        }

        public long DroppedPacketCount
        {
            get
            {
                return Interlocked.Read(ref _droppedPacketCount);
            }
        }

        public UdpPacketFanOut(
            string name,
            int listenPort,
            int receiveBufferBytes,
            bool softDropEnabled,
            int backlogDropThresholdBytes,
            int maxDrainPackets,
            params IPEndPoint[] outputEndpoints)
        {
            if (listenPort <= 0)
                throw new ArgumentOutOfRangeException("listenPort");

            if (outputEndpoints == null || outputEndpoints.Length == 0)
                throw new ArgumentException("At least one output endpoint is required.", "outputEndpoints");

            _name = string.IsNullOrWhiteSpace(name) ? "UDP" : name;
            _listenPort = listenPort;

            _receiveBufferBytes = receiveBufferBytes <= 0 ? 1024 * 1024 : receiveBufferBytes;
            _softDropEnabled = softDropEnabled;
            _backlogDropThresholdBytes = backlogDropThresholdBytes <= 0 ? 512 * 1024 : backlogDropThresholdBytes;
            _maxDrainPackets = maxDrainPackets <= 0 ? 16 : maxDrainPackets;

            for (int i = 0; i < outputEndpoints.Length; i++)
                _outputEndpoints.Add(outputEndpoints[i]);
        }

        public void AddOutputEndpoint(IPEndPoint endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            lock (_endpointLock)
            {
                for (int i = 0; i < _outputEndpoints.Count; i++)
                {
                    if (_outputEndpoints[i].Equals(endpoint))
                        return;
                }

                _outputEndpoints.Add(endpoint);
            }

            RaiseStatus(_name + " fan-out added endpoint: " + endpoint);
        }

        public void RemoveOutputEndpoint(IPEndPoint endpoint)
        {
            if (endpoint == null)
                return;

            lock (_endpointLock)
            {
                for (int i = _outputEndpoints.Count - 1; i >= 0; i--)
                {
                    if (_outputEndpoints[i].Equals(endpoint))
                        _outputEndpoints.RemoveAt(i);
                }
            }

            RaiseStatus(_name + " fan-out removed endpoint: " + endpoint);
        }

        public void Start()
        {
            if (IsRunning)
                return;

            Stop();

            _stopRequested = false;
            _receivedPacketCount = 0;
            _forwardedPacketCount = 0;
            _droppedPacketCount = 0;

            _receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiveSocket.ExclusiveAddressUse = false;
            _receiveSocket.ReceiveBufferSize = _receiveBufferBytes;

            _receiveSocket.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReuseAddress,
                true
            );

            _receiveSocket.Bind(new IPEndPoint(IPAddress.Any, _listenPort));

            _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _sendSocket.SendBufferSize = 256 * 1024;

            _workerTask = Task.Run(new Action(Run));

            RaiseStatus(
                _name +
                " fan-out listening on 0.0.0.0:" +
                _listenPort +
                ", receiveBuffer=" +
                _receiveBufferBytes +
                ", softDrop=" +
                _softDropEnabled +
                ", backlogThreshold=" +
                _backlogDropThresholdBytes +
                ", maxDrain=" +
                _maxDrainPackets
            );
        }

        public void Stop()
        {
            _stopRequested = true;

            try
            {
                if (_receiveSocket != null)
                    _receiveSocket.Close();
            }
            catch
            {
            }

            try
            {
                if (_sendSocket != null)
                    _sendSocket.Close();
            }
            catch
            {
            }

            try
            {
                if (_workerTask != null)
                    _workerTask.Wait(500);
            }
            catch
            {
            }

            try
            {
                if (_receiveSocket != null)
                    _receiveSocket.Dispose();
            }
            catch
            {
            }

            try
            {
                if (_sendSocket != null)
                    _sendSocket.Dispose();
            }
            catch
            {
            }

            _receiveSocket = null;
            _sendSocket = null;
            _workerTask = null;

            RaiseStatus(_name + " fan-out stopped.");
        }

        private void Run()
        {
            byte[] receiveBuffer = new byte[65536];
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (!_stopRequested)
                {
                    int packetLength = _receiveSocket.ReceiveFrom(receiveBuffer, ref remoteEndPoint);

                    if (packetLength <= 0)
                        continue;

                    Interlocked.Increment(ref _receivedPacketCount);

                    int newestPacketLength = packetLength;
                    byte[] newestPacket = new byte[packetLength];
                    Buffer.BlockCopy(receiveBuffer, 0, newestPacket, 0, packetLength);

                    if (_softDropEnabled && _receiveSocket.Available > _backlogDropThresholdBytes)
                    {
                        int drainedPackets = 0;

                        while (drainedPackets < _maxDrainPackets && _receiveSocket.Available > 0)
                        {
                            EndPoint drainRemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                            int drainedLength = _receiveSocket.ReceiveFrom(receiveBuffer, ref drainRemoteEndPoint);

                            if (drainedLength <= 0)
                                break;

                            Interlocked.Increment(ref _receivedPacketCount);

                            newestPacketLength = drainedLength;
                            newestPacket = new byte[drainedLength];
                            Buffer.BlockCopy(receiveBuffer, 0, newestPacket, 0, drainedLength);

                            drainedPackets++;
                        }

                        if (drainedPackets > 0)
                            Interlocked.Add(ref _droppedPacketCount, drainedPackets);
                    }

                    ForwardPacket(newestPacket, newestPacketLength);

                    long received = Interlocked.Read(ref _receivedPacketCount);

                    if (received == 1 || received % 500 == 0)
                    {
                        Debug.WriteLine(
                            _name +
                            " packets received=" +
                            ReceivedPacketCount +
                            ", forwarded=" +
                            ForwardedPacketCount +
                            ", dropped=" +
                            DroppedPacketCount +
                            ", socketAvailable=" +
                            (_receiveSocket != null ? _receiveSocket.Available : 0)
                        );
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (SocketException ex)
            {
                if (!_stopRequested)
                {
                    Debug.WriteLine(_name + " fan-out socket error: " + ex);
                    RaiseError(ex);
                }
            }
            catch (Exception ex)
            {
                if (!_stopRequested)
                {
                    Debug.WriteLine(_name + " fan-out error: " + ex);
                    RaiseError(ex);
                }
            }
        }

        private void ForwardPacket(byte[] packet, int packetLength)
        {
            IPEndPoint[] endpoints;

            lock (_endpointLock)
            {
                endpoints = _outputEndpoints.ToArray();
            }

            for (int i = 0; i < endpoints.Length; i++)
            {
                try
                {
                    _sendSocket.SendTo(packet, 0, packetLength, SocketFlags.None, endpoints[i]);
                    Interlocked.Increment(ref _forwardedPacketCount);
                }
                catch (SocketException ex)
                {
                    Debug.WriteLine(_name + " send error to " + endpoints[i] + ": " + ex.Message);
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }

        private void RaiseStatus(string message)
        {
            EventHandler<string> handler = StatusChanged;

            if (handler != null)
                handler(this, message);
        }

        private void RaiseError(Exception ex)
        {
            EventHandler<Exception> handler = ErrorOccurred;

            if (handler != null)
                handler(this, ex);
        }
    }
}