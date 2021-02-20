#if NETSTANDARD2_1
using MathF = System.MathF;
#endif
#if NETSTANDARD2_0
using MathF = ChemSharp.Mathematics.MathF;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace ChemSharp.Mathematics
{
    public static class MathV
    {
        /// <summary>
        /// Calculates the centroid of given vectors by 1/m sum_(i=0 to m) v_i
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Centroid(IEnumerable<Vector3> input)
        {
            var array = input.ToArray();
            return Sum(array) / array.Length;
        }

        /// <summary>
        /// Calculates the Sum of given Vectors
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Sum(IEnumerable<Vector3> input)
        {
            var array = input as Vector3[] ?? input.ToArray();
            var sumX = array.Sum(s => s.X);
            var sumY = array.Sum(s => s.Y);
            var sumZ = array.Sum(s => s.Z);
            return new Vector3(sumX, sumY, sumZ);
        }

        /// <summary>
        /// Wrapper function for <see cref="Vector3.Dot"/>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float Dot(Vector3 left, Vector3 right) => Vector3.Dot(left, right);

        /// <summary>
        /// Wrapper function for <see cref="Vector3.Cross"/>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Vector3 Cross(Vector3 left, Vector3 right) => Vector3.Cross(left, right);

        /// <summary>
        /// Wrapper function for <see cref="Vector3.Normalize"/>
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 Normalize(Vector3 v) => Vector3.Normalize(v);

        /// <summary>
        /// Wrapper function for <see cref="Vector3.Distance"/>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float Distance(Vector3 left, Vector3 right) => Vector3.Distance(left, right);

        /// <summary>
        /// Projects a point onto plane p
        /// </summary>
        /// <param name="p"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 Project(Plane p, Vector3 point)
        {
            var dotProduct = Dot(p.Normal, point);
            var projVec = (dotProduct + p.D) * p.Normal;
            return point - projVec;
        }

        /// <summary>
        /// Calculates distance to plane
        /// </summary>
        /// <param name="p"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float Distance(Plane p, Vector3 point)
        {
            var projection = Project(p, point);
            var vectorTo = point - projection;
            return Dot(vectorTo, p.Normal);
        }

        /// <summary>
        /// Returns an Angle between three Vectors in Degrees
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        /// 
        public static float Angle(Vector3 a, Vector3 b, Vector3 c) => (180f / MathF.PI) * RadAngle(a, b, c);

        /// <summary>
        /// Calculates an angle between two planes
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Angle(Plane p1, Plane p2)
        {
            var d = MathF.Abs((p1.Normal.X * p2.Normal.X) + (p1.Normal.Y * p2.Normal.Y) + (p1.Normal.Z * p2.Normal.Z));
            var e1 = MathF.Pow(p1.Normal.X, 2) + MathF.Pow(p1.Normal.Y, 2) + MathF.Pow(p1.Normal.Z, 2);
            var e2 = MathF.Pow(p2.Normal.X, 2) + MathF.Pow(p2.Normal.Y, 2) + MathF.Pow(p2.Normal.Z, 2);

            return 180f / MathF.PI * MathF.Acos(d / (MathF.Sqrt(e1) * MathF.Sqrt(e2)));
        }

        /// <summary>
        /// Returns an Angle between three Vectors in Radian
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float RadAngle(Vector3 a, Vector3 b, Vector3 c)
        {
            var b1 = Vector3.Normalize(a - b);
            var b2 = Vector3.Normalize(c - b);
            return MathF.Acos(Dot(b1, b2));
        }

        /// <summary>
        /// Returns a Dihedral of 4 Vectors in Degrees
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        /// 
        public static float Dihedral(Vector3 a, Vector3 b, Vector3 c, Vector3 d) => (180 / MathF.PI) * RadDihedral(a, b, c, d);
        /// <summary>
        /// Returns a Dihedral of 4 Vectors in Radian
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float RadDihedral(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var b1 = -(a - b);
            var b2 = b - c;
            var b3 = d - c;

            var c1 = Normalize(Cross(b1, b2));
            var c2 = Normalize(Cross(b2, b3));
            var c3 = Normalize(Cross(c1, b2));

            return MathF.Atan2(Dot(c3, c2), Dot(c1, c2));
        }

        /// <summary>
        /// Gets the mean plane of a list of atoms
        /// </summary>
        /// <returns></returns>
        public static Plane MeanPlane(IList<Vector3> input)
        {
            //calculate Centroid first
            //get the centroid
            var centroid = Centroid(input);

            //subtract centroid from each point... & build matrix of that
            var A = Matrix<float>.Build.Dense(3, input.Count);
            for (var x = 0; x < input.Count; x++)
            {
                A[0, x] = (input[x] - centroid).X;
                A[1, x] = (input[x] - centroid).Y;
                A[2, x] = (input[x] - centroid).Z;
            }

            //get svd
            var svd = A.Svd(true);

            //get plane unit vector
            var a = svd.U[0, 2];
            var b = svd.U[1, 2];
            var c = svd.U[2, 2];

            var d = -Dot(centroid, new Vector3(a, b, c));

            return new Plane(a, b, c, d);
        }

    }
}