using System;

namespace Services.ExtensionMethods
{
    public static class DecimalExtensionMethods
    {
        public static int RoundToNearestInt(this decimal value)
        {
            return Convert.ToInt32(Math.Round(value, MidpointRounding.AwayFromZero)); ;
        }
    }
}
