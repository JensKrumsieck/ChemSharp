using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
#if NETSTANDARD2_1
using MathF = System.MathF;
#endif

#if NETSTANDARD2_0
#endif

namespace ChemSharp.Mathematics;

public static class MathV
{
	/// <summary>
	///     Calculates the centroid of given vectors by 1/m sum_(i=0 to m) v_i
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static Vector3 Centroid(this IEnumerable<Vector3> input)
	{
		var array = input.ToArray();
		return Sum(array) / array.Length;
	}

	/// <summary>
	///     Calculates the Sum of given Vectors
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

	/// <summary>
	///     Projects a point onto plane p
	/// </summary>
	/// <param name="p"></param>
	/// <param name="point"></param>
	/// <returns></returns>
	public static Vector3 Project(this Plane p, Vector3 point)
	{
		var dotProduct = Vector3.Dot(p.Normal, point);
		var projVec = (dotProduct + p.D) * p.Normal;
		return point - projVec;
	}

	/// <summary>
	///     Calculates distance to plane
	/// </summary>
	/// <param name="p"></param>
	/// <param name="point"></param>
	/// <returns></returns>
	public static float Distance(this Plane p, Vector3 point)
	{
		var projection = Project(p, point);
		var vectorTo = point - projection;
		return Vector3.Dot(vectorTo, p.Normal);
	}

	/// <summary>
	///     Returns an Angle between three Vectors in Degrees
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	/// <returns></returns>
	public static float Angle(Vector3 a, Vector3 b, Vector3 c) => 180f / MathF.PI * RadAngle(a, b, c);

	/// <summary>
	///     Calculates an angle between two planes
	/// </summary>
	/// <param name="p1"></param>
	/// <param name="p2"></param>
	/// <returns></returns>
	public static double Angle(Plane p1, Plane p2)
	{
		var d = Math.Abs(p1.Normal.X * p2.Normal.X + p1.Normal.Y * p2.Normal.Y + p1.Normal.Z * p2.Normal.Z);
		var e1 = MathF.Pow(p1.Normal.X, 2) + MathF.Pow(p1.Normal.Y, 2) + MathF.Pow(p1.Normal.Z, 2);
		var e2 = MathF.Pow(p2.Normal.X, 2) + MathF.Pow(p2.Normal.Y, 2) + MathF.Pow(p2.Normal.Z, 2);

		return 180f / MathF.PI * MathF.Acos(d / (MathF.Sqrt(e1) * MathF.Sqrt(e2)));
	}

	/// <summary>
	///     Returns an Angle between three Vectors in Radian
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	/// <returns></returns>
	public static float RadAngle(Vector3 a, Vector3 b, Vector3 c)
	{
		var b1 = Vector3.Normalize(a - b);
		var b2 = Vector3.Normalize(c - b);
		return MathF.Acos(Vector3.Dot(b1, b2));
	}

	/// <summary>
	///     Returns a Dihedral of 4 Vectors in Degrees
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	/// <param name="d"></param>
	/// <returns></returns>
	public static float Dihedral(Vector3 a, Vector3 b, Vector3 c, Vector3 d) =>
		180 / MathF.PI * RadDihedral(a, b, c, d);

	/// <summary>
	///     Returns a Dihedral of 4 Vectors in Radian
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

		var c1 = Vector3.Normalize(Vector3.Cross(b1, b2));
		var c2 = Vector3.Normalize(Vector3.Cross(b2, b3));
		var c3 = Vector3.Normalize(Vector3.Cross(c1, b2));

		return MathF.Atan2(Vector3.Dot(c3, c2), Vector3.Dot(c1, c2));
	}

	/// <summary>
	///     Gets the mean plane of a list of vectors
	/// </summary>
	/// <returns></returns>
	public static Plane MeanPlane(this IList<Vector3> input)
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
		var svd = A.Svd();

		//get plane unit vector
		var a = svd.U[0, 2];
		var b = svd.U[1, 2];
		var c = svd.U[2, 2];

		var d = -Vector3.Dot(centroid, new Vector3(a, b, c));

		return new Plane(a, b, c, d);
	}
}
