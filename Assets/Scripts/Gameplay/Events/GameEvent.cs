using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public abstract class GameEvent
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public bool HasTriggered { get; set; }
        public virtual bool IsOneTime => true;
        public virtual int Priority => 0; // higher = checked first
        
        public abstract bool CanTrigger(GameState state);
        
        // For automatic events
        public virtual void Execute(GameState state, ChangeLog log) { }
        
        // For respondable events
        public virtual bool IsRespondable => false;
        public virtual EventResponse[] GetResponses(GameState state) => System.Array.Empty<EventResponse>();
        public virtual void ExecuteResponse(GameState state, ChangeLog log, int responseIndex) { }
        
        // Narrative text shown to the player
        public virtual string GetNarrativeText(GameState state) => Description;
    }
}
