using Siege.Gameplay;
using Siege.Gameplay.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

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