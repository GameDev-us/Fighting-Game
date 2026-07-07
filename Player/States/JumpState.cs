using _Scripts.GameSystems;
using _Scripts.Player.BaseScripts;
using UnityEngine;

namespace _Scripts.Player.States
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerContext ctx) : base(ctx) { JumpTimer = new Timer(context.Config.JumpCooldown); }

        public readonly Timer JumpTimer;
        
        public override void Enter()
        {
            JumpTimer.Start();
            
            context.PlayerMotor.SetGravityScale(context.Config.NormalGravityScale);
            context.PlayerMotor.OverrideVelocity(new Vector2(context.Velocity.x, 0f));

            var jumpForce = Mathf.Sqrt(-2 * (Physics.gravity.y * context.GravityScale) * context.Config.JumpHeight);
            context.PlayerMotor.AddForce(jumpForce * Vector2.up, true);
        }
    }
}
