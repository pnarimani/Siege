using System;

namespace Siege.Gameplay.Simulation
{
    public static class StateChangeFormatter
    {
        public static string Format(StateChange change)
        {
            int amount = (int)Math.Abs(change.Amount);
            bool positive = change.Amount > 0;
            string s = amount == 1 ? "" : "s";

            return change.Field switch
            {
                "HealthyWorkers" => positive
                    ? $"+{amount} worker{s}"
                    : $"{amount} worker{s} died",

                "SickWorkers" => positive
                    ? $"+{amount} worker{s} sickened"
                    : $"{amount} sick worker{s} recovered",

                "Guards" => positive
                    ? $"+{amount} guard{s}"
                    : $"{amount} guard{s} lost",

                "WoundedGuards" => positive
                    ? $"+{amount} guard{s} wounded"
                    : $"{amount} wounded guard{s} recovered",

                "Elderly" => positive
                    ? $"+{amount} elderly"
                    : $"−{amount} elderly",

                "Deaths" => $"{amount} died",

                "Morale" => positive ? $"Morale +{amount}" : $"Morale −{amount}",
                "Unrest" => positive ? $"Unrest +{amount}" : $"Unrest −{amount}",
                "Sickness" => positive ? $"Sickness +{amount}" : $"Sickness −{amount}",

                "Food" => positive ? $"+{amount} food" : $"−{amount} food",
                "Water" => positive ? $"+{amount} water" : $"−{amount} water",
                "Fuel" => positive ? $"+{amount} fuel" : $"−{amount} fuel",
                "Materials" => positive ? $"+{amount} materials" : $"−{amount} materials",
                "Medicine" => positive ? $"+{amount} medicine" : $"−{amount} medicine",

                "Integrity" => positive
                    ? $"+{amount} zone integrity"
                    : $"−{amount} zone integrity",

                "SiegeIntensity" => positive
                    ? $"Siege intensity +{amount}"
                    : $"Siege intensity −{amount}",

                _ => positive
                    ? $"{change.Field} +{amount}"
                    : $"{change.Field} −{amount}",
            };
        }
    }
}
