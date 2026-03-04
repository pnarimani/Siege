using AutofacUnity;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Siege.Gameplay
{
    /// <summary>
    /// Handles pause input (Space key) and delegates to GameClock.
    /// </summary>
    public class PauseInputHandler : MonoBehaviour
    {
        GameClock _clock;

        void Start()
        {
            _clock = Resolver.Resolve<GameClock>();
        }

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                _clock.TogglePause();
        }
    }
}
