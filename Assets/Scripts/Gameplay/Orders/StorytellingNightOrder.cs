using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;

namespace Siege.Gameplay.Orders
{
    public class StorytellingNightOrder : Order
    {
        const int Cooldown = 4;
        const double MoraleGain = 8;
        const double MoraleMin = 20;
        const double MoraleMax = 50;

        public override string Id => "storytelling_night";
        public override string Name => "Storytelling Night";
        public override string Description => "Gather the people for a night of stories, easing despair when morale is middling.";
        public override string NarrativeText => "Around a guttering fire, an old woman speaks of summers past. For an hour, the siege is forgotten.";
        public override int CooldownDays => Cooldown;

        public override bool CanIssue(GameState state) =>
            state.Morale >= MoraleMin && state.Morale <= MoraleMax;

        public override void Execute(GameState state, ChangeLog log)
        {
            int before = log.CurrentChanges.Count;
            state.Morale += MoraleGain;
            log.Record("Morale", MoraleGain, Id);
            Popup.Open(Name, NarrativeText, log.SliceSince(before));
        }
    }
}
