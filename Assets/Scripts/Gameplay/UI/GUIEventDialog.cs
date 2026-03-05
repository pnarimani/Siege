using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIEventDialog : MonoBehaviour
    {
        UIDocument _document;
        VisualElement _root;
        Label _title;
        Label _description;
        VisualElement _changesContainer;
        VisualElement _responseContainer;

        DialogContent _current;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _title = root.Q<Label>("Title");
            _description = root.Q<Label>("Description");
            _changesContainer = root.Q("ChangesContainer");
            _responseContainer = root.Q("ResponseContainer");

            _root.style.display = DisplayStyle.None;
        }

        public void Show(DialogContent content)
        {
            _current = content;
            _title.text = content.Title;

            bool hasDescription = !string.IsNullOrEmpty(content.Description);
            _description.text = hasDescription ? content.Description : "";
            _description.style.display = hasDescription ? DisplayStyle.Flex : DisplayStyle.None;

            _changesContainer.Clear();
            if (content.Changes != null && content.Changes.Count > 0)
            {
                foreach (var change in content.Changes)
                {
                    var label = new Label(StateChangeFormatter.Format(change));
                    label.AddToClassList("event-dialog__change-entry");
                    _changesContainer.Add(label);
                }
                _changesContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _changesContainer.style.display = DisplayStyle.None;
            }

            _responseContainer.Clear();
            if (content.Responses != null && content.Responses.Length > 0)
            {
                for (int i = 0; i < content.Responses.Length; i++)
                {
                    int index = i;
                    var opt = content.Responses[i];
                    var btn = new SiegeButton { Text = opt.Label };
                    btn.AddToClassList("event-dialog__response-btn");
                    if (!string.IsNullOrEmpty(opt.Tooltip))
                        btn.tooltip = opt.Tooltip;
                    btn.Clicked += () => content.OnRespond?.Invoke(index);
                    _responseContainer.Add(btn);
                }
            }
            else
            {
                var btn = new SiegeButton { Text = "OK" };
                btn.AddToClassList("event-dialog__continue-btn");
                btn.Clicked += () => content.OnDismiss?.Invoke();
                _responseContainer.Add(btn);
            }

            _root.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            _root.style.display = DisplayStyle.None;
            _current = null;
        }
    }
}
