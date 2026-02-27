namespace Siege.Gameplay
{
    public class GameBalance
    {
        public int TargetSurvivalDay = 40;

        public int StartingHealthyWorkers = 60;
        public int StartingGuards = 15;
        public int StartingSickWorkers = 12;
        public int StartingElderly = 15;

        public int StartingFood = 170;
        public int StartingWater = 180;
        public int StartingFuel = 170;
        public int StartingMedicine = 35;
        public int StartingMaterials = 110;

        public int StartingMorale = 58;
        public int StartingUnrest = 22;
        public int StartingSickness = 18;
        public int StartingSiegeIntensity = 1;

        public int MaxSiegeIntensity = 7;

        public int RevoltThreshold = 88;
        public int FoodWaterLossThresholdDays = 3;

        public int OvercrowdingThreshold = 5;
        public int OvercrowdingUnrestPerStack = 2;
        public int OvercrowdingSicknessPerStack = 2;
        public double OvercrowdingConsumptionPerStack = 0.04;

        public int EvacIntegrityThreshold = 45;
        public int EvacSiegeThreshold = 3;

        public int LawCooldownDays = 2;

        public int MissionCooldownDays = 3;

        public double FoodPerPersonPerDay = 0.42;
        public double WaterPerPersonPerDay = 0.36;
        public double FuelPerPersonPerDay = 0.12;

        public double PerimeterScalingBase = 4.5;

        public int SiegeEscalationIntervalDays = 3;

        public int RecoveryThresholdSickness = 65;
        public int BaseRecoveryTimeDays = 4;
        public int RecoveryPerClinicSlot = 2;
        public int MedicinePerRecovery = 1;

        // Building Upgrades
        public static bool EnableBuildingUpgrades => true;
        public int BuildingMaxUpgradeLevel = 3;
        public int BuildingUpgradeMaterialsCost = 35;
        public double BuildingUpgradeBonusPerLevel = 0.35;
        public int BuildingUpgradeDelayDays = 1;

        // Kitchen Recipes
        public static bool EnableKitchenRecipes => true;
        public double GruelFoodPerWorker = 1.3;
        public int GruelSicknessPerDay = 2;
        public double FeastFuelPerWorker = 2.5;
        public double FeastFoodPerWorker = 2.8;
        public int FeastMoralePerDay = 2;

        // Sortie Mission
        public static bool EnableSortieMission => false;
        public int SortieGuardCost = 5;
        public int SortieSuccessChance = 25;
        public int SortiePartialChance = 40;
        public int SortieSuccessSiegeReduction = 1;
        public int SortieSuccessEscalationDelay = 2;
        public double SortiePartialDamageMultiplier = 0.85;
        public int SortiePartialDurationDays = 2;
        public int SortieFailGuardDeaths = 4;
        public int SortieFailUnrest = 12;

        // Defenses
        public static bool EnableDefenses => true;
        public int BarricadeMaterialsCost = 15;
        public int BarricadeBufferAmount = 12;
        public int OilCauldronFuelCost = 10;
        public int OilCauldronMaterialsCost = 10;
        public int ArcherPostMaterialsCost = 20;
        public int ArcherPostGuardsRequired = 2;
        public double ArcherPostDamageReduction = 0.12;

        // Scouting Mission
        public static bool EnableScoutingMission => true;
        public int ScoutingSuccessChance = 50;
        public int ScoutingFailDeaths = 2;
        public int ScoutingFailUnrest = 6;

        // Spy Intel Event
        public static bool EnableSpyIntelEvent => false;
        public int SpyIntelMinDay = 8;
        public int SpyIntelTriggerChance = 12;
        public int SpyIntelMaterialsCost = 15;
        public int SpyIntelFoodCost = 10;
        public int IntelBuffDurationDays = 4;
        public double IntelMissionSuccessBonus = 0.08;
        public int IntelInterceptGuardCost = 3;
        public int IntelInterceptGuardDeathRisk = 3;
        public double IntelInterceptSiegeDamageReduction = 0.75;
        public int IntelInterceptDurationDays = 2;
        public int IntelBraceIntegrityBonus = 8;

        // Black Market Event
        public static bool EnableBlackMarketEvent => true;
        public int BlackMarketMinDay = 5;
        public int BlackMarketRecurrenceMin = 6;
        public int BlackMarketRecurrenceMax = 8;
        public int BlackMarketHaggleUnrest = 6;

        // Clinic Specialization
        public static bool EnableClinicSpecialization => true;
        public double HospitalRecoveryBonus = 0.35;
        public int QuarantineWardSicknessReduction = 4;
        public int ClinicSpecializationMaterialsCost = 30;

        // Flag System
        public static bool EnableFlagSystem => true;

        // Fortifications
        public static bool EnableFortifications => true;
        public int FortificationMaxLevel = 3;
        public int FortificationMaterialsCost = 25;
        public int FortificationDamageReductionPerLevel = 1;

        // Storage system
        public int StorageBaseCapacity = 90;
        public int StorageCapacityPerUpgrade = 35;
        public int StorageMaxUpgradeLevel = 3;
        public int StorageUpgradeMaterialsCost = 30;
        public static bool WasteExcessResources => true;
        public double EvacuationResourceSalvagePercent = 0.25;

        // Trading Post
        public static bool EnableTradingPost => true;
        public int TradingPostBuildCost = 45;
        public double TradingPostBaseRate = 2.0;
        public double TradingPostHighSiegeRate = 3.2;
        public double TradingPostFluctuationRange = 0.25;
        public int TradingPostInterceptionBase = 8;
        public double TradingPostTyrannyRate = 1.4;
        public int TradingPostTyrannyUnrestInterval = 3;
        public int TradingPostTyrannyUnrest = 5;
        public int TradingPostFaithBonusChance = 12;
        public int TradingPostFaithBonusAmount = 2;

        // Wounded System
        public static bool EnableWoundedSystem => true;
        public int WoundedBaseRecoveryDays = 2;
        public int WoundedDeathDays = 3;
        public int MedicinePerWoundedRecovery = 1;
        public double WoundedFromDeathsSplit = 0.6;

        // Good Day Morale Boost
        public static bool EnableGoodDayMoraleBoost => true;
        public int StreakNoDeficitDaysRequired = 3;
        public int StreakNoDeficitMoraleBoost = 4;
        public int StreakLowSicknessDaysRequired = 5;
        public int StreakLowSicknessThreshold = 20;
        public int StreakLowSicknessMoraleBoost = 3;
        public int StreakLowSicknessUnrestReduction = 2;
        public int StreakZoneHeldDaysRequired = 3;
        public int StreakZoneHeldMoraleBoost = 5;
        public int StreakMissionSuccessRequired = 2;
        public int StreakMissionSuccessWorkerBonus = 2;

        // Relief Army / Hope Timer
        public static bool EnableReliefArmy => true;
        public int ReliefArmyBaseDay = 40;
        public int ReliefArmyVariance = 5;
        public int ReliefStartEstimateMin = 35;
        public int ReliefStartEstimateMax = 50;
        public int IntelNarrowPerLevel = 3;
        public int MaxIntelLevel = 3;
        public int MaxReliefAcceleration = 5;
        public int SignalFireFuelCost = 15;
        public int SignalFireMaterialsCost = 10;
        public int CorrespondenceAccelChance = 5;
        public int CorrespondenceMaxAccel = 2;

        // Humanity Score
        public static bool EnableHumanityScore => true;
        public int HumanityLowThreshold = 20;
        public int HumanityBleakThreshold = 35;
        public int HumanityHighThreshold = 65;
        public int HumanityHeroicThreshold = 80;
        public int HumanityMoraleBoostChance = 30;
        public int HumanityMoraleBoostAmount = 2;
        public int HumanityUnrestChance = 5;

        // Night Scavenging Phase
        public static bool EnableNightPhase => true;
        public int NightPhaseMinWorkers = 2;
        public int NightPhaseMaxWorkers = 4;
        public int ScavengingLocationRefreshDays = 3;
        public double FatiguedWorkerProductionPenalty = 0.10;
        public int NightPhaseDangerLowCasualty = 5;
        public int NightPhaseDangerMediumCasualty = 15;
        public int NightPhaseDangerHighCasualty = 30;

        // Named Characters
        public static bool EnableNamedCharacters => true;
        public double NamedCharacterDeathChancePerDeath = 0.04;
        public double NamedCharacterDesertionChance = 0.03;

        // Diplomacy & Negotiation
        public static bool EnableDiplomacy => true;
        public int BribeFoodCost = 10;
        public int BribeMaterialsCost = 7;
        public int BribeFoodCostTyranny = 7;
        public int BribeMaterialsCostTyranny = 5;
        public double BribeSiegeDamageMultiplier = 0.85;
        public int BribeInterceptionChance = 10;
        public int BribeInterceptionUnrest = 12;

        public int HostageFoodCost = 4;
        public int HostageMedicineCost = 2;
        public int HostageDailyMorale = -3;

        public int TributeFoodCost = 12;
        public int TributeWaterCost = 12;
        public int TributeDailyMorale = -6;

        public int CorrespondenceMaterialsCost = 4;
        public int CorrespondenceDailyMorale = 1;
        public int CorrespondenceIntelChance = 8;
        public int CorrespondenceIntelResourceAmount = 4;

        public int BetrayalFood = 30;
        public int BetrayalWater = 30;
        public int BetrayalMaterials = 20;
        public int BetrayalUnrest = 18;
        public int BetrayalMorale = -18;
        public int BetrayalRetaliationChance = 15;

        // Building Specializations
        public static bool EnableBuildingSpecializations => true;
        public int BuildingSpecializationMaterialsCost = 25;

        // Farm specs
        public double GrainSilosFoodPerWorker = 3.5;
        public double MedicinalHerbsFoodPerWorker = 2;
        public double MedicinalHerbsMedicinePerWorker = 0.5;

        // HerbGarden specs
        public double ApothecaryLabMedicinePerWorker = 1.5;
        public int ApothecaryLabFuelInput = 1;
        public int HealersRefugeSicknessReduction = 3;

        // Well specs
        public double DeepBoringWaterPerWorker = 3.5;
        public int DeepBoringFuelInput = 2;
        public int PurificationBasinSicknessReduction = 2;

        // FuelStore specs
        public double CoalPitsFuelPerWorker = 2.5;
        public int CoalPitsDailySickness = 1;
        public double RationedDistributionFuelPerWorker = 1.5;
        public double RationedDistributionFuelConsumptionMultiplier = 0.85;

        // FieldKitchen specs
        public double SoupLineFoodPerWorker = 2.5;
        public int SoupLineDailyMorale = -3;

        // Workshop specs
        public double ArmsFoundryMaterialsPerWorker = 2.5;
        public int ArmsFoundryFuelInput = 1;
        public int SalvageYardChance = 10;
        public int SalvageYardAmount = 5;

        // Smithy specs
        public double WarSmithIntegrityPerWorker = 1.5;
        public int WarSmithMaterialsInput = 3;
        public double SmithyDefaultIntegrityPerWorker = 1;

        // Cistern specs
        public double RainCollectionWaterPerWorker = 1.5;
        public double RainCollectionHeavyRainsMultiplier = 2.0;

        // Storehouse specs
        public int WeaponCacheUnrestReduction = 5;
        public double EmergencySuppliesSalvagePercent = 0.50;

        // RootCellar specs
        public double PreservedStoresFoodPerWorker = 1.5;
        public double PreservedStoresFoodConsumptionMultiplier = 0.90;
        public double MushroomFarmFoodPerWorker = 2;
        public int MushroomFarmDailySickness = 1;

        // RepairYard specs
        public double SiegeWorkshopIntegrityPerWorker = 1.5;
        public int SiegeWorkshopMaterialsInput = 4;
        public double EngineerCorpsFortificationCostMultiplier = 0.50;

        // RationingPost specs
        public double DistributionHubWaterPerWorker = 1.3;
        public double DistributionHubFoodConsumptionMultiplier = 0.95;
        public int PropagandaPostDailyMorale = 2;
        public int PropagandaPostDailyUnrest = -1;

        // Defensive Posture System
        public static bool EnableDefensivePosture => true;
        public static bool EnableDefensivePostureGuardOverride => true;
        public int DefensivePostureGuardMinimum = 4;
        public double HunkerDownSiegeReduction = 0.20;
        public double ActiveDefenseSiegeReduction = 0.30;
        public int AggressivePatrolsUnrest = 2;
        public int AggressivePatrolsInterceptChance = 12;
        public int AggressivePatrolsResourceMin = 1;
        public int AggressivePatrolsResourceMax = 5;
        public int OpenGatesMorale = 3;
        public int OpenGatesRefugeeChance = 15;
        public int OpenGatesRefugeeMin = 2;
        public int OpenGatesRefugeeMax = 5;
        public int OpenGatesInfiltratorChance = 15;
        public int OpenGatesInfiltratorUnrest = 10;
        public int OpenGatesInfiltratorSickness = 4;
        public int ScorchedPerimeterIntegrityDamage = 12;
        public double ScorchedPerimeterSiegeReduction = 0.35;
        public int ScorchedPerimeterDuration = 2;
        public int ScorchedPerimeterMorale = -12;
        public int ScorchedPerimeterTyranny = 1;

        // Morale Emergency Orders
        public static bool EnableMoraleOrders => true;

        public int HoldAFeastFoodCost = 20;
        public int HoldAFeastFuelCost = 8;
        public int HoldAFeastMoraleGain = 15;
        public int HoldAFeastUnrest = -6;
        public int HoldAFeastFoodGate = 45;
        public int HoldAFeastCooldown = 6;

        public int DayOfRemembranceMoraleGain = 18;
        public int DayOfRemembranceUnrest = -5;
        public int DayOfRemembranceSickness = -3;
        public int DayOfRemembranceFaithGain = 2;
        public int DayOfRemembranceMoraleGate = 30;
        public int DayOfRemembranceCooldown = 10;

        public int PublicTrialDeaths = 2;
        public int PublicTrialTyrannyUnrest = -18;
        public int PublicTrialTyrannyMorale = -12;
        public int PublicTrialFaithMorale = 8;
        public int PublicTrialFaithUnrest = -7;
        public int PublicTrialCooldown = 5;

        public int StorytellingNightMoraleGain = 6;
        public int StorytellingNightMoraleMin = 20;
        public int StorytellingNightMoraleMax = 60;
        public int StorytellingNightCooldown = 4;

        public int DistributeLuxuriesFuelCost = 12;
        public int DistributeLuxuriesMaterialsCost = 12;
        public int DistributeLuxuriesMoraleGain = 10;
        public int DistributeLuxuriesUnrest = -3;
        public int DistributeLuxuriesSickness = -2;
        public int DistributeLuxuriesMaterialsGate = 25;
        public int DistributeLuxuriesFuelGate = 18;
        public int DistributeLuxuriesCooldown = 6;

        // Cannibalism Law
        public static bool EnableCannibalismLaw => false;
        public int CannibalismFoodThreshold = 5;
        public int CannibalismTyrannyGain = 3;
        public int CannibalismFearGain = 2;
        public int CannibalismOnEnactUnrest = 20;
        public int CannibalismOnEnactDesertions = 5;
        public int CannibalismFoodPerDeath = 3;
        public int CannibalismMaxFoodPerDay = 10;
        public int CannibalismDailyMorale = -5;
        public int CannibalismDailySickness = 3;
        public int CannibalismDailyUnrest = -3;
        public int CannibalismGuardDesertionChance = 15;
        public int CannibalismWorkerDesertionChance = 10;

        // Production penalty thresholds - penalties only apply after these values
        public static bool EnableProductionMultipliers => true;
        public int MoraleProductionThreshold = 70;
        public int UnrestProductionThreshold = 70;
        public int SicknessProductionThreshold = 70;
    }
}