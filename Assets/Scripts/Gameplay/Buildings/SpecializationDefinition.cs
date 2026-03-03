using System.Collections.Generic;

namespace Siege.Gameplay.Buildings
{
    /// <summary>
    /// Defines the two specialization options for each building type and their modified stats.
    /// Specialization is irreversible and costs resources (gated behind a law).
    /// </summary>
    public class SpecializationDefinition
    {
        public readonly BuildingType BuildingType;
        public readonly SpecializationId Id;
        public readonly string Name;
        public readonly string Description;
        public readonly ResourceQuantity[] ModifiedInputs;
        public readonly ResourceQuantity[] ModifiedOutputs;
        public readonly ResourceQuantity[] Cost; // one-time cost to specialize

        // Optional passive effects (applied by systems that check specialization)
        public readonly double SicknessPerDay;
        public readonly double MoralePerDay;
        public readonly double UnrestPerDay;
        public readonly double FoodConsumptionModifier;  // e.g. -0.15 = -15%
        public readonly double FuelConsumptionModifier;

        public SpecializationDefinition(
            BuildingType buildingType, SpecializationId id, string name, string description,
            ResourceQuantity[] modifiedInputs, ResourceQuantity[] modifiedOutputs,
            ResourceQuantity[] cost = null,
            double sicknessPerDay = 0, double moralePerDay = 0, double unrestPerDay = 0,
            double foodConsumptionModifier = 0, double fuelConsumptionModifier = 0)
        {
            BuildingType = buildingType;
            Id = id;
            Name = name;
            Description = description;
            ModifiedInputs = modifiedInputs;
            ModifiedOutputs = modifiedOutputs;
            Cost = cost ?? System.Array.Empty<ResourceQuantity>();
            SicknessPerDay = sicknessPerDay;
            MoralePerDay = moralePerDay;
            UnrestPerDay = unrestPerDay;
            FoodConsumptionModifier = foodConsumptionModifier;
            FuelConsumptionModifier = fuelConsumptionModifier;
        }

        // ── Registry ──────────────────────────────────────────────────

        static Dictionary<(BuildingType, SpecializationId), SpecializationDefinition> _specs;

        public static SpecializationDefinition Get(BuildingType type, SpecializationId id)
        {
            if (_specs == null) InitializeSpecs();
            return _specs.TryGetValue((type, id), out var spec) ? spec : null;
        }

