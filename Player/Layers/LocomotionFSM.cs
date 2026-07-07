using _Scripts.Player.BaseScripts;
using _Scripts.Player.States;

namespace _Scripts.Player.Layers
{
    public class LocomotionFSM : BaseLayer
    {
        private readonly RunState _runState;
        
        public LocomotionFSM(PlayerContext ctx) : base(ctx)
        {
            _runState = new RunState(ctx);

            currentState = _runState;
        }
    }
}
