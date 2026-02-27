using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Siege.Gameplay.UI
{
    [CreateAssetMenu(menuName = "Siege/AddressableUIRegistry", fileName = "UIRegistry", order = 0)]
    public class AddressableUIRegistry : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<Entry> _entries = new();

        readonly Dictionary<string, string> _addressByType = new();

        public async Task PreloadAllPrefabs()
        {
            var handles = new List<Task>();
            foreach (var address in _addressByType.Values)
                handles.Add(Addressables.LoadAssetAsync<GameObject>(address).Task);

            await Task.WhenAll(handles);
        }

        public GameObject GetPrefab<T>()
        {
            var t = typeof(T);
            return Addressables.LoadAssetAsync<GameObject>(_addressByType[t.FullName ?? t.Name]).WaitForCompletion();
        }

        [Serializable]
        public struct Entry
        {
            public string Type;
            public string Address;

            public Entry(string type, string address)
            {
                Type = type;
                Address = address;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _entries.Clear();
            foreach (var kvp in _addressByType)
                _entries.Add(new Entry { Type = kvp.Key, Address = kvp.Value });
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _addressByType.Clear();
            foreach (var entry in _entries)
                _addressByType[entry.Type] = entry.Address;
        }
    }
}