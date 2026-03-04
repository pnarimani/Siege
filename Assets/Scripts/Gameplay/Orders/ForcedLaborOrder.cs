namespace Siege.Gameplay.Orders
{
    public class ForcedLaborOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "forced_labor";
        public string Name => "Forced Labor";
        public string Description => "Press civilians into dangerous labor to gather materials. Not all will survive.";
        public string NarrativeText => "Under the lash, the rubble is cleared. Two bodies are pulled from the wreckage at dusk.";
        public int CooldownDays => 3;
    }
}
