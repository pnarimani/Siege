using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using UnityEngine;

namespace Siege.Gameplay
{
    public class GameplayBootstrap : MonoBehaviour
    {
        SimulationRunner _runner;

        void Start()
        {
            _runner = gameObject.AddComponent<SimulationRunner>();

            // Register all simulation systems (order matters — this is the tick pipeline)
            var systems = Resolver.Resolve<IEnumerable<ISimulationSystem>>();
            foreach (var system in systems)
                _runner.RegisterSystem(system);

            // Initialize political state
            var political = Resolver.Resolve<PoliticalState>();
            political.Initialize();

            UISystem.Open<GameplayHUD>(UILayer.Screen);
        }
    }
}