using Autofac;
using AutofacUnity;
using JetBrains.Annotations;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Installers
{
    [UsedImplicitly]
    public class GameplayInstaller : IGameplayInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<GameState>().SingleInstance();
            builder.RegisterType<GameClock>().SingleInstance();
            builder.RegisterType<ChangeLog>().SingleInstance();
            builder.RegisterType<PoliticalState>().SingleInstance();
            builder.RegisterType<PoliticalDecaySystem>().As<ISimulationSystem>().SingleInstance();
        }
    }
}