namespace Eden
{
    public static class Calc
    {
        public static float Threshold(float value, float threshold)
        {
            if (Math.Abs(value) >= threshold)
                return value;
            else
                return 0;
        }

        public static float SignThreshold(float value, float threshold)
        {
            if (Math.Abs(value) >= threshold)
                return Math.Sign(value);
            else
                return 0;
        }
    }
}