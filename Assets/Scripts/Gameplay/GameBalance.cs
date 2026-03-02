using System.Collections.Generic;

namespace Siege.Gameplay
{
    public class GameBalance
    {
        public int DayDurationSeconds { get; } = 120;
        public int TargetSurvivalDay { get; } = 40;

        public int StartingHealthyWorkers { get; } = 60;
        public int StartingGuards { get; } = 15;
        public int StartingSickWorkers { get; } = 12;
        public int StartingElderly { get; } = 15;

        public int StartingFood { get; } = 170;
        public int StartingWater { get; } = 180;
        public int StartingFuel { get; } = 170;
        public int StartingMedicine { get; } = 35;
        public int StartingMaterials { get; } = 110;

        public int StartingMorale { get; } = 58;
        public int StartingUnrest { get; } = 22;
        public int StartingSickness { get; } = 18;
        public int StartingSiegeIntensity { get; } = 1;

        public int MaxSiegeIntensity { get; } = 7;

        public int RevoltThreshold { get; } = 88;
        public int FoodWaterLossThresholdDays { get; } = 3;

        public int ResourceBuildingCapacity { get; } = 100;

        public int OvercrowdingThreshold { get; } = 5;
        public int OvercrowdingUnrestPerStack { get; } = 2;
        public int OvercrowdingSicknessPerStack { get; } = 2;
        public double OvercrowdingConsumptionPerStack { get; } = 0.04;

        public int EvacIntegrityThreshold { get; } = 45;
        public int EvacSiegeThreshold { get; } = 3;

        public int LawCooldownDays { get; } = 2;

        public int MissionCooldownDays { get; } = 3;

        public double FoodPerPersonPerDay { get; } = 0.42;
        public double WaterPerPersonPerDay { get; } = 0.36;
        public double FuelPerPersonPerDay { get; } = 0.12;

        public double PerimeterScalingBase { get; } = 4.5;

        public int SiegeEscalationIntervalDays { get; } = 3;

        public int RecoveryThresholdSickness { get; } = 65;
        public int BaseRecoveryTimeDays { get; } = 4;
        public int RecoveryPerClinicSlot { get; } = 2;
        public int MedicinePerRecovery { get; } = 1;

        // Building Upgrades
        public static bool EnableBuildingUpgrades => true;
        public int BuildingMaxUpgradeLevel { get; } = 3;
        public int BuildingUpgradeMaterialsCost { get; } = 35;
        public double BuildingUpgradeBonusPerLevel { get; } = 0.35;
        public int BuildingUpgradeDelayDays { get; } = 1;

        // Kitchen Recipes
        public static bool EnableKitchenRecipes => true;
        public double GruelFoodPerWorker { get; } = 1.3;
        public int GruelSicknessPerDay { get; } = 2;
        public double FeastFuelPerWorker { get; } = 2.5;
        public double FeastFoodPerWorker { get; } = 2.8;
        public int FeastMoralePerDay { get; } = 2;

        // Sortie Mission
        public static bool EnableSortieMission => false;
        public int SortieGuardCost { get; } = 5;
        public int SortieSuccessChance { get; } = 25;
        public int SortiePartialChance { get; } = 40;
        public int SortieSuccessSiegeReduction { get; } = 1;
        public int SortieSuccessEscalationDelay { get; } = 2;
        public double SortiePartialDamageMultiplier { get; } = 0.85;
        public int SortiePartialDurationDays { get; } = 2;
        public int SortieFailGuardDeaths { get; } = 4;
        public int SortieFailUnrest { get; } = 12;

        // Defenses
        public static bool EnableDefenses => true;
        public int BarricadeMaterialsCost { get; } = 15;
        public int BarricadeBufferAmount { get; } = 12;
        public int OilCauldronFuelCost { get; } = 10;
        public int OilCauldronMaterialsCost { get; } = 10;
        public int ArcherPostMaterialsCost { get; } = 20;
        public int ArcherPostGuardsRequired { get; } = 2;
        public double ArcherPostDamageReduction { get; } = 0.12;

