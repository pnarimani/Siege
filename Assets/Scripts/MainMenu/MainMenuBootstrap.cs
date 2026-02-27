using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.MainMenu
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        void Start()
        {
            UISystem.Open<MainMenuView>(UILayer.Window);
        }
    }
}