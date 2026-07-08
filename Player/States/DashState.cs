using _Scripts.Player.BaseScripts;
using _Scripts.GameSystems;
using UnityEngine;

namespace _Scripts.Player.States
{
	public class DashState : BaseState, IOverrides
	{
		private Vector2 _dashDirection;
		private readonly GameTimer _dashDuration;
		private readonly GameTimer _dashCooldown;
		
		public DashState(PlayerContext ctx) : base(ctx)
		{
			_dashCooldown = new GameTimer(context.Config.DashCooldown);
			_dashDuration = new GameTimer(context.Config.DashDuration);

			_dashDuration.OnTimerEnd += () => _dashCooldown.Start();
		}

		public int Priority { get; } = 20;

		public bool CanActivate() => context.DashInput && !_dashDuration.IsOn() && !_dashCooldown.IsOn();
		public bool CanDeactivate() => !_dashDuration.IsOn();

		public override void Enter()
		{
			_dashDuration.Start();
			_dashDirection = context.MoveInput != Vector2.zero ? context.MoveInput : Vector2.right;
			context.PlayerMotor.OverrideVelocity(Vector2.zero);
		}

		public override void FrameTick() 
		{
			_dashDuration.Tick();
		}

		public override void PhysicsTick()
		{
			context.PlayerMotor.OverrideVelocity(_dashDirection * context.Config.DashSpeed); 
		}

		public override void Exit()
		{
			context.PlayerMotor.OverrideVelocity(context.Velocity * context.Config.DashVelocityCutMulti);
		}

		public void UpdateTimers()
		{
			_dashCooldown.Tick();
		}
	}
}