        // Scouting Mission
        public static bool EnableScoutingMission => true;
        public int ScoutingSuccessChance { get; } = 50;
        public int ScoutingFailDeaths { get; } = 2;
        public int ScoutingFailUnrest { get; } = 6;

        // Spy Intel Event
        public static bool EnableSpyIntelEvent => false;
        public int SpyIntelMinDay { get; } = 8;
        public int SpyIntelTriggerChance { get; } = 12;
        public int SpyIntelMaterialsCost { get; } = 15;
        public int SpyIntelFoodCost { get; } = 10;
        public int IntelBuffDurationDays { get; } = 4;
        public double IntelMissionSuccessBonus { get; } = 0.08;
        public int IntelInterceptGuardCost { get; } = 3;
        public int IntelInterceptGuardDeathRisk { get; } = 3;
        public double IntelInterceptSiegeDamageReduction { get; } = 0.75;
        public int IntelInterceptDurationDays { get; } = 2;
        public int IntelBraceIntegrityBonus { get; } = 8;

        // Black Market Event
        public static bool EnableBlackMarketEvent => true;
        public int BlackMarketMinDay { get; } = 5;
        public int BlackMarketRecurrenceMin { get; } = 6;
        public int BlackMarketRecurrenceMax { get; } = 8;
        public int BlackMarketHaggleUnrest { get; } = 6;

        // Clinic Specialization
        public static bool EnableClinicSpecialization => true;
        public double HospitalRecoveryBonus { get; } = 0.35;
        public int QuarantineWardSicknessReduction { get; } = 4;
        public int ClinicSpecializationMaterialsCost { get; } = 30;

        // Flag System
        public static bool EnableFlagSystem => true;

        // Fortifications
        public static bool EnableFortifications => true;
        public int FortificationMaxLevel { get; } = 3;
        public int FortificationMaterialsCost { get; } = 25;
        public int FortificationDamageReductionPerLevel { get; } = 1;

        // Storage system
        public int StorageBaseCapacity { get; } = 90;
        public int StorageCapacityPerUpgrade { get; } = 35;
        public int StorageMaxUpgradeLevel { get; } = 3;
        public int StorageUpgradeMaterialsCost { get; } = 30;
        public static bool WasteExcessResources => true;
        public double EvacuationResourceSalvagePercent { get; } = 0.25;

        // Trading Post
        public static bool EnableTradingPost => true;
        public int TradingPostBuildCost { get; } = 45;
        public double TradingPostBaseRate { get; } = 2.0;
        public double TradingPostHighSiegeRate { get; } = 3.2;
        public double TradingPostFluctuationRange { get; } = 0.25;
        public int TradingPostInterceptionBase { get; } = 8;
        public double TradingPostTyrannyRate { get; } = 1.4;
        public int TradingPostTyrannyUnrestInterval { get; } = 3;
        public int TradingPostTyrannyUnrest { get; } = 5;
        public int TradingPostFaithBonusChance { get; } = 12;
        public int TradingPostFaithBonusAmount { get; } = 2;

        // Wounded System
        public static bool EnableWoundedSystem => true;
        public int WoundedBaseRecoveryDays { get; } = 2;
        public int WoundedDeathDays { get; } = 3;
        public int MedicinePerWoundedRecovery { get; } = 1;
        public double WoundedFromDeathsSplit { get; } = 0.6;

        // Good Day Morale Boost
        public static bool EnableGoodDayMoraleBoost => true;
        public int StreakNoDeficitDaysRequired { get; } = 3;
        public int StreakNoDeficitMoraleBoost { get; } = 4;
        public int StreakLowSicknessDaysRequired { get; } = 5;
        public int StreakLowSicknessThreshold { get; } = 20;
        public int StreakLowSicknessMoraleBoost { get; } = 3;
        public int StreakLowSicknessUnrestReduction { get; } = 2;
        public int StreakZoneHeldDaysRequired { get; } = 3;
        public int StreakZoneHeldMoraleBoost { get; } = 5;
        public int StreakMissionSuccessRequired { get; } = 2;
        public int StreakMissionSuccessWorkerBonus { get; } = 2;

