using System;
using UnityEngine;

namespace Siege.Gameplay.UI
{
    public class NotificationData
    {
        public string Title;
        public string Text;
        public Sprite Icon;
        public float Lifetime;
    }

    public class NotificationService
    {
        public event Action<NotificationData> Pushed;

        public void Push(string title, string text, Sprite icon = null, float lifetime = 5f)
        {
            Pushed?.Invoke(new NotificationData
            {
                Title = title,
                Text = text,
                Icon = icon,
                Lifetime = lifetime,
            });
        }
    }
}
