using _Scripts.Player.BaseScripts;

namespace _Scripts.Player.States
{
    public class NoOverride : BaseState, IOverrides
    {
        public NoOverride(PlayerContext ctx) : base(ctx) { }

        public int Priority { get; } = 0;
        public bool CanActivate() => true;
        public bool CanDeactivate() => true;
    }
}