        // Relief Army / Hope Timer
        public static bool EnableReliefArmy => true;
        public int ReliefArmyBaseDay { get; } = 40;
        public int ReliefArmyVariance { get; } = 5;
        public int ReliefStartEstimateMin { get; } = 35;
        public int ReliefStartEstimateMax { get; } = 50;
        public int IntelNarrowPerLevel { get; } = 3;
        public int MaxIntelLevel { get; } = 3;
        public int MaxReliefAcceleration { get; } = 5;
        public int SignalFireFuelCost { get; } = 15;
        public int SignalFireMaterialsCost { get; } = 10;
        public int CorrespondenceAccelChance { get; } = 5;
        public int CorrespondenceMaxAccel { get; } = 2;

        // Humanity Score
        public static bool EnableHumanityScore => true;
        public int HumanityLowThreshold { get; } = 20;
        public int HumanityBleakThreshold { get; } = 35;
        public int HumanityHighThreshold { get; } = 65;
        public int HumanityHeroicThreshold { get; } = 80;
        public int HumanityMoraleBoostChance { get; } = 30;
        public int HumanityMoraleBoostAmount { get; } = 2;
        public int HumanityUnrestChance { get; } = 5;

        // Night Scavenging Phase
        public static bool EnableNightPhase => true;
        public int NightPhaseMinWorkers { get; } = 2;
        public int NightPhaseMaxWorkers { get; } = 4;
        public int ScavengingLocationRefreshDays { get; } = 3;
        public double FatiguedWorkerProductionPenalty { get; } = 0.10;
        public int NightPhaseDangerLowCasualty { get; } = 5;
        public int NightPhaseDangerMediumCasualty { get; } = 15;
        public int NightPhaseDangerHighCasualty { get; } = 30;

        // Named Characters
        public static bool EnableNamedCharacters => true;
        public double NamedCharacterDeathChancePerDeath { get; } = 0.04;
        public double NamedCharacterDesertionChance { get; } = 0.03;

        // Diplomacy & Negotiation
        public static bool EnableDiplomacy => true;
        public int BribeFoodCost { get; } = 10;
        public int BribeMaterialsCost { get; } = 7;
        public int BribeFoodCostTyranny { get; } = 7;
        public int BribeMaterialsCostTyranny { get; } = 5;
        public double BribeSiegeDamageMultiplier { get; } = 0.85;
        public int BribeInterceptionChance { get; } = 10;
        public int BribeInterceptionUnrest { get; } = 12;

        public int HostageFoodCost { get; } = 4;
        public int HostageMedicineCost { get; } = 2;
        public int HostageDailyMorale { get; } = -3;

        public int TributeFoodCost { get; } = 12;
        public int TributeWaterCost { get; } = 12;
        public int TributeDailyMorale { get; } = -6;

        public int CorrespondenceMaterialsCost { get; } = 4;
        public int CorrespondenceDailyMorale { get; } = 1;
        public int CorrespondenceIntelChance { get; } = 8;
        public int CorrespondenceIntelResourceAmount { get; } = 4;

        public int BetrayalFood { get; } = 30;
        public int BetrayalWater { get; } = 30;
        public int BetrayalMaterials { get; } = 20;
        public int BetrayalUnrest { get; } = 18;
        public int BetrayalMorale { get; } = -18;
        public int BetrayalRetaliationChance { get; } = 15;

        // Building Specializations
        public static bool EnableBuildingSpecializations => true;
        public int BuildingSpecializationMaterialsCost { get; } = 25;

        // Farm specs
        public double GrainSilosFoodPerWorker { get; } = 3.5;
        public double MedicinalHerbsFoodPerWorker { get; } = 2;
        public double MedicinalHerbsMedicinePerWorker { get; } = 0.5;

        // HerbGarden specs
        public double ApothecaryLabMedicinePerWorker { get; } = 1.5;
        public int ApothecaryLabFuelInput { get; } = 1;
        public int HealersRefugeSicknessReduction { get; } = 3;

        // Well specs
        public double DeepBoringWaterPerWorker { get; } = 3.5;
        public int DeepBoringFuelInput { get; } = 2;
        public int PurificationBasinSicknessReduction { get; } = 2;

