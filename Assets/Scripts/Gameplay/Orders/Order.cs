using JetBrains.Annotations;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [UsedImplicitly]
    [RegisterTypeLookup]
    public interface IOrder
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
