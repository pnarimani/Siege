using UnityEngine;
using UnityEngine.UI;

namespace Siege.Gameplay.UI
{
    public class IconProgressBar : MonoBehaviour
    {
        ProgressBar _progressBar;
        Image _icon;

        void Awake()
        {
            _progressBar = GetComponentInChildren<ProgressBar>();
            _icon = this.FindRecursive<Image>("#Icon");
        }
    }
}
