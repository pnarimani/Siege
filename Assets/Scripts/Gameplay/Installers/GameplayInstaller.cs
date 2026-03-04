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
using TypeRegistry;

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
            builder.RegisterType<SimulationRunner>().SingleInstance();

            // Political
            builder.RegisterType<PoliticalState>().SingleInstance();
            builder.RegisterType<PoliticalDecaySystem>().As<ISimulationSystem>().SingleInstance();

            // Zones & Buildings
            builder.RegisterType<ZoneManager>().SingleInstance();
            builder.RegisterType<ZoneRegistry>().SingleInstance();
            builder.RegisterType<BuildingRegistry>().SingleInstance();
            builder.RegisterType<StorageBuildingRegistry>().SingleInstance();
            builder.RegisterType<BuildingDefinitionService>().SingleInstance();
            builder.RegisterType<BuildingService>().SingleInstance();
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

            // Laws (TypeRegistry scan)
            RegisterDerivedTypes<ILaw>(builder, asBase: true);
            RegisterDerivedTypes<ILawHandler>(builder, asBase: true);
            builder.RegisterType<LawDispatcher>().SingleInstance();
            builder.RegisterType<LawEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Orders (TypeRegistry scan)
            RegisterDerivedTypes<IOrder>(builder, asBase: true);
            RegisterDerivedTypes<IOrderHandler>(builder, asBase: true);
            builder.RegisterType<OrderDispatcher>().SingleInstance();
            builder.RegisterType<OrderEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Missions (TypeRegistry scan)
            RegisterDerivedTypes<IMission>(builder, asBase: true);
            RegisterDerivedTypes<IMissionHandler>(builder, asBase: true);
            builder.RegisterType<MissionDispatcher>().SingleInstance();
            builder.RegisterType<MissionProgressSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<ScavengingSystem>().AsSelf().As<ISimulationSystem>().SingleInstance();

            // Events (TypeRegistry scan)
            RegisterDerivedTypes<IGameEvent>(builder, asBase: true);
            RegisterDerivedTypes<IEventHandler>(builder, asBase: true);
            builder.RegisterType<EventDispatcher>().SingleInstance();
            builder.RegisterType<EventTriggerSystem>().As<ISimulationSystem>().SingleInstance();

            // Win/Loss Conditions
            builder.RegisterType<LossConditionSystem>().AsSelf().As<ISimulationSystem>().SingleInstance();
        }

        static void RegisterDerivedTypes<T>(ContainerBuilder builder, bool asBase)
        {
            var types = TypeLookup.GetTypesDerivedFrom<T>();
            foreach (var type in types)
            {
                if (type.IsAbstract) continue;
                var reg = builder.RegisterType(type).AsSelf();
                if (asBase) reg.As<T>();
                reg.SingleInstance();
            }
        }
    }
}