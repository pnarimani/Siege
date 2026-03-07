using System;
using AutofacUnity;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Siege.Gameplay.UI
{
    public class UISystem : IDisposable
    {
        readonly AddressableUIRegistry _registry;
        Transform _root;

        static UISystem Self => Resolver.Resolve<UISystem>();

        public UISystem(AddressableUIRegistry registry)
        {
            _registry = registry;
            EnsureRoot();
            _registry.PreloadAllPrefabs().Forget();
        }

        public static T Open<T>(UILayer layer)
        {
            var self = Self;
            Debug.Assert(self != null, "UISystem is not installed.");
            
            var prefab = self._registry.GetPrefab<T>();
            if (prefab == null)
            {
                Debug.LogError("UISystem: No prefab registered for type " + typeof(T).Name);
                return default;
            }

            var parent = GetLayer(layer);
            Debug.Assert(parent != null, "UISystem: Invalid UILayer " + layer);
            var go = Object.Instantiate(prefab, parent);
            if (go.TryGetComponent<UIDocument>(out var doc))
                doc.sortingOrder = (int)layer + go.transform.GetSiblingIndex();
            return go.GetComponent<T>();
        }

        public static T GetOrOpen<T>(UILayer layer)
        {
            var self = Self;
            var prefab = self._registry.GetPrefab<T>();
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
            var self = Self;
            var instances = self._root.GetComponentsInChildren<T>(true);
            foreach (var inst in instances)
            {
                if (predicate == null || predicate(inst))
                    return inst;
            }

            return default;
        }

        public static Transform GetLayer(UILayer layer)
        {
            var self = Self;
            Debug.Assert(self != null, "UISystem is not installed.");
            
            self.EnsureRoot();
            
            return layer switch
            {
                UILayer.World => self._root.GetChild(0),
                UILayer.Screen => self._root.GetChild(1),
                UILayer.Window => self._root.GetChild(2),
                UILayer.Popup => self._root.GetChild(3),
                UILayer.Dragging => self._root.GetChild(4),
                UILayer.Overlay => self._root.GetChild(5),
                UILayer.Tooltip => self._root.GetChild(6),
                _ => null,
            };
        }

        void CreateLayer(UILayer uiLayer)
        {
            new GameObject(uiLayer.ToString()).transform.SetParent(_root, false);
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