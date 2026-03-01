using System;

namespace Siege.Gameplay
{
    [Serializable]
    public struct ResourceQuantity
    {
        public ResourceKind Resource;
        public double Quantity;

        public ResourceQuantity(ResourceKind resource, double quantity)
        {
            Resource = resource;
            Quantity = quantity;
        }
    }
}