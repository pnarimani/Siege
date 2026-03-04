using System;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class NotificationPanel : IDisposable
    {
        const string InClass = "notification--in";
        const string OutClass = "notification--out";

        readonly VisualElement _list;
        readonly NotificationService _service;

        public NotificationPanel(VisualElement list, NotificationService service)
        {
            _list = list;
            _service = service;
            _service.Pushed += OnPushed;
        }

        public void Dispose()
        {
            _service.Pushed -= OnPushed;
        }

        void OnPushed(NotificationData data)
        {
            var item = BuildItem(data);
            _list.Add(item);

            // Trigger enter animation on next frame (transitions don't fire on first frame)
            item.schedule.Execute(() => item.AddToClassList(InClass));

            // Schedule fade-out after lifetime
            long lifetimeMs = (long)(data.Lifetime * 1000);
            item.schedule.Execute(() => FadeOut(item)).StartingIn(lifetimeMs);
        }

        void FadeOut(VisualElement item)
        {
            item.RemoveFromClassList(InClass);
            item.AddToClassList(OutClass);

            bool removed = false;
            item.RegisterCallback<TransitionEndEvent>(evt =>
            {
                if (!removed && evt.stylePropertyNames.Contains("opacity"))
                {
                    removed = true;
                    item.RemoveFromHierarchy();
                }
            });
        }

        VisualElement BuildItem(NotificationData data)
        {
            var item = new VisualElement();
            item.AddToClassList("notification");
            item.pickingMode = PickingMode.Ignore;

            if (data.Icon != null)
            {
                var icon = new Image { sprite = data.Icon };
                icon.AddToClassList("notification__icon");
                icon.pickingMode = PickingMode.Ignore;
                item.Add(icon);
            }

            var body = new VisualElement();
            body.AddToClassList("notification__body");
            body.pickingMode = PickingMode.Ignore;

            var title = new Label(data.Title);
            title.AddToClassList("notification__title");
            title.pickingMode = PickingMode.Ignore;
            body.Add(title);

            if (!string.IsNullOrEmpty(data.Text))
            {
                var text = new Label(data.Text);
                text.AddToClassList("notification__text");
                text.pickingMode = PickingMode.Ignore;
                body.Add(text);
            }

            item.Add(body);
            return item;
        }
    }
}
