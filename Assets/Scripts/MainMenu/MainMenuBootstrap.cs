using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.MainMenu
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("Opening Main Menu");
            UISystem.Open<MainMenuView>(UILayer.Window);
        }
    }
}