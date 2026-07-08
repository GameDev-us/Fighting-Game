using System.Collections.Generic;
using _Scripts.Player.BaseScripts;
using _Scripts.Player.States;

namespace _Scripts.Player.Layers
{
    public class OverrideFSM : BaseLayer
    {
        private readonly NoOverride _noOverride;

        private readonly List<IOverrides> _abilities = new();

        public OverrideFSM(PlayerContext ctx) : base(ctx)
        {
            _noOverride = new NoOverride(ctx);
            _nextAbility = _noOverride;
            
            _abilities.Add(_noOverride);
            _abilities.Add(new ClimbState(ctx));
            _abilities.Add(new WallJumpState(ctx));
            _abilities.Add(new DashState(ctx));

            currentState = _noOverride;
        }
        
        public bool IsActive { get; private set; }
        private IOverrides _nextAbility;

        public override void FrameTick()
        {
            foreach (var ability in _abilities)
            {
                ability.UpdateTimers();
                if (ability.Priority == _nextAbility.Priority) continue;

                if (ability.CanActivate() && _nextAbility.CanDeactivate())
                    _nextAbility = ability;
            }
            
            TransitionTo((BaseState) _nextAbility);
            
            IsActive = currentState != _noOverride;
            currentState.FrameTick();
        }

        public override void PhysicsTick()
        {
            currentState.PhysicsTick();
        }
    }
}