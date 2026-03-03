using System;
using System.Collections.Generic;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class EventManager
    {
        readonly List<GameEvent> _events = new();
        readonly ChangeLog _changeLog;
        
        public GameEvent PendingEvent { get; private set; }
        public event Action<GameEvent> EventTriggered;
        
        public IReadOnlyList<GameEvent> AllEvents => _events;
        
        public EventManager(ChangeLog changeLog)
        {
            _changeLog = changeLog;
            RegisterAll();
        }
        
        void RegisterAll()
        {
            // Day-triggered events
            Register(new OpeningBombardmentEvent());
            Register(new EnemyMessengerEvent());
            Register(new SupplyCartsInterceptedEvent());
            Register(new SmugglerAtGateEvent());
            Register(new WellContaminationScareEvent());
            Register(new MilitiaVolunteersEvent());
            Register(new SiegeTowersSpottedEvent());
            Register(new RefugeesAtGatesEvent());
            Register(new EnemySappersEvent());
            Register(new EnemyCommanderLetterEvent());
            Register(new TaintedWellEvent());
            Register(new PlagueRatsEvent());
            Register(new BurningFarmsEvent());
            Register(new EnemyUltimatumEvent());
            Register(new FinalAssaultEvent());
            Register(new DistantHornsEvent());

            // Condition-triggered events
            Register(new WorkerTakesLifeEvent());
            Register(new SiegeBombardmentEvent());
            Register(new HungerRiotEvent());
            Register(new FeverOutbreakEvent());
            Register(new DesertionWaveEvent());
            Register(new WallBreachEvent());
            Register(new FireArtisanQuarterEvent());
            Register(new DespairEvent());
            Register(new ChildrensPleaEvent());
            Register(new CrisisOfFaithEvent());
            Register(new SiegeEngineersArriveEvent());
            Register(new DissidentsDiscoveredEvent());
            Register(new SignalFireEvent());
            Register(new TyrantsReckoningEvent());
            Register(new BetrayalFromWithinEvent());

            // Relief army events
            Register(new ReliefDustCloudsEvent());
            Register(new ReliefHornsEvent());
            Register(new ReliefBannersEvent());

            // Game over events
            Register(new CouncilRevoltEvent());
            Register(new TotalCollapseEvent());

            // Special events
            Register(new BlackMarketTraderEvent());
            Register(new SpySellingIntelEvent());
            Register(new IntelSiegeWarningEvent());

            // Streak events
            Register(new SteadySuppliesEvent());
            Register(new HealthImprovingEvent());
            Register(new WallsStillStandEvent());
            Register(new FortuneFavorsBoldEvent());
        }
        
        void Register(GameEvent e) => _events.Add(e);
        
        public void EvaluateEvents(GameState state)
        {
            if (PendingEvent != null) return; // already have a pending event
            
            // Sort by priority descending
            GameEvent best = null;
            foreach (var e in _events)
            {
                if (e.IsOneTime && e.HasTriggered) continue;
                if (!e.CanTrigger(state)) continue;
                if (best == null || e.Priority > best.Priority) best = e;
            }
            
            if (best == null) return;
            
            best.HasTriggered = true;
            
            if (best.IsRespondable)
            {
                PendingEvent = best;
                EventTriggered?.Invoke(best);
            }
            else
            {
                best.Execute(state, _changeLog);
                EventTriggered?.Invoke(best);
            }
        }
        
        public void RespondToEvent(GameState state, int responseIndex)
        {
            if (PendingEvent == null) return;
            PendingEvent.ExecuteResponse(state, _changeLog, responseIndex);
            PendingEvent = null;
        }
        
        public void DismissEvent()
        {
            PendingEvent = null;
        }
    }
}
