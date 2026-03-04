namespace Siege.Gameplay.Orders
{
    public class OfferTributeOrder : IOrder
    {
        public bool IsActive { get; set; }
        public string Id => "offer_tribute";
        public string Name => "Offer Tribute";
        public string Description => "Send food and water to the besiegers to stall their advance. Devastating to morale.";
        public string NarrativeText => "Carts of provisions roll out the gate. The enemy takes them without a word. The people watch in silence.";
        public int CooldownDays => 0;
        public bool IsToggle => true;
    }
}
