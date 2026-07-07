using _Scripts.GameSystems;
using _Scripts.Player.BaseScripts;
using UnityEngine;

namespace _Scripts.Player.States
{
    public class WallJumpState : BaseState, IOverrides
    {
        public WallJumpState(PlayerContext ctx) : base(ctx)
        {
            _jumpBufferTimer = new Timer(context.Config.WallJumpBufferTime);
            _jumpCoyoteTimer = new Timer(context.Config.WallJumpCoyoteTime);
            _jumpTimer = new Timer(context.Config.WallJumpCooldown);
        }

        private readonly Timer _jumpBufferTimer;
        private readonly Timer _jumpCoyoteTimer;
        private readonly Timer _jumpTimer;
        
        public int Priority { get; } = 5;
        public bool CanActivate() => _jumpBufferTimer.IsOn() && _jumpCoyoteTimer.IsOn() && _jumpCoyoteTimer.IsOn();
        public bool CanDeactivate() => !CanActivate();

        public override void Enter()
        {
            _jumpBufferTimer.End();
            _jumpCoyoteTimer.End();
            _jumpTimer.Start();
            
            context.PlayerMotor.OverrideVelocity(Vector2.zero);
            
            var pushDirection = context.OnLeftWall ? 1 : -1;
            context.PlayerMotor.AddForce(new Vector2(context.Config.WallJumpForce.x * pushDirection, context.Config.WallJumpForce.y), true);
        }

        public void UpdateGraceTimers()
        {
            if (context.OnWall && !_jumpCoyoteTimer.IsOn() && !_jumpTimer.IsOn())
                _jumpCoyoteTimer.Start();
            
            if (context.MoveInput.y > 0.01f && !_jumpBufferTimer.IsOn())
                _jumpBufferTimer.Start();
            
            _jumpBufferTimer.Tick();
            _jumpCoyoteTimer.Tick();
            _jumpTimer.Tick();
        }
    }
}
