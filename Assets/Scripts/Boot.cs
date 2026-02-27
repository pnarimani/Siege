using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Siege
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private string _addressableSceneName = "MainMenu";

        void Start()
        {
            Debug.Log("Loading");
            Addressables.LoadSceneAsync(_addressableSceneName);
        }
    }
}
