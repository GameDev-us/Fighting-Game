namespace _Scripts.Player.BaseScripts
{
    public interface IOverrides
    {
        int Priority { get; }
        
        bool CanActivate();
        bool CanDeactivate();
        void UpdateTimers() {}
    }
}
