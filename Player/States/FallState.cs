using _Scripts.Player.BaseScripts;

namespace _Scripts.Player.States
{
    public class FallState : BaseState
    {
        public FallState(PlayerContext ctx) : base(ctx) { }
        
        public override void Enter() => context.PlayerMotor.SetGravityScale(context.Config.FallGravityScale);
        
    }
}
