#if NETSTANDARD2_0
namespace ChemSharp.Mathematics
{
    public static class MathF
    {
        public const float PI = 3.1415926f;
        public static float Sqrt(float d) => (float)System.Math.Sqrt(d);
        public static float Pow(float x, float y) => (float)System.Math.Pow(x, y);
        public static float Sin(float a) => (float)System.Math.Sin(a);
        public static float Cos(float d) => (float)System.Math.Cos(d);
        public static float Atan(float d) => (float)System.Math.Atan(d);
        public static float Acos(float d) => (float)System.Math.Acos(d);
        public static float Atan2(float y, float x) => (float)System.Math.Atan2(y, x);
        public static float Abs(float d) => System.Math.Abs(d);
    }
}
#endif