using Autofac;
using AutofacUnity;
using JetBrains.Annotations;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Defense;
using Siege.Gameplay.Events;
using Siege.Gameplay.Laws;
using Siege.Gameplay.LossConditions;
using Siege.Gameplay.Missions;
using Siege.Gameplay.Orders;
using Siege.Gameplay.Political;
using Siege.Gameplay.Population;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using Siege.Gameplay.Zones;

namespace Siege.Gameplay.Installers
{
    [UsedImplicitly]
    public class GameplayInstaller : IGameplayInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            // Core simulation
            builder.RegisterType<GameState>().SingleInstance();
            builder.RegisterType<GameClock>().SingleInstance();
            builder.RegisterType<ChangeLog>().SingleInstance();

            // Political
            builder.RegisterType<PoliticalState>().SingleInstance();
            builder.RegisterType<PoliticalDecaySystem>().As<ISimulationSystem>().SingleInstance();

            // Zones & Buildings
            builder.RegisterType<ZoneManager>().SingleInstance();
            builder.RegisterType<WorkerAllocation>().SingleInstance();

            // Resource Economy
            builder.RegisterType<ResourceStorage>().SingleInstance();
            builder.RegisterType<ResourceProductionSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<ResourceConsumptionSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<CareSystem>().As<ISimulationSystem>().SingleInstance();

            // Siege & Defense
            builder.RegisterType<DefenseManager>().SingleInstance();
            builder.RegisterType<ReliefArmy>().AsSelf().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<SiegeSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<GuardEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Population & Health
            builder.RegisterType<PopulationSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<SicknessSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<OvercrowdingSystem>().As<ISimulationSystem>().SingleInstance();

            // Services
            builder.RegisterType<PopupService>().As<IPopupService>().SingleInstance();

            // Laws
            builder.RegisterType<AbandonOuterRingLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<AbandonOuterRingLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<BurnTheDeadLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<BurnTheDeadLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<CannibalismLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<CannibalismLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<CollectiveFarmsLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<CollectiveFarmsLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<ConscriptElderlyLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<ConscriptElderlyLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<CurfewLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<CurfewLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<EmergencySheltersLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<EmergencySheltersLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<ExtendedShiftsLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<ExtendedShiftsLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<FaithProcessionsLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<FaithProcessionsLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<FoodConfiscationLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<FoodConfiscationLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<GarrisonMandateLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<GarrisonMandateLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<MandatoryGuardServiceLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<MandatoryGuardServiceLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<MartialLawLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<MartialLawLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<MedicalTriageLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<MedicalTriageLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<OathOfMercyLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<OathOfMercyLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<PublicExecutionsLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<PublicExecutionsLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<PurgeTheDisloyalLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<PurgeTheDisloyalLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<ScorchedEarthLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<ScorchedEarthLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<ShadowCouncilLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<ShadowCouncilLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<StrictRationsLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<StrictRationsLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<WaterRationingLaw>().AsSelf().As<Law>().SingleInstance();
            builder.RegisterType<WaterRationingLawHandler>().As<ILawHandler>().SingleInstance();
            builder.RegisterType<LawDispatcher>().SingleInstance();
            builder.RegisterType<LawEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Orders
            builder.RegisterType<BetrayAlliesOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<BetrayAlliesOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<BribeEnemyOfficerOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<BribeEnemyOfficerOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<BurnSurplusOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<BurnSurplusOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<CrackdownPatrolsOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<CrackdownPatrolsOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<DayOfRemembranceOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<DayOfRemembranceOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<DivertSuppliesOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<DivertSuppliesOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<DistributeLuxuriesOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<DistributeLuxuriesOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<ForcedLaborOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<ForcedLaborOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<FortifyGateOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<FortifyGateOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<HoldFeastOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<HoldFeastOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<HostageExchangeOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<HostageExchangeOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<InspirePeopleOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<InspirePeopleOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<OfferTributeOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<OfferTributeOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<PublicConfessionOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<PublicConfessionOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<PublicTrialOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<PublicTrialOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<QuarantineDistrictOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<QuarantineDistrictOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<RallyGuardsOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<RallyGuardsOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<RationMedicineOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<RationMedicineOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<ReinforceWallsOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<ReinforceWallsOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<SacrificeSickOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<SacrificeSickOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<ScavengeMedicineOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<ScavengeMedicineOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<SecretCorrespondenceOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<SecretCorrespondenceOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<StorytellingNightOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<StorytellingNightOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<VoluntaryEvacuationOrder>().AsSelf().As<Order>().SingleInstance();
            builder.RegisterType<VoluntaryEvacuationOrderHandler>().As<IOrderHandler>().SingleInstance();
            builder.RegisterType<OrderDispatcher>().SingleInstance();
            builder.RegisterType<OrderEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Missions
            builder.RegisterType<DiplomaticEnvoy>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<DiplomaticEnvoyHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<EngineerTunnels>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<EngineerTunnelsHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<ForageBeyondWalls>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<ForageBeyondWallsHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<NegotiateBlackMarketeers>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<NegotiateBlackMarketeersHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<NightRaid>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<NightRaidHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<RaidCivilianFarms>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<RaidCivilianFarmsHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<SabotageEnemySupplies>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<SabotageEnemySuppliesHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<ScoutingMission>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<ScoutingMissionHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<SearchAbandonedHomes>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<SearchAbandonedHomesHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<Sortie>().AsSelf().As<Mission>().SingleInstance();
            builder.RegisterType<SortieHandler>().As<IMissionHandler>().SingleInstance();
            builder.RegisterType<MissionDispatcher>().SingleInstance();
            builder.RegisterType<MissionProgressSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<ScavengingSystem>().AsSelf().As<ISimulationSystem>().SingleInstance();

