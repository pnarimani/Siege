using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Siege
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private string _addressableSceneName = "MainMenu";

        void Start()
        {
            Addressables.LoadSceneAsync(_addressableSceneName);
        }
    }
}
