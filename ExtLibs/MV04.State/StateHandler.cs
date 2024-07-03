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
        public MV04_State PreviousState { get; set; }

        public MV04_State NewState { get; set; }

        public MV04StateChangeEventArgs(MV04_State previousState, MV04_State newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }
    }

    public static class StateHandler
    {
        private static MV04_State _currentSate;

        public static MV04_State CurrentSate
        {
            get => _currentSate;
            set
            {
                MV04_State previousState = _currentSate;
                _currentSate = value;
                MV04StateChange(null, new MV04StateChangeEventArgs(previousState, _currentSate));
            }
        }

        public static event EventHandler<MV04StateChangeEventArgs> MV04StateChange;
    }
}
