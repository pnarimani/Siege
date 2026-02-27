using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Siege.Gameplay.UI
{
    [ExecuteAlways]
    public class LocalizeTextComponent : MonoBehaviour
    {
        public LocalizedString Text;
        private TextMeshProUGUI _tmp;

        private void OnEnable()
        {
            UpdateText();
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdateText();
        }
#endif

        private void UpdateText()
        {
            if (!_tmp)
                _tmp = GetComponent<TextMeshProUGUI>();

            if (Text is { IsEmpty: false })
            {
                if (LocalizationSettings.SelectedLocale == null)
                    LocalizationSettings.SelectedLocale = Locale.CreateLocale("en");
                var str = Text.GetLocalizedString();
                if (str != _tmp.text)
                    _tmp.text = str;
            }
        }
    }
}