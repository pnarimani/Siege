using System.Collections.Generic;

namespace Siege.Gameplay.UI
{
    public class BackButtonManager
    {
        readonly IUIBackButtonInput _input;
        readonly List<IBackButtonHandler> _handlers = new();

        public BackButtonManager(IUIBackButtonInput input)
        {
            _input = input;
            _input.OnBackButtonPressed += HandleBackButtonPressed;
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

        public void PopHandler(IBackButtonHandler handler) { _handlers.Remove(handler); }
    }
}