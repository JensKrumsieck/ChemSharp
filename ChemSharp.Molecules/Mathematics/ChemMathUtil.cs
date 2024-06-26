﻿namespace ChemSharp.Molecules.Mathematics;

public static class ChemMathUtil
{
	/// <summary>
	///     See BondTo Method
	/// </summary>
	private const float Delta = 25f;

	/// <summary>
	///     Tests if Atom is Bond to another based on distance!
	///     allow uncertainty of <see cref="Delta" /> overall
	/// </summary>
	/// <param name="atom"></param>
	/// <param name="test"></param>
	/// <param name="delta"></param>
	/// <returns></returns>
	public static bool BondToByCovalentRadii(this Atom atom, Atom test, float delta = Delta)
	{
		var check = (atom.CovalentRadius + (float)test.CovalentRadius + delta) / 100f;
		return atom.DistanceToSquared(test) < check * check;
	}
}
