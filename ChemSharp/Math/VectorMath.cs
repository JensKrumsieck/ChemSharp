using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Math
{
    public static class VectorMath
    {
        /// <summary>
        /// Calculates the centroid of given vectors by 1/m sum_(i=0 to m) v_i
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Centroid(this IEnumerable<Vector3> input)
        {
            var array = input.ToArray();
            return array.Sum() / array.Length;
        }

        /// <summary>
        /// Calculates the Sum of given Vectors
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Sum(this IEnumerable<Vector3> input)
        {
            var array = input as Vector3[] ?? input.ToArray();
            var sumX = array.Sum(s => s.X);
            var sumY = array.Sum(s => s.Y);
            var sumZ = array.Sum(s => s.Z);
            return new Vector3(sumX, sumY, sumZ);
        }

        // <summary>
        /// Wrapper for dot product
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float Dot(this Vector3 left, Vector3 right) => Vector3.Dot(left, right);

        /// <summary>
        /// Calculates distance to plane
        /// </summary>
        /// <param name="p"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float Distance(this Plane p, Vector3 point)
        {
            var projection = p.Project(point);
            var vectorTo = point - projection;
            return vectorTo.Dot(p.Normal);
        }

        /// <summary>
        /// Projects a point onto plane p
        /// </summary>
        /// <param name="p"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 Project(this Plane p, Vector3 point)
        {
            var dotProduct = p.Normal.Dot(point);
            var projVec = (dotProduct + p.D) * p.Normal;
            return point - projVec;
        }

        /// <summary>
        /// converts float array [3] to vector3
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this float[] input) => new Vector3(input[0], input[1], input[2]);
    }
}