        // FuelStore specs
        public double CoalPitsFuelPerWorker { get; } = 2.5;
        public int CoalPitsDailySickness { get; } = 1;
        public double RationedDistributionFuelPerWorker { get; } = 1.5;
        public double RationedDistributionFuelConsumptionMultiplier { get; } = 0.85;

        // FieldKitchen specs
        public double SoupLineFoodPerWorker { get; } = 2.5;
        public int SoupLineDailyMorale { get; } = -3;

        // Workshop specs
        public double ArmsFoundryMaterialsPerWorker { get; } = 2.5;
        public int ArmsFoundryFuelInput { get; } = 1;
        public int SalvageYardChance { get; } = 10;
        public int SalvageYardAmount { get; } = 5;

        // Smithy specs
        public double WarSmithIntegrityPerWorker { get; } = 1.5;
        public int WarSmithMaterialsInput { get; } = 3;
        public double SmithyDefaultIntegrityPerWorker { get; } = 1;

        // Cistern specs
        public double RainCollectionWaterPerWorker { get; } = 1.5;
        public double RainCollectionHeavyRainsMultiplier { get; } = 2.0;

        // Storehouse specs
        public int WeaponCacheUnrestReduction { get; } = 5;
        public double EmergencySuppliesSalvagePercent { get; } = 0.50;

        // RootCellar specs
        public double PreservedStoresFoodPerWorker { get; } = 1.5;
        public double PreservedStoresFoodConsumptionMultiplier { get; } = 0.90;
        public double MushroomFarmFoodPerWorker { get; } = 2;
        public int MushroomFarmDailySickness { get; } = 1;

        // RepairYard specs
        public double SiegeWorkshopIntegrityPerWorker { get; } = 1.5;
        public int SiegeWorkshopMaterialsInput { get; } = 4;
        public double EngineerCorpsFortificationCostMultiplier { get; } = 0.50;

        // RationingPost specs
        public double DistributionHubWaterPerWorker { get; } = 1.3;
        public double DistributionHubFoodConsumptionMultiplier { get; } = 0.95;
        public int PropagandaPostDailyMorale { get; } = 2;
        public int PropagandaPostDailyUnrest { get; } = -1;

        // Defensive Posture System
        public static bool EnableDefensivePosture => true;
        public static bool EnableDefensivePostureGuardOverride => true;
        public int DefensivePostureGuardMinimum { get; } = 4;
        public double HunkerDownSiegeReduction { get; } = 0.20;
        public double ActiveDefenseSiegeReduction { get; } = 0.30;
        public int AggressivePatrolsUnrest { get; } = 2;
        public int AggressivePatrolsInterceptChance { get; } = 12;
        public int AggressivePatrolsResourceMin { get; } = 1;
        public int AggressivePatrolsResourceMax { get; } = 5;
        public int OpenGatesMorale { get; } = 3;
        public int OpenGatesRefugeeChance { get; } = 15;
        public int OpenGatesRefugeeMin { get; } = 2;
        public int OpenGatesRefugeeMax { get; } = 5;
        public int OpenGatesInfiltratorChance { get; } = 15;
        public int OpenGatesInfiltratorUnrest { get; } = 10;
        public int OpenGatesInfiltratorSickness { get; } = 4;
        public int ScorchedPerimeterIntegrityDamage { get; } = 12;
        public double ScorchedPerimeterSiegeReduction { get; } = 0.35;
        public int ScorchedPerimeterDuration { get; } = 2;
        public int ScorchedPerimeterMorale { get; } = -12;
        public int ScorchedPerimeterTyranny { get; } = 1;

        // Morale Emergency Orders
        public static bool EnableMoraleOrders => true;

        public int HoldAFeastFoodCost { get; } = 20;
        public int HoldAFeastFuelCost { get; } = 8;
        public int HoldAFeastMoraleGain { get; } = 15;
        public int HoldAFeastUnrest { get; } = -6;
        public int HoldAFeastFoodGate { get; } = 45;
        public int HoldAFeastCooldown { get; } = 6;

