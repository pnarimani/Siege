using TypeRegistry;

namespace Siege.Gameplay.Missions
{
    [RegisterTypeLookup]
    public interface IMission
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        int DurationDays { get; }
        int WorkerCost { get; }
        int GuardCost => 0;
        bool IsActive { get; set; }
        int DaysRemaining { get; set; }
    }
}
