using System;

namespace Siege.Gameplay
{
    public enum ZoneId
    {
        OuterFarms = 1,
        OuterResidential = 2,
        ArtisanQuarter = 3,
        InnerDistrict = 4,
        Keep = 5,
    }

    public static class ZoneIds
    {
        public static readonly ZoneId[] All = (ZoneId[])Enum.GetValues(typeof(ZoneId));
    }
}
