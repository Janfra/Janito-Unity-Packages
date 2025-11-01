namespace Janito.Audio
{
    public static class RangeUtils
    {
        public static void ValidateRangeOrder(ref float minValue, ref float maxValue)
        {
            if (minValue > maxValue)
            {
                float temp = minValue;
                minValue = maxValue;
                maxValue = temp;
            }
        }
    }
}