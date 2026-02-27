using System;
using Gameplay;
using Siege.Gameplay.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Siege
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
