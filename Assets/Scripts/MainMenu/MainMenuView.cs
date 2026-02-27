using System;
using Gameplay;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Siege.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        void Awake()
        {
            var start = this.FindRecursive<Button>("#Start");
            start.onClick.AddListener(OnStartClicked);
        }

        void OnStartClicked()
        {
            Addressables.LoadSceneAsync("Game");
        }
    }
}