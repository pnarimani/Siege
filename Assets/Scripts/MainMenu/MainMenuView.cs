using System;
using Siege.Gameplay;
using Siege.Gameplay.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Siege.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        void Awake()
        {
            this.FindElement<SiegeButton>("start").Clicked += OnStartClicked;
        }

        void OnStartClicked()
        {
            Addressables.LoadSceneAsync("Game");
        }
    }
}