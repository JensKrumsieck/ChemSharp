namespace ChemSharp.Extensions;

public static class ArrayExtensions
{
	/// <summary>
	///     Gets Column of Multidimensional Array
	/// </summary>
	/// <param name="matrix"></param>
	/// <param name="columnNumber"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T[] GetColumn<T>(this T[,] matrix, int columnNumber) =>
		Enumerable.Range(0, matrix.GetLength(0))
		          .Select(x => matrix[x, columnNumber])
		          .ToArray();

	/// <summary>
	///     Gets Row of Multidimensional Array
	/// </summary>
	/// <param name="matrix"></param>
	/// <param name="rowNumber"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T[] GetRow<T>(this T[,] matrix, int rowNumber) =>
		Enumerable.Range(0, matrix.GetLength(1))
		          .Select(x => matrix[rowNumber, x])
		          .ToArray();

	public static int[] FindAllIndicesOf<T>(this T[] array, Predicate<T> match) =>
		array.Select((value, index) => match(value) ? index : -1)
		     .Where(index => index != -1).ToArray();
}
