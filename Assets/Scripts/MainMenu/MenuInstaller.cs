using Autofac;
using AutofacUnity;
using JetBrains.Annotations;

namespace Siege.MainMenu
{
    [UsedImplicitly]
    public class MenuInstaller : IMenuInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<MenuBootstrapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}