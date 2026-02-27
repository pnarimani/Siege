using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Siege.Gameplay.UI
{
    public class UISystem : IDisposable
    {
        readonly AddressableUIRegistry _registry;
        Transform _root;
        
        static UISystem _instance;

        public UISystem(AddressableUIRegistry registry)
        {
            _instance = this;
            _registry = registry;
            EnsureRoot();
            _registry.PreloadAllPrefabs().Forget();
        }

        public static T Open<T>(UILayer layer)
        {
            Debug.Assert(_instance != null, "UISystem instance is not set. Make sure to install UISystem in your project.");
            
            var prefab = _instance._registry.GetPrefab<T>();
            if (prefab == null)
            {
                Debug.LogError("UISystem: No prefab registered for type " + typeof(T).Name);
                return default;
            }

            var parent = GetLayer(layer);
            Debug.Assert(parent != null, "UISystem: Invalid UILayer " + layer);
            var instance = Object.Instantiate(prefab, parent);
            return instance.GetComponent<T>();
        }

        public static T GetOrOpen<T>(UILayer layer)
        {
            var prefab = _instance._registry.GetPrefab<T>();
            if (prefab == null)
            {
                Debug.LogError("UISystem: No prefab registered for type " + typeof(T).Name);
                return default;
            }

            var parent = GetLayer(layer);
            if (parent != null)
            {
                var existing = parent.GetComponentInChildren<T>(true);
                if (existing != null)
                    return existing;
            }

            return Open<T>(layer);
        }

        public static T GetExisting<T>(Func<T, bool> predicate = null)
        {
            var instances = _instance._root.GetComponentsInChildren<T>(true);
            foreach (var instance in instances)
            {
                if (predicate == null || predicate(instance))
                    return instance;
            }

            return default;
        }

        public static Transform GetLayer(UILayer layer)
        {
            Debug.Assert(_instance != null, "UISystem instance is not set. Make sure to install UISystem in your project.");
            
            _instance.EnsureRoot();
            
            return layer switch
            {
                UILayer.World => _instance._root.GetChild(0),
                UILayer.Screen => _instance._root.GetChild(1),
                UILayer.Window => _instance._root.GetChild(2),
                UILayer.Popup => _instance._root.GetChild(3),
                UILayer.Dragging => _instance._root.GetChild(4),
                UILayer.Overlay => _instance._root.GetChild(5),
                UILayer.Tooltip => _instance._root.GetChild(6),
                _ => null,
            };
        }

        void CreateLayer(UILayer uiLayer)
        {
            var transform = (RectTransform)new GameObject(uiLayer.ToString(), typeof(RectTransform)).transform;
            transform.SetParent(_root, false);
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            transform.localScale = Vector3.one;
        }

        void EnsureRoot()
        {
            if (_root)
                return;
            
            _root = new GameObject("UI Root").transform;
            
            CreateLayer(UILayer.World);
            CreateLayer(UILayer.Screen);
            CreateLayer(UILayer.Window);
            CreateLayer(UILayer.Popup);
            CreateLayer(UILayer.Dragging);
            CreateLayer(UILayer.Overlay);
            CreateLayer(UILayer.Tooltip);
        }

        public void Dispose()
        {
            if (_root != null)
                Object.Destroy(_root.gameObject);
        }
    }
}