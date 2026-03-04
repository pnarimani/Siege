using AutofacUnity;
using UnityEngine;

namespace Siege.Gameplay.Simulation
{
    /// <summary>
    /// Thin MonoBehaviour that drives SimulationRunner each frame.
    /// This is the only frame-driving MonoBehaviour for the simulation.
    /// </summary>
    public class TickRunner : MonoBehaviour
    {
        SimulationRunner _runner;

        public void Register(SimulationRunner runner)
        {
            _runner = runner;
        }

        void Update()
        {
            if (_runner != null)
                _runner.Tick(Time.deltaTime);
        }
    }
}
