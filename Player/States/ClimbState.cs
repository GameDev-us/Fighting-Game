using _Scripts.Player.BaseScripts;
using UnityEngine;

namespace _Scripts.Player.States
{
    public class ClimbState : BaseState, IOverrides
    {
        public ClimbState(PlayerContext ctx) : base(ctx) { }

        public int Priority { get; } = 10;
        public bool CanActivate() => context.OnWall && context.WallGrabInput && !context.OnGround;
        public bool CanDeactivate() => !CanActivate();

        private float _moveInput;

        public override void Enter()
        {
            context.PlayerMotor.SetGravityScale(0); 
        }

        public override void FrameTick()
        {
            _moveInput = Mathf.Round(context.MoveInput.y);
        }

        public override void PhysicsTick()
        {
            var acceleration = context.Config.ClimbAcceleration;
            var deceleration = context.Config.ClimbDeceleration;
            
            #region Run
            
            var targetSpeed = _moveInput * context.Config.ClimbSpeed;
            var accelRate = Mathf.Abs(_moveInput) > 0.01f ? acceleration : deceleration;
            var speedDifference = targetSpeed - context.Velocity.y;
            var force = Mathf.Pow(Mathf.Abs(speedDifference * accelRate), context.Config.ClimbVelocityPower) * Mathf.Sign(speedDifference);
            
            context.PlayerMotor.AddForce(force * Vector2.up, false);

            #endregion
            
            #region Friction

            if (!context.OnWall || !(Mathf.Abs(_moveInput) < 0.01f)) return;
            
            var amount = Mathf.Min(Mathf.Abs(context.Config.ClimbFriction), Mathf.Abs(context.Velocity.y)) * Mathf.Sign(context.Velocity.y);
            context.PlayerMotor.AddForce(-amount * Vector2.up, true);

            #endregion
        }

        public override void Exit()
        {
            context.PlayerMotor.SetGravityScale(context.Config.NormalGravityScale);
        }
    }
}
