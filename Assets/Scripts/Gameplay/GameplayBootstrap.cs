using System;
using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Political;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.UI;
using Siege.Gameplay.Zones;
using UnityEngine;

namespace Siege.Gameplay
{
    public class GameplayBootstrap : MonoBehaviour
    {
        SimulationRunner _runner;

        void Start()
        {
            SetupScene();

            var state = Resolver.Resolve<GameState>();
            state.Initialize();

            _runner = gameObject.AddComponent<SimulationRunner>();

            var systems = Resolver.Resolve<IEnumerable<ISimulationSystem>>();
            foreach (var system in systems)
                _runner.RegisterSystem(system);

            var political = Resolver.Resolve<PoliticalState>();
            political.Initialize();

            // Distribute population across zones and auto-allocate workers
            var zoneManager = Resolver.Resolve<ZoneManager>();
            zoneManager.DistributePopulation();

            var workerAllocation = Resolver.Resolve<WorkerAllocation>();
            workerAllocation.AutoAllocate();

            SeedStartingResources();

            UISystem.Open<GameplayHUD>(UILayer.Screen);
        }

        // ── Scene Setup ───────────────────────────────────────────────

        /// <summary>
        /// Creates zone and building GameObjects with placeholder geometry.
        /// Finds existing zone parents by name under a "Zones" root, or creates them.
        /// </summary>
        void SetupScene()
        {
            var zonesRoot = GameObject.Find("Zones");
            if (zonesRoot == null)
            {
                zonesRoot = new GameObject("Zones");
            }

            foreach (var def in BuildingDefinition.All.Values)
            {
                var zoneName = def.Zone.ToString();
                var zoneTransform = zonesRoot.transform.Find(zoneName);

                if (zoneTransform == null)
                {
                    var zoneGo = new GameObject(zoneName);
                    zoneGo.transform.SetParent(zonesRoot.transform);
                    zoneGo.transform.localPosition = GetZonePosition(def.Zone);
                    zoneTransform = zoneGo.transform;
                }

                // Ensure Zone component exists
                if (zoneTransform.GetComponent<Zone>() == null)
                    zoneTransform.gameObject.AddComponent<Zone>();

                // Create building if it doesn't exist
                var buildingName = def.Type.ToString();
                var buildingTransform = zoneTransform.Find(buildingName);
                if (buildingTransform == null)
                {
                    var buildingGo = CreatePlaceholderBuilding(buildingName);
                    buildingGo.transform.SetParent(zoneTransform);
                    buildingGo.transform.localPosition = GetBuildingOffset(def.Type);
                    buildingTransform = buildingGo.transform;
                }

                // Ensure Building component exists
                if (buildingTransform.GetComponent<Building>() == null)
                    buildingTransform.gameObject.AddComponent<Building>();

                // Ensure click handler for selection
                if (buildingTransform.GetComponent<BuildingClickHandler>() == null)
                    buildingTransform.gameObject.AddComponent<BuildingClickHandler>();

                // Add StorageBuilding if flagged
                if (def.IsStorage && buildingTransform.GetComponent<StorageBuilding>() == null)
                    buildingTransform.gameObject.AddComponent<StorageBuilding>();

                // Add ProductionCycleState to non-storage buildings
                if (!def.IsStorage && buildingTransform.GetComponent<ProductionCycleState>() == null)
                    buildingTransform.gameObject.AddComponent<ProductionCycleState>();

                // Ensure collider for selection
                if (buildingTransform.GetComponent<Collider>() == null)
                    buildingTransform.gameObject.AddComponent<BoxCollider>();
            }

            // Refresh building lists on all zones
            foreach (var zone in Zone.All)
                zone.RefreshBuildings();
        }

        static GameObject CreatePlaceholderBuilding(string name)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.localScale = new Vector3(2f, 3f, 2f);
            go.transform.localPosition = new Vector3(0, 1.5f, 0);
            return go;
        }

        static Vector3 GetZonePosition(ZoneId zone) => zone switch
        {
            ZoneId.OuterFarms       => new Vector3(0, 0, 20),
            ZoneId.OuterResidential => new Vector3(0, 0, 10),
            ZoneId.ArtisanQuarter   => new Vector3(0, 0, 0),
            ZoneId.InnerDistrict    => new Vector3(0, 0, -10),
            ZoneId.Keep             => new Vector3(0, 0, -20),
            _                       => Vector3.zero,
        };

        static Vector3 GetBuildingOffset(BuildingType type) => type switch
        {
            BuildingType.Farm         => new Vector3(-4, 1.5f, 0),
            BuildingType.HerbGarden   => new Vector3( 4, 1.5f, 0),
            BuildingType.Well         => new Vector3(-6, 1.5f, 0),
            BuildingType.FuelStore    => new Vector3( 0, 1.5f, 0),
            BuildingType.FieldKitchen => new Vector3( 6, 1.5f, 0),
            BuildingType.Workshop     => new Vector3(-6, 1.5f, 0),
            BuildingType.Smithy       => new Vector3( 0, 1.5f, 0),
            BuildingType.Cistern      => new Vector3( 6, 1.5f, 0),
            BuildingType.Clinic       => new Vector3(-6, 1.5f, 0),
            BuildingType.Storehouse   => new Vector3(-2, 1.5f, 0),
            BuildingType.RootCellar   => new Vector3( 2, 1.5f, 0),
            BuildingType.TradingPost  => new Vector3( 6, 1.5f, 0),
            BuildingType.RepairYard   => new Vector3(-4, 1.5f, 0),
            BuildingType.RationingPost=> new Vector3( 4, 1.5f, 0),
            _                         => new Vector3(0, 1.5f, 0),
        };

        // ── Resource Seeding ──────────────────────────────────────────

        void SeedStartingResources()
        {
            var state = Resolver.Resolve<GameState>();
            var storages = StorageBuilding.All;
            if (storages.Count == 0) return;

            DistributeToStorage(storages, ResourceType.Food, state.Food);
            DistributeToStorage(storages, ResourceType.Water, state.Water);
            DistributeToStorage(storages, ResourceType.Fuel, state.Fuel);
            DistributeToStorage(storages, ResourceType.Medicine, state.Medicine);
            DistributeToStorage(storages, ResourceType.Materials, state.Materials);
        }

        static void DistributeToStorage(IReadOnlyList<StorageBuilding> storages, ResourceType type, double amount)
        {
            double perStorage = amount / storages.Count;
            foreach (var s in storages)
                s.Deposit(type, perStorage);
        }
    }
}