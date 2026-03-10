using UnityEngine.AddressableAssets;

namespace Siege.UI
{
    public class UISystem
    {
        public T Open<T>()
            => Addressables.InstantiateAsync($"Content/UI/{typeof(T).Name}.prefab")
                .WaitForCompletion()
                .GetComponent<T>();
    }
}