using Autofac;
using AutofacUnity;
using JetBrains.Annotations;
using Siege.Gameplay.Buildings;

namespace Siege.Gameplay
{
    [UsedImplicitly]
    public class GameplayInstaller : IGameplayInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<GameplayBootstrapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GameData>().SingleInstance();
            builder.RegisterType<BuildingAssets>().SingleInstance();
        }
    }
}