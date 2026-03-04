namespace Siege.Gameplay.Orders
{
    public class StorytellingNightOrder : Order
    {
        public bool IsActive { get; set; }
        public string Id => "storytelling_night";
        public string Name => "Storytelling Night";
        public string Description => "Gather the people for a night of stories, easing despair when morale is middling.";
        public string NarrativeText => "Around a guttering fire, an old woman speaks of summers past. For an hour, the siege is forgotten.";
        public int CooldownDays => 4;
    }
}
