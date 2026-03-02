using System;

namespace Siege.Gameplay
{
    [Serializable]
    public struct ResourceQuantity
    {
        public ResourceType Resource;
        public double Quantity;

        public ResourceQuantity(ResourceType resource, double quantity)
        {
            Resource = resource;
            Quantity = quantity;
        }
    }
}