namespace ChemSharp.Mathematics
{
    public static class MathUtil
    {
        /// <summary>
        /// Checks if number is power of 2
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool PowerOf2(int x) => (x & (x - 1)) == 0;

        /// <summary>
        /// returns the next power of 2 of xs
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int NextPowerOf2(int x) => (int)System.Math.Pow(2, System.Math.Floor(System.Math.Log(x, 2)) + 1);
    }
}
