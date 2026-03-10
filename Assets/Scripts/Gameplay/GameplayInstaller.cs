using Autofac;
using AutofacUnity;
using JetBrains.Annotations;

namespace Siege.Gameplay
{
    [UsedImplicitly]
    public class GameplayInstaller : IGameplayInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<GameplayBootstrapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}