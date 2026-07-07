using _Scripts.Player.BaseScripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Scripts.Player.States
{
    public class RunState : BaseState
    {
        public RunState(PlayerContext ctx) : base(ctx) { }

        private float _moveInput;

        public override void FrameTick()
        {
            _moveInput = Mathf.Round(context.MoveInput.x);
        }

        public override void PhysicsTick()
        {
            #region Variables
            
            var maxSpeed = context.Config.RunSpeed * (context.OnGround ? 1f : context.Config.AirSpeedMult);
            var accel = context.Config.RunAcceleration * (context.OnGround ? 1f : context.Config.AirControl);
            var decel = context.Config.RunDeceleration * (context.OnGround ? 1f : context.Config.AirControl);

            #endregion

            #region Run
            
            var targetSpeed = _moveInput * maxSpeed;
            var accelRate = Mathf.Abs(_moveInput) > 0.01f ? accel : decel;
            var speedDifference = targetSpeed - context.Velocity.x;
            var force = Mathf.Pow(Mathf.Abs(speedDifference * accelRate), context.Config.RunVelocityPower) * Mathf.Sign(speedDifference);
            
            context.PlayerMotor.AddForce(force * Vector2.right, false);

            #endregion

            #region Friction

            if (context.OnGround && Mathf.Abs(_moveInput) < 0.01f)
            {
                var amount = Mathf.Min(Mathf.Abs(context.Config.RunFriction), Mathf.Abs(context.Velocity.x)) * Mathf.Sign(context.Velocity.x);
                context.PlayerMotor.AddForce(-amount * Vector2.right, true);
            }

            #endregion
        }
    }
}
