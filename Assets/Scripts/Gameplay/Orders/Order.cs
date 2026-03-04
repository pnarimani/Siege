namespace Siege.Gameplay.Orders
{
    public interface Order
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string NarrativeText => null;
        int CooldownDays { get; }
        bool IsToggle => false;
        bool CanDeactivate => true;
        bool IsActive { get; set; }
    }
}