            // Events
            builder.RegisterType<BetrayalFromWithinEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<BetrayalFromWithinEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<BlackMarketTraderEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<BlackMarketTraderEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<BurningFarmsEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<BurningFarmsEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<ChildrensPleaEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<ChildrensPleaEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<CouncilRevoltEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<CouncilRevoltEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<CrisisOfFaithEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<CrisisOfFaithEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<DespairEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<DespairEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<DesertionWaveEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<DesertionWaveEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<DissidentsDiscoveredEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<DissidentsDiscoveredEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<DistantHornsEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<DistantHornsEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<EnemyCommanderLetterEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<EnemyCommanderLetterEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<EnemyMessengerEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<EnemyMessengerEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<EnemySappersEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<EnemySappersEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<EnemyUltimatumEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<EnemyUltimatumEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<FeverOutbreakEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<FeverOutbreakEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<FinalAssaultEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<FinalAssaultEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<FireArtisanQuarterEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<FireArtisanQuarterEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<FortuneFavorsBoldEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<FortuneFavorsBoldEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<HealthImprovingEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<HealthImprovingEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<HungerRiotEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<HungerRiotEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<IntelSiegeWarningEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<IntelSiegeWarningEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<MilitiaVolunteersEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<MilitiaVolunteersEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<OpeningBombardmentEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<OpeningBombardmentEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<PlagueRatsEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<PlagueRatsEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<RefugeesAtGatesEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<RefugeesAtGatesEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<ReliefBannersEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<ReliefBannersEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<ReliefDustCloudsEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<ReliefDustCloudsEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<ReliefHornsEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<ReliefHornsEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SiegeBombardmentEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SiegeBombardmentEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SiegeEngineersArriveEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SiegeEngineersArriveEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SiegeTowersSpottedEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SiegeTowersSpottedEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SignalFireEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SignalFireEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SmugglerAtGateEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SmugglerAtGateEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SpySellingIntelEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SpySellingIntelEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SteadySuppliesEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SteadySuppliesEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<SupplyCartsInterceptedEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<SupplyCartsInterceptedEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<TaintedWellEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<TaintedWellEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<TotalCollapseEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<TotalCollapseEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<TyrantsReckoningEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<TyrantsReckoningEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<WallBreachEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<WallBreachEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<WallsStillStandEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<WallsStillStandEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<WellContaminationScareEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<WellContaminationScareEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<WorkerTakesLifeEvent>().AsSelf().As<GameEvent>().SingleInstance();
            builder.RegisterType<WorkerTakesLifeEventHandler>().As<IEventHandler>().SingleInstance();
            builder.RegisterType<EventDispatcher>().SingleInstance();
            builder.RegisterType<EventTriggerSystem>().As<ISimulationSystem>().SingleInstance();

            // Win/Loss Conditions
            builder.RegisterType<LossConditionSystem>().AsSelf().As<ISimulationSystem>().SingleInstance();
        }
    }
}