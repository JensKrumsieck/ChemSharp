using System.Collections.Generic;

namespace ChemSharp.Mathematics
{
    public static class DataPointMath
    {
        /// <summary>
        /// Calculates Derivative of DataPoint Collection
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<DataPoint> Derive(this IList<DataPoint> input)
        {
            for (var i = 0; i < input.Count; i++)
            {
                yield return i == 0
                    ? new DataPoint(input[i].X, 0)
                    : input[i].Derivative(input[i - 1]);
            }
        }

        /// <summary>
        /// Calculates Slope at current position with given previous element
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        internal static DataPoint Derivative(this DataPoint current, DataPoint previous) => new DataPoint(current.X, (current.Y - previous.Y) / (current.X - previous.X));

        /// <summary>
        /// Integral of DataPoint Collection
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<DataPoint> Integrate(this IList<DataPoint> input)
        {
            var cache = DataPoint.Zero;
            for (var i = 0; i < input.Count; i++)
            {
                var value = Integration(input[i], cache);
                cache = value;
                yield return i == 0
                    ? input[i]
                    : value;
            }
        }

        /// <summary>
        /// Calculates Integral at current position with given previous integrate element
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        internal static DataPoint Integration(this DataPoint current, DataPoint previous) => new DataPoint(current.X, current.Y + previous.Y);
    }
}
