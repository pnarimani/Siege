using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay
{
    public class GameplayBootstrap : MonoBehaviour
    {
        void Start()
        {
            UISystem.Open<GameplayHUD>(UILayer.Screen);
            UISystem.Open<BuildingView>(UILayer.Window);
        }
    }
}