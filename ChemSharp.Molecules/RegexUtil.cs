using System.Text.RegularExpressions;

namespace ChemSharp.Molecules
{
    public static class RegexUtil
    {

        /// <summary>
        /// the regex pattern for molecular formulas
        /// </summary>
        private const string SumFormulaPattern = @"([A-Z][a-z]*)(\d*[.]?\d*)|(\()|(\))(\d*[.]?\d*)";

        /// <summary>
        /// Regex Pattern for parsing xyz files
        /// </summary>
        private const string XYZPattern = @"([A-Z][a-z]{0,1}){1}\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)";

        public static readonly Regex AtomLabel = new(@"([A-Z][a-z]*)");
        public static readonly Regex XYZString = new(XYZPattern);
        public static readonly Regex SumFormula = new(SumFormulaPattern);
        public static readonly Regex PointMatch = new("[.]");
    }
}
