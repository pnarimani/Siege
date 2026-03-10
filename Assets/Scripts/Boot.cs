using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Siege
{
    public class Boot : MonoBehaviour
    {
        void Start()
        {
            Addressables.LoadSceneAsync("MainMenu");
        }
    }
}