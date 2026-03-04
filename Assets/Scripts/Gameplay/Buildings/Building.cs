using System;
using AutofacUnity;
using Siege.Gameplay.Zones;
using UnityEngine;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// A building placed in the 3D world. Produces/consumes resources based on assigned workers.
    /// BuildingType is derived from the GameObject name (must match enum name).
    /// </summary>
    public class Building : MonoBehaviour
    {
        BuildingRegistry _registry;

        // ── Runtime State ─────────────────────────────────────────────
        public BuildingType Type { get; private set; }
        public BuildingDefinition Definition { get; private set; }
        public Zone Zone { get; private set; }

        public int MaxWorkers => Definition.MaxWorkers;
        public int AssignedWorkers { get; set; }
        public bool IsActive { get; set; }
        public SpecializationId Specialization { get; private set; } = SpecializationId.None;
        public bool IsSpecialized => Specialization != SpecializationId.None;

        /// <summary>
        /// True if this building requires repair before it can function (e.g., Trading Post).
        /// </summary>
        public bool NeedsRepair { get; set; }

        /// <summary>
        /// Fired when building is selected by the player in 3D.
        /// </summary>
        public static event Action<Building> Selected;

        // ── Lifecycle ─────────────────────────────────────────────────

        void OnEnable() => _registry?.Register(this);
        void OnDisable() => _registry?.Unregister(this);

        void Awake()
        {
            _registry = Resolver.Resolve<BuildingRegistry>();
            var definitions = Resolver.Resolve<BuildingDefinitionService>();
            Type = Enum.Parse<BuildingType>(gameObject.name);
            Definition = definitions.Get(Type);
            Zone = GetComponentInParent<Zone>();
            IsActive = !Definition.RequiresRepair;
            NeedsRepair = Definition.RequiresRepair;
        }

        // ── Production ────────────────────────────────────────────────

        public ResourceQuantity[] GetCurrentInputs()
        {
            if (IsSpecialized)
            {
                var spec = SpecializationDefinition.Get(Type, Specialization);
                if (spec != null) return spec.ModifiedInputs;
            }
            return Definition.Inputs;
        }

        public ResourceQuantity[] GetCurrentOutputs()
        {
            if (IsSpecialized)
            {
                var spec = SpecializationDefinition.Get(Type, Specialization);
                if (spec != null) return spec.ModifiedOutputs;
            }
            return Definition.Outputs;
        }

        public SpecializationDefinition GetSpecializationDef()
        {
            return IsSpecialized ? SpecializationDefinition.Get(Type, Specialization) : null;
        }

        // ── Specialization ────────────────────────────────────────────

        public void ApplySpecialization(SpecializationId id)
        {
            if (IsSpecialized) return;
            Specialization = id;
        }

        // ── Selection ─────────────────────────────────────────────────

        public void Select() => Selected?.Invoke(this);
    }
}
