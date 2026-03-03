using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Siege.Gameplay
{
    public class ResourceManagement
    {
        readonly GameBalance _gameBalance;

        public ResourceManagement(GameBalance gameBalance)
        {
            _gameBalance = gameBalance;
        }

        public bool TryConsumeResources(ResourceQuantity[] requiredResources)
        {
            var storages = Object.FindObjectsByType<Building>(FindObjectsSortMode.None)
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

        public void ProduceResources(ResourceQuantity[] output)
        {
            var storages = Object.FindObjectsByType<Building>(FindObjectsSortMode.None)
                .Where(b => b.Id == BuildingId.Storage)
                .OrderBy(b => b.GetDefinition().Zone)
                .ToList();

            foreach (var produced in output)
            {
                var remaining = produced.Quantity;
                foreach (var storage in storages)
                {
                    if (remaining <= 0) break;
                    var capacity = _gameBalance.StorageBaseCapacity;
                    var current = storage.Resources.FirstOrDefault(r => r.Resource == produced.Resource).Quantity;
                    var availableSpace = capacity - current;
                    var toAdd = Math.Min(availableSpace, remaining);
                    if (toAdd > 0)
                    {
                        storage.Add(produced.Resource, (int)toAdd);
                        remaining -= toAdd;
                    }
                }
            }
        }
    }
}