using UnityEngine;

namespace Gameplay
{
    public static class ComponentExtensions
    {
        public static T FindRecursive<T>(this Component parent, string name) where T : UnityEngine.Component
        {
            foreach (Transform child in parent.transform)
            {
                if (child.name == name)
                {
                    return child.GetComponent<T>();
                }

                T result = FindRecursive<T>(child, name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}