using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using TypeRegistry;

namespace Siege.Gameplay.Orders
{
    [RegisterTypeLookup]
    public class StorytellingNightOrder : IOrder
    {
        const string Narrative = "Around a guttering fire, an old woman speaks of summers past. For an hour, the siege is forgotten.";
        const double MoraleGain = 8;
        const double MoraleMin = 20;
        const double MoraleMax = 50;

        readonly IPopupService _popup;

        public StorytellingNightOrder(IPopupService popup) => _popup = popup;

        public string Id => "storytelling_night";
        public string Name => "Storytelling Night";
        public string Description => "Gather the people for a night of stories, easing despair when morale is middling.";
        public int CooldownDays => 4;

        public bool CanIssue(GameState state) =>
            state.Morale >= MoraleMin && state.Morale <= MoraleMax;

        public void OnExecute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);
            _popup.Open(Name, Narrative, log.SliceSince(before));
        }

        public IOrder Clone() => new StorytellingNightOrder(_popup);
    }
}
