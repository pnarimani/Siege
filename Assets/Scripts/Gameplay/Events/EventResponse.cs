namespace Siege.Gameplay.Events
{
    public struct EventResponse
    {
        public string Label;
        public string Description;
        public string NarrativeText;
        
        public EventResponse(string label, string description, string narrativeText = null)
        {
            Label = label;
            Description = description;
            NarrativeText = narrativeText;
        }
    }
}
