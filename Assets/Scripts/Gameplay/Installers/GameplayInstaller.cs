using Autofac;
using AutofacUnity;
using JetBrains.Annotations;

namespace Siege.Gameplay.Installers
{
    [UsedImplicitly]
    public class GameplayInstaller  : IGameplayInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<FlagsState>().SingleInstance();
            builder.RegisterType<GameBalance>().SingleInstance();
        }
    }
}