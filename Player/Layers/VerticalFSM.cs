using _Scripts.GameSystems;
using _Scripts.Player.BaseScripts;
using _Scripts.Player.States;

namespace _Scripts.Player.Layers
{
    public class VerticalFSM : BaseLayer
    {
        private readonly JumpState _jumpState;
        private readonly RiseState _riseState;
        private readonly FallState _fallState;

        private readonly GameTimer _lastGrounded;
        private readonly GameTimer _lastJumpInput;

        public VerticalFSM(PlayerContext ctx) : base(ctx)
        {
            _jumpState = new JumpState(ctx);
            _riseState = new RiseState(ctx);
            _fallState = new FallState(ctx);

            _lastGrounded = new GameTimer(context.Config.CoyoteTime);
            _lastJumpInput = new GameTimer(context.Config.BufferTime);

            currentState = _fallState;
            currentState?.Enter();
            
            _jumpState.JumpGameTimer.OnTimerStart += () => context.CanJump = false;
            _jumpState.JumpGameTimer.OnTimerEnd += ()  => context.CanJump = true;
        }

        public override void FrameTick()
        {
            currentState.FrameTick();
            
            if (_lastJumpInput.CurrentTime > 0f && _lastGrounded.CurrentTime > 0f && context.CanJump)
                TransitionTo(_jumpState);
            else if (!context.OnGround)
            {
                switch (context.Velocity.y)
                {
                    case > 0:
                        TransitionTo(_riseState);
                        break;
                    case <= 0:
                        TransitionTo(_fallState);
                        break;
                }
            }

            if (context.OnGround) _lastGrounded.Start();
            if (context.MoveInput.y > 0.01f) _lastJumpInput.Start();
            
            _lastGrounded.Tick();
            _lastJumpInput.Tick();
            
            _jumpState.JumpGameTimer.Tick();
        }
    }
}
