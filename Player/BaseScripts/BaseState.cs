namespace _Scripts.Player.BaseScripts
{
    public abstract class BaseState
    {
        protected PlayerContext context;

        public BaseState(PlayerContext ctx) => context = ctx;
        
        public virtual void Enter() {}
        public virtual void FrameTick() {}
        public virtual void PhysicsTick() {}
        public virtual void Exit() {}
    }
}
