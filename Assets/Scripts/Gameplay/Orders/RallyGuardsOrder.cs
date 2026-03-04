namespace Siege.Gameplay.Orders
{
    public class RallyGuardsOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "rally_guards";
        public string Name => "Rally Guards";
        public string Description => "Muster the garrison for a show of force, calming unrest and lifting morale.";
        public string NarrativeText => "Armored boots echo through the square. For a moment, the people remember what order looked like.";
        public int CooldownDays => 3;
    }
}
