using Unity.Properties;
using UnityEngine;

namespace Siege.Gameplay.UI
{
    public class ResourceBuildingView : MonoBehaviour
    {
        void Awake()
        {

        }

        public class ResourceBuildingViewModel
        {

            [CreateProperty]
            public ResourceQuantity Fuel, Food, Water, Medicine, Materials;
        }
    }
}