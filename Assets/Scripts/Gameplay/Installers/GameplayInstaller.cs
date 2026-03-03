using Autofac;
using AutofacUnity;
using JetBrains.Annotations;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Political;
using Siege.Gameplay.Resources;
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
        }
    }
}