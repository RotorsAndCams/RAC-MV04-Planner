using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MV04.State
{
    public enum MV04_State
    {
        Manual,
        TapToFly,
        Auto,
        Follow,
        FPV,
        Takeoff,
        RTL,
        Land,
        Unknown
    }

    public class MV04StateChangeEventArgs: EventArgs
    {
        #region Fields

        public MV04_State PreviousState { get; set; }

        public MV04_State NewState { get; set; }

        #endregion

        #region Methods

        public MV04StateChangeEventArgs(MV04_State previousState, MV04_State newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }

        #endregion
    }

    public static class StateHandler
    {
        #region Fields

        public static MV04_State PrevioustSate { get; private set; }

        private static MV04_State _currentSate;

        public static MV04_State CurrentSate
        {
            get => _currentSate;
            set
            {
                PrevioustSate = _currentSate;
                _currentSate = value;
                if (MV04StateChange != null)
                    MV04StateChange(null, new MV04StateChangeEventArgs(PrevioustSate, _currentSate));
            }
        }

        public static event EventHandler<MV04StateChangeEventArgs> MV04StateChange;

        #endregion
    }
}
