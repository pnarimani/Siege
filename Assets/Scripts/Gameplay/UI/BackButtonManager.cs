using System;
using System.Collections.Generic;

namespace Siege.Gameplay.UI
{
    public class BackButtonManager : IDisposable
    {
        readonly List<IBackButtonHandler> _handlers = new();
        readonly PlayerInputActions _inputs;

        public BackButtonManager()
        {
            _inputs = new PlayerInputActions();
            _inputs.Enable();
            _inputs.UI.Enable();
            _inputs.UI.Cancel.performed += _ => HandleBackButtonPressed();
        }

        void HandleBackButtonPressed()
        {
            _handlers.RemoveAll(x => x == null);

            if (_handlers.Count == 0)
                return;

            _handlers[^1].OnBackButtonPressed();
        }

        public void PushHandler(IBackButtonHandler handler)
        {
            _handlers.Remove(handler);
            _handlers.Add(handler);
        }

        public void PopHandler(IBackButtonHandler handler)
        {
            _handlers.Remove(handler);
        }

        public void Dispose()
        {
            _inputs?.Dispose();
        }
    }
}