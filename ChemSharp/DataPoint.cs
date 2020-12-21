using System;
using System.Collections.Generic;

namespace ChemSharp
{
    public struct DataPoint
    {
        public static readonly DataPoint Zero = new DataPoint(0, 0);

        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Returns a string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"({X},{Y})";

        /// <summary>
        /// Converts to double arrays in DataPoint Collection
        /// </summary>
        /// <param name="xData"></param>
        /// <param name="yData"></param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static IEnumerable<DataPoint> FromDoubles(double[] xData, double[] yData)
        {
            if (xData.Length != yData.Length) throw new Exception("X and Y Length does not match");
            for (var i = 0; i < xData.Length; i++) yield return new DataPoint(xData[i], yData[i]);
        }
    }
}
