using System;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Simulation;
using Siege.Gameplay.Zones;

namespace Siege.Gameplay.Siege
{
    /// <summary>
    /// Applies daily siege damage to the active perimeter zone.
    /// Damage = (PerimeterScaling + SiegeIntensity) × PerimeterFactor - modifiers.
    /// Handles zone loss when integrity reaches 0.
    /// </summary>
    public class SiegeSystem : ISimulationSystem
    {
        const double PerimeterScaling = 3.0;
        const double GuardDamageReductionPerGuard = 0.02; // 2% per guard
        const int IntensityEscalationInterval = 5; // days between intensity increases
        const int FirstEscalationDay = 5;

        readonly ZoneManager _zoneManager;
        readonly ChangeLog _changeLog;
        readonly LawManager _lawManager;

        bool _appliedToday;

        public SiegeSystem(ZoneManager zoneManager, ChangeLog changeLog, LawManager lawManager)
        {
            _zoneManager = zoneManager;
            _changeLog = changeLog;
            _lawManager = lawManager;
        }

        public void OnDayStart(GameState state, int day)
        {
            _appliedToday = false;

            // Escalate siege intensity over time
            if (day >= FirstEscalationDay && (day - FirstEscalationDay) % IntensityEscalationInterval == 0)
            {
                if (state.SiegeIntensity < GameState.MaxSiegeIntensity)
                {
                    state.SiegeIntensity++;
                    _changeLog.Record("SiegeIntensity", 1, "Siege escalation");
                }
            }
        }

        public void Tick(GameState state, float deltaTime)
        {
            if (_appliedToday) return;

            // Apply siege damage once per day (at start of day)
            float dayFraction = deltaTime / GameClock.DayLengthSeconds;
            if (dayFraction <= 0) return;

            _appliedToday = true;
            ApplyDailyDamage(state);
        }

        void ApplyDailyDamage(GameState state)
        {
            var perimeter = state.ActivePerimeter;
            var zoneState = state.Zones[perimeter];

            if (zoneState.IsLost) return;

            double baseDamage = (PerimeterScaling + state.SiegeIntensity) * ZoneDefaults.PerimeterFactor(perimeter);

            // Oil cauldron negation
            if (zoneState.HasOilCauldron)
            {
                zoneState.HasOilCauldron = false;
                _changeLog.Record("Integrity", 0, "Oil cauldron negated siege damage");
                return;
            }

            // Apply modifiers
            double damage = baseDamage * _lawManager.CombinedSiegeDamageMultiplier;

            // Temporal reduction from missions
            if (state.SiegeDamageReductionDays > 0)
                damage *= state.SiegeDamageReductionMultiplier;

            // Guard reduction
            double guardReduction = state.Guards * GuardDamageReductionPerGuard;
            damage *= (1.0 - Math.Min(guardReduction, 0.5)); // cap at 50% reduction

            // Archer post reduction
            if (zoneState.HasArcherPost && zoneState.ArcherPostGuards >= 2)
            {
                damage *= 0.88; // 12% reduction
            }

            // Final assault multiplier
            if (state.FinalAssaultActive)
            {
                damage *= 2.0;
            }

            // Apply to barricade buffer first
            if (zoneState.BarricadeBuffer > 0)
            {
                double absorbed = Math.Min(zoneState.BarricadeBuffer, damage);
                zoneState.BarricadeBuffer -= absorbed;
                damage -= absorbed;
                _changeLog.Record("Barricade", -absorbed, "Absorbed siege damage");
            }

            // Apply remaining damage to integrity
            if (damage > 0)
            {
                zoneState.Integrity -= damage;
                _changeLog.Record("Integrity", -damage, $"Siege damage ({perimeter})");

                // Check for zone loss
                if (zoneState.Integrity <= 0)
                {
                    zoneState.Integrity = 0;
                    _zoneManager.LoseZone(perimeter, isEvacuation: false);
                }
            }
        }
    }
}
