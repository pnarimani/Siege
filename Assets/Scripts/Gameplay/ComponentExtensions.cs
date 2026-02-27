using UnityEngine;
using UnityEngine.UIElements;

namespace Gameplay
{
    public static class ComponentExtensions
    {
        public static T FindRecursive<T>(this Component parent, string name) where T : Component
        {
            foreach (Transform child in parent.transform)
            {
                if (child.name == name)
                    return child.GetComponent<T>();

                var result = child.FindRecursive<T>(name);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static Transform FindRecursive(this Component parent, string name) =>
            parent.FindRecursive<Transform>(name);

        public static T FindElement<T>(this Component parent, string name) where T : VisualElement
        {
            var document = parent.GetComponent<UIDocument>();
            return document != null ? document.rootVisualElement.Q<T>(name) : null;
        }
    }
}