        static void InitializeSpecs()
        {
            _specs = new Dictionary<(BuildingType, SpecializationId), SpecializationDefinition>();

            // Farm
            Reg(new(BuildingType.Farm, SpecializationId.OptionA, "Grain Silos",
                "Output 4 food/worker (up from 3)",
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Food, 4) }));
            Reg(new(BuildingType.Farm, SpecializationId.OptionB, "Medicinal Herbs",
                "Output 2 food + 0.5 medicine/worker",
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Food, 2), new ResourceQuantity(ResourceType.Medicine, 0.5) }));

            // Herb Garden
            Reg(new(BuildingType.HerbGarden, SpecializationId.OptionA, "Apothecary Lab",
                "Output 1.5 medicine/worker, requires 1 fuel input",
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Medicine, 1.5) }));
            Reg(new(BuildingType.HerbGarden, SpecializationId.OptionB, "Healer's Refuge",
                "No output bonus, -3 sickness/day passive",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                sicknessPerDay: -3));

            // Well
            Reg(new(BuildingType.Well, SpecializationId.OptionA, "Deep Boring",
                "Output 4 water/worker, fuel input 2",
                new[] { new ResourceQuantity(ResourceType.Fuel, 2) },
                new[] { new ResourceQuantity(ResourceType.Water, 4) }));
            Reg(new(BuildingType.Well, SpecializationId.OptionB, "Purification Basin",
                "No output bonus, -2 sickness/day passive",
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Water, 3) },
                sicknessPerDay: -2));

            // Fuel Store
            Reg(new(BuildingType.FuelStore, SpecializationId.OptionA, "Coal Pits",
                "Output 3 fuel/worker, +1 sickness/day",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Fuel, 3) },
                sicknessPerDay: 1));
            Reg(new(BuildingType.FuelStore, SpecializationId.OptionB, "Rationed Distribution",
                "Output 1.5 fuel/worker, -15% fuel consumption globally",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Fuel, 1.5) },
                fuelConsumptionModifier: -0.15));

            // Field Kitchen
            Reg(new(BuildingType.FieldKitchen, SpecializationId.OptionA, "Soup Line",
                "Output 3 food/worker, -3 morale/day",
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Food, 3) },
                moralePerDay: -3));
            Reg(new(BuildingType.FieldKitchen, SpecializationId.OptionB, "Fortified Kitchen",
                "Survives zone loss (rebuilt in next inner zone), +5 morale on spec",
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Food, 2) }));

            // Workshop
            Reg(new(BuildingType.Workshop, SpecializationId.OptionA, "Arms Foundry",
                "Output 3 materials/worker, requires 1 fuel input",
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                new[] { new ResourceQuantity(ResourceType.Materials, 3) }));
            Reg(new(BuildingType.Workshop, SpecializationId.OptionB, "Salvage Yard",
                "No output bonus, 10% daily chance of +5 random resource",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Materials, 2) }));

            // Smithy
            Reg(new(BuildingType.Smithy, SpecializationId.OptionA, "War Smith",
                "Output 2 integrity/worker, input 3 materials",
                new[] { new ResourceQuantity(ResourceType.Materials, 3) },
                new[] { new ResourceQuantity(ResourceType.Integrity, 2) }));
            Reg(new(BuildingType.Smithy, SpecializationId.OptionB, "Armor Works",
                "No integrity change, +2 guards on spec, Fortification +1",
                new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                new[] { new ResourceQuantity(ResourceType.Integrity, 1) }));

            // Cistern
            Reg(new(BuildingType.Cistern, SpecializationId.OptionA, "Rain Collection",
                "Output 1.5 water/worker, doubles on Heavy Rains",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Water, 1.5) }));
            Reg(new(BuildingType.Cistern, SpecializationId.OptionB, "Emergency Reserve",
                "+20 water on spec, auto-releases 10 water if water hits 0 (once)",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Water, 1) }));

            // Clinic
            Reg(new(BuildingType.Clinic, SpecializationId.OptionA, "Hospital",
                "+50% recovery slots",
                new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                new[] { new ResourceQuantity(ResourceType.Care, 1.5) }));
            Reg(new(BuildingType.Clinic, SpecializationId.OptionB, "Quarantine Ward",
                "-5 sickness/day",
                new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                new[] { new ResourceQuantity(ResourceType.Care, 1) },
                sicknessPerDay: -5));

            // Storehouse
            Reg(new(BuildingType.Storehouse, SpecializationId.OptionA, "Weapon Cache",
                "No fuel output, instead -5 unrest/day, Tyranny +1",
                System.Array.Empty<ResourceQuantity>(),
                System.Array.Empty<ResourceQuantity>(),
                unrestPerDay: -5));
            Reg(new(BuildingType.Storehouse, SpecializationId.OptionB, "Emergency Supplies",
                "No change, 50% resource salvage on zone loss",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Fuel, 1) }));

            // Root Cellar
            Reg(new(BuildingType.RootCellar, SpecializationId.OptionA, "Preserved Stores",
                "Output 1.5 food/worker, -10% food consumption globally",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Food, 1.5) },
                foodConsumptionModifier: -0.10));
            Reg(new(BuildingType.RootCellar, SpecializationId.OptionB, "Mushroom Farm",
                "Output 2 food/worker, +1 sickness/day",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Food, 2) },
                sicknessPerDay: 1));

            // Repair Yard
            Reg(new(BuildingType.RepairYard, SpecializationId.OptionA, "Siege Workshop",
                "Output 2 integrity/worker, input 4 materials",
                new[] { new ResourceQuantity(ResourceType.Materials, 4) },
                new[] { new ResourceQuantity(ResourceType.Integrity, 2) }));
            Reg(new(BuildingType.RepairYard, SpecializationId.OptionB, "Engineer Corps",
                "No change, fortification upgrades cost 50% less materials",
                new[] { new ResourceQuantity(ResourceType.Materials, 3) },
                new[] { new ResourceQuantity(ResourceType.Integrity, 1) }));

            // Rationing Post
            Reg(new(BuildingType.RationingPost, SpecializationId.OptionA, "Distribution Hub",
                "Output 1.5 water/worker, -5% food consumption globally",
                System.Array.Empty<ResourceQuantity>(),
                new[] { new ResourceQuantity(ResourceType.Water, 1.5) },
                foodConsumptionModifier: -0.05));
            Reg(new(BuildingType.RationingPost, SpecializationId.OptionB, "Propaganda Post",
                "No water output, +3 morale/day, -2 unrest/day, Faith +1",
                System.Array.Empty<ResourceQuantity>(),
                System.Array.Empty<ResourceQuantity>(),
                moralePerDay: 3, unrestPerDay: -2));
        }

        static void Reg(SpecializationDefinition def) => _specs[(def.BuildingType, def.Id)] = def;
    }
}
