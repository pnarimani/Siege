using Autofac;
using AutofacUnity;
using JetBrains.Annotations;

namespace Siege.UI
{
    [UsedImplicitly]
    public class UIInstaller : IProjectInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<UISystem>().SingleInstance();
        }
    }
}