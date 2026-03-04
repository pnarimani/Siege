namespace Siege.Gameplay.Missions
{
    public interface Mission
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
