using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;

namespace Siege.Gameplay.Buildings
{
    public class BuildingAssets
    {
        public GameObject Spawn(string buildingId)
        {
            return Addressables.InstantiateAsync($"Content/Buildings/{buildingId}/{buildingId}.prefab").WaitForCompletion();
        } 
        
        public Texture2D GetIcon(string buildingId)
        {
            return Addressables.LoadAssetAsync<Texture2D>($"Content/Buildings/{buildingId}/{buildingId}.png").WaitForCompletion();
        }
        
        public string GetName(string buildingId)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString("Buildings", buildingId);
        }
    }
}