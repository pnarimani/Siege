using System.Collections.Generic;
using System.Linq;
using Siege.Gameplay.UI;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace SurvivalGame.UI.Editor
{
    public class AddressableUIRegistryRefresher
    {
        public static void RefreshAsset(AddressableUIRegistry asset)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("Addressable settings not found.");
                return;
            }

            // type full name -> list of addresses that contain it
            var typeToAddresses = new Dictionary<string, List<string>>();

            var allEntries = new List<AddressableAssetEntry>();
            foreach (var group in settings.groups)
            {
                if (group == null) continue;
                foreach (var entry in group.entries)
                    entry.GatherAllAssets(allEntries, true, true, false);
            }

            foreach (var entry in allEntries)
            {
                if (!entry.AssetPath.Contains("Content/Prefabs/UI")) continue;

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(entry.AssetPath);
                if (prefab == null) continue;

                foreach (var component in prefab.GetComponentsInChildren<Component>(true))
                {
                    if (component == null) continue;
                    var t = component.GetType();
                    var ns = t.Namespace ?? "";
                    if (ns.StartsWith("UnityEngine") || ns.StartsWith("UnityEditor") || ns.StartsWith("TMPro")) continue;

                    var typeName = t.FullName ?? t.Name;
                    if (!typeToAddresses.TryGetValue(typeName, out var list))
                        typeToAddresses[typeName] = list = new List<string>();
                    if (!list.Contains(entry.address))
                        list.Add(entry.address);
                }
            }

            var so = new SerializedObject(asset);
            var entriesProp = so.FindProperty("_entries");
            entriesProp.ClearArray();

            var index = 0;
            foreach (var kvp in typeToAddresses.Where(kvp => kvp.Value.Count == 1))
            {
                entriesProp.InsertArrayElementAtIndex(index);
                var elem = entriesProp.GetArrayElementAtIndex(index++);
                elem.FindPropertyRelative("Type").stringValue = kvp.Key;
                elem.FindPropertyRelative("Address").stringValue = kvp.Value[0];
            }

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log($"[AddressableUIRegistry] Refreshed with {index} entries.");
        }
    }
}