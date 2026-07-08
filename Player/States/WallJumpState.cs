using _Scripts.GameSystems;
using _Scripts.Player.BaseScripts;
using UnityEngine;

namespace _Scripts.Player.States
{
    public class WallJumpState : BaseState, IOverrides
    {
        public WallJumpState(PlayerContext ctx) : base(ctx)
        {
            _jumpBufferGameTimer = new GameTimer(context.Config.WallJumpBufferTime);
            _jumpCoyoteGameTimer = new GameTimer(context.Config.WallJumpCoyoteTime);
            _jumpGameTimer = new GameTimer(context.Config.WallJumpCooldown);
        }

        private readonly GameTimer _jumpBufferGameTimer;
        private readonly GameTimer _jumpCoyoteGameTimer;
        private readonly GameTimer _jumpGameTimer;
        
        public int Priority { get; } = 5;
        public bool CanActivate() => _jumpBufferGameTimer.IsOn() && _jumpCoyoteGameTimer.IsOn() && context.Velocity.y > -context.Config.WallJumpVerVelThreshold;
        public bool CanDeactivate() => !CanActivate();

        public override void Enter()
        {
            _jumpBufferGameTimer.End();
            _jumpCoyoteGameTimer.End();
            _jumpGameTimer.Start();
            
            context.PlayerMotor.OverrideVelocity(Vector2.zero);
            
            var pushDirection = context.OnLeftWall ? 1 : -1;
            context.PlayerMotor.AddForce(new Vector2(context.Config.WallJumpForce.x * pushDirection, context.Config.WallJumpForce.y), true);
        }

        public void UpdateTimers()
        {
            if (context.OnWall && !_jumpCoyoteGameTimer.IsOn() && !_jumpGameTimer.IsOn())
                _jumpCoyoteGameTimer.Start();
            
            if (context.MoveInput.y > 0.01f && !_jumpBufferGameTimer.IsOn())
                _jumpBufferGameTimer.Start();
            
            _jumpBufferGameTimer.Tick();
            _jumpCoyoteGameTimer.Tick();
            _jumpGameTimer.Tick();
        }
    }
}
