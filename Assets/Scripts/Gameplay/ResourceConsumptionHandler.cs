using System;
using System.Linq;
using UnityEngine;

namespace Siege.Gameplay
{
    public class ResourceConsumptionHandler
    {
        readonly GameBalance _gameBalance;

        public ResourceConsumptionHandler(GameBalance gameBalance)
        {
            _gameBalance = gameBalance;
        }

        public bool TryConsumeResources(ResourceQuantity[] requiredResources)
        {
            var storages = UnityEngine.Object.FindObjectsByType<Building>(UnityEngine.FindObjectsSortMode.None)
                .Where(b => b.Id == BuildingId.Storage)
                .OrderBy(b => b.GetDefinition().Zone)
                .ToList();

            foreach (var required in requiredResources)
            {
                var total = storages.Sum(s => s.Resources
                    .Where(r => r.Resource == required.Resource)
                    .Sum(r => r.Quantity));

                if (total < required.Quantity)
                    return false;
            }

            foreach (var required in requiredResources)
            {
                var remaining = required.Quantity;
                foreach (var storage in storages)
                {
                    if (remaining <= 0) break;
                    var available = storage.Resources
                        .FirstOrDefault(r => r.Resource == required.Resource).Quantity;
                    var toDeduct = Math.Min(available, remaining);
                    if (toDeduct > 0)
                    {
                        storage.Remove(required.Resource, toDeduct);
                        remaining -= toDeduct;
                    }
                }
            }

            return true;
        }
    }
}