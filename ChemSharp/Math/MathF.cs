#if NETSTANDARD2_0
namespace ChemSharp.Math
{
    public static class MathF
    {
        public const float PI = 3.1415926f;
        public static float Sqrt(float input) => Pow(input, 0.5f);
        public static float Pow(float x, float y) => (float) System.Math.Pow(x, y);
        public static float Sin(float a) => (float) System.Math.Sin(a);
        public static float Cos(float d) => (float) System.Math.Cos(d);
        public static float Atan(float d) => (float) System.Math.Atan(d);
    }
}
#endif