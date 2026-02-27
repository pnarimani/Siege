using System.Collections.Generic;
using Siege.Gameplay.UI;
using UnityEngine;

namespace SurvivalGame.UI.Editor
{
    [UnityEditor.CustomEditor(typeof(AddressableUIRegistry))]
    public class AddressableUIRegistryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Auto Fill Addresses"))
                AddressableUIRegistryRefresher.RefreshAsset((AddressableUIRegistry)target);
            
            DrawDefaultInspector();
        }
    }
}