        public int DayOfRemembranceMoraleGain { get; } = 18;
        public int DayOfRemembranceUnrest { get; } = -5;
        public int DayOfRemembranceSickness { get; } = -3;
        public int DayOfRemembranceFaithGain { get; } = 2;
        public int DayOfRemembranceMoraleGate { get; } = 30;
        public int DayOfRemembranceCooldown { get; } = 10;

        public int PublicTrialDeaths { get; } = 2;
        public int PublicTrialTyrannyUnrest { get; } = -18;
        public int PublicTrialTyrannyMorale { get; } = -12;
        public int PublicTrialFaithMorale { get; } = 8;
        public int PublicTrialFaithUnrest { get; } = -7;
        public int PublicTrialCooldown { get; } = 5;

        public int StorytellingNightMoraleGain { get; } = 6;
        public int StorytellingNightMoraleMin { get; } = 20;
        public int StorytellingNightMoraleMax { get; } = 60;
        public int StorytellingNightCooldown { get; } = 4;

        public int DistributeLuxuriesFuelCost { get; } = 12;
        public int DistributeLuxuriesMaterialsCost { get; } = 12;
        public int DistributeLuxuriesMoraleGain { get; } = 10;
        public int DistributeLuxuriesUnrest { get; } = -3;
        public int DistributeLuxuriesSickness { get; } = -2;
        public int DistributeLuxuriesMaterialsGate { get; } = 25;
        public int DistributeLuxuriesFuelGate { get; } = 18;
        public int DistributeLuxuriesCooldown { get; } = 6;

        // Cannibalism Law
        public static bool EnableCannibalismLaw => false;
        public int CannibalismFoodThreshold { get; } = 5;
        public int CannibalismTyrannyGain { get; } = 3;
        public int CannibalismFearGain { get; } = 2;
        public int CannibalismOnEnactUnrest { get; } = 20;
        public int CannibalismOnEnactDesertions { get; } = 5;
        public int CannibalismFoodPerDeath { get; } = 3;
        public int CannibalismMaxFoodPerDay { get; } = 10;
        public int CannibalismDailyMorale { get; } = -5;
        public int CannibalismDailySickness { get; } = 3;
        public int CannibalismDailyUnrest { get; } = -3;
        public int CannibalismGuardDesertionChance { get; } = 15;
        public int CannibalismWorkerDesertionChance { get; } = 10;

        // Production penalty thresholds - penalties only apply after these values
        public static bool EnableProductionMultipliers => true;
        public int MoraleProductionThreshold { get; } = 70;
        public int UnrestProductionThreshold { get; } = 70;
        public int SicknessProductionThreshold { get; } = 70;

        public BuildingDefinition Farm { get; } = new()
        {
            Id = BuildingId.Farm,
            Zone = ZoneId.OuterFarms,
            MaxWorkers = 10,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "crop_cultivation",
                    Input = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                    Output = new[] { new ResourceQuantity(ResourceType.Food, 3) },
                },
            },
        };

        public BuildingDefinition HerbGarden { get; } = new()
        {
            Id = BuildingId.HerbGarden,
            Zone = ZoneId.OuterFarms,
            MaxWorkers = 10,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "herbal_medicine",
                    Output = new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                },
            },
        };

        public BuildingDefinition Well { get; } = new()
        {
            Id = BuildingId.Well,
            Zone = ZoneId.OuterResidential,
            MaxWorkers = 10,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "water_extraction",
                    Input = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                    Output = new[] { new ResourceQuantity(ResourceType.Water, 3) },
                },
            },
        };

        public BuildingDefinition FuelStore { get; } = new()
        {
            Id = BuildingId.FuelStore,
            Zone = ZoneId.OuterResidential,
            MaxWorkers = 8,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "fuel_refinement",
                    Output = new[] { new ResourceQuantity(ResourceType.Fuel, 2) },
                },
            },
        };

        public BuildingDefinition FieldKitchen { get; } = new()
        {
            Id = BuildingId.FieldKitchen,
            Zone = ZoneId.ArtisanQuarter,
            MaxWorkers = 6,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "field_meal_preparation",
                    Input = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                    Output = new[] { new ResourceQuantity(ResourceType.Food, 2) },
                },
            },
        };

        public BuildingDefinition Workshop { get; } = new()
        {
            Id = BuildingId.Workshop,
            Zone = ZoneId.ArtisanQuarter,
            MaxWorkers = 6,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "crafting",
                    Input = new[] { new ResourceQuantity(ResourceType.Fuel, 1) },
                    Output = new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                },
            },
        };

        public BuildingDefinition Smithy { get; } = new()
        {
            Id = BuildingId.Smithy,
            Zone = ZoneId.InnerDistrict,
            MaxWorkers = 5,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "metalwork",
                    Input = new[] { new ResourceQuantity(ResourceType.Materials, 3) },
                    Output = new[] { new ResourceQuantity(ResourceType.Integrity, 1) },
                },
            },
        };

        public BuildingDefinition Cistern { get; } = new()
        {
            Id = BuildingId.Cistern,
            Zone = ZoneId.InnerDistrict,
            MaxWorkers = 5,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "water_collection",
                    Output = new[] { new ResourceQuantity(ResourceType.Water, 1.5) },
                },
            },
        };

        public BuildingDefinition Clinic { get; } = new()
        {
            Id = BuildingId.Clinic,
            Zone = ZoneId.Keep,
            MaxWorkers = 4,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "medical_treatment",
                    Input = new[] { new ResourceQuantity(ResourceType.Medicine, 1) },
                    Output = new[] { new ResourceQuantity(ResourceType.Care, 1) },
                },
            },
        };

        public BuildingDefinition Storehouse { get; } = new()
        {
            Id = BuildingId.Storehouse,
            Zone = ZoneId.Keep,
            MaxWorkers = 4,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "structural_repairs",
                    Input = new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                    Output = new[] { new ResourceQuantity(ResourceType.Integrity, 1) },
                },
            },
        };

        public BuildingDefinition RootCellar { get; } = new()
        {
            Id = BuildingId.RootCellar,
            Zone = ZoneId.Keep,
            MaxWorkers = 4,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "food_preservation",
                    Input = new[] { new ResourceQuantity(ResourceType.Food, 1) },
                    Output = new[] { new ResourceQuantity(ResourceType.Food, 1.5) },
                },
            },
        };

        public BuildingDefinition RepairYard { get; } = new()
        {
            Id = BuildingId.RepairYard,
            Zone = ZoneId.Keep,
            MaxWorkers = 4,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "siege_repairs",
                    Input = new[] { new ResourceQuantity(ResourceType.Materials, 4) },
                    Output = new[] { new ResourceQuantity(ResourceType.Integrity, 1.5) },
                },
            },
        };

        public BuildingDefinition RationingPost { get; } = new()
        {
            Id = BuildingId.RationingPost,
            Zone = ZoneId.Keep,
            MaxWorkers = 4,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "resource_distribution",
                    Input = new[] { new ResourceQuantity(ResourceType.Food, 1) },
                    Output = new[] { new ResourceQuantity(ResourceType.Water, 1.3) },
                },
            },
        };

        public BuildingDefinition TradingPost { get; } = new()
        {
            Id = BuildingId.TradingPost,
            Zone = ZoneId.Keep,
            MaxWorkers = 4,
            IsBuilt = false,
            Recipes = new ProductionRecipe[]
            {
                new()
                {
                    Id = "trade_exchange",
                    Input = new[] { new ResourceQuantity(ResourceType.Materials, 2) },
                    Output = new[] { new ResourceQuantity(ResourceType.Food, 2) },
                },
            },
        };

        public List<BuildingDefinition> Buildings { get; }

        public GameBalance()
        {
            Buildings = new List<BuildingDefinition>
            {
                Farm,
                HerbGarden,
                Well,
                FuelStore,
                FieldKitchen,
                Workshop,
                Smithy,
                Cistern,
                Clinic,
                Storehouse,
                RootCellar,
                RepairYard,
                RationingPost,
                TradingPost,
            };
        }
    }
}
