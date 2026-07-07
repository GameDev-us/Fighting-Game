using _Scripts.Player.BaseScripts;

namespace _Scripts.Player.States
{
    public class RiseState : BaseState
    {
        public RiseState(PlayerContext ctx) : base(ctx) { }

        public override void Enter() => context.PlayerMotor.SetGravityScale(context.Config.NormalGravityScale);
        
    }
}
