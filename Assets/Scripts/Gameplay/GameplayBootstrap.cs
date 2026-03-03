using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay
{
    public class GameplayBootstrap : MonoBehaviour
    {
        void Start()
        {
            gameObject.AddComponent<ProductionSystem>();
            UISystem.Open<GameplayHUD>(UILayer.Screen);
            WorkerAllocation.Reallocate();
        }
    }
}