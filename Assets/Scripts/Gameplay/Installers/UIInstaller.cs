using Autofac;
using AutofacUnity;
using JetBrains.Annotations;
using Siege.Gameplay.UI;
using UnityEngine.AddressableAssets;

namespace Gameplay.Installers
{
    [UsedImplicitly]
    public class UIInstaller : IProjectInstaller
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterInstance(Addressables.LoadAssetAsync<AddressableUIRegistry>("UIRegistry")
                    .WaitForCompletion())
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<UISystem>()
                .SingleInstance()
                .AutoActivate()
                .OnActivated(x => UISystem.SetInstance(x.Instance));
        }
    }
}