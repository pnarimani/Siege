using System.Reflection;
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
            var asm = Assembly.GetExecutingAssembly();

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

            // Laws (auto-scan)
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(ILaw).IsAssignableFrom(t)).AsSelf().As<ILaw>().SingleInstance();
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(ILawHandler).IsAssignableFrom(t)).As<ILawHandler>().SingleInstance();
            builder.RegisterType<LawDispatcher>().SingleInstance();
            builder.RegisterType<LawEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Orders (auto-scan)
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(IOrder).IsAssignableFrom(t)).AsSelf().As<IOrder>().SingleInstance();
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(IOrderHandler).IsAssignableFrom(t)).As<IOrderHandler>().SingleInstance();
            builder.RegisterType<OrderDispatcher>().SingleInstance();
            builder.RegisterType<OrderEffectSystem>().As<ISimulationSystem>().SingleInstance();

            // Missions (auto-scan)
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(IMission).IsAssignableFrom(t)).AsSelf().As<IMission>().SingleInstance();
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(IMissionHandler).IsAssignableFrom(t)).As<IMissionHandler>().SingleInstance();
            builder.RegisterType<MissionDispatcher>().SingleInstance();
            builder.RegisterType<MissionProgressSystem>().As<ISimulationSystem>().SingleInstance();
            builder.RegisterType<ScavengingSystem>().AsSelf().As<ISimulationSystem>().SingleInstance();

            // Events (auto-scan)
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(IGameEvent).IsAssignableFrom(t)).AsSelf().As<IGameEvent>().SingleInstance();
            builder.RegisterAssemblyTypes(asm).Where(t => typeof(IEventHandler).IsAssignableFrom(t)).As<IEventHandler>().SingleInstance();
            builder.RegisterType<EventDispatcher>().SingleInstance();
            builder.RegisterType<EventTriggerSystem>().As<ISimulationSystem>().SingleInstance();

            // Win/Loss Conditions
            builder.RegisterType<LossConditionSystem>().AsSelf().As<ISimulationSystem>().SingleInstance();
        }
    }
}