using Autofac;
using AutofacUnity;
using JetBrains.Annotations;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Defense;
using Siege.Gameplay.Events;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Missions;
using Siege.Gameplay.Political;
using Siege.Gameplay.Population;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Siege;
using Siege.Gameplay.Simulation;
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

            // Laws
            builder.RegisterType<LawManager>().SingleInstance();
            builder.RegisterType<LawEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Orders
            builder.RegisterType<Orders.OrderManager>().SingleInstance();
            builder.RegisterType<Orders.OrderEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Missions
            builder.RegisterType<MissionManager>().SingleInstance();
            builder.RegisterType<MissionProgressSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<ScavengingSystem>().AsSelf().As<ISimulationSystem>().SingleInstance();

            // Events
            builder.RegisterType<EventManager>().SingleInstance();
            builder.RegisterType<EventTriggerSystem>().As<ISimulationSystem>().SingleInstance();
        }
    }
}