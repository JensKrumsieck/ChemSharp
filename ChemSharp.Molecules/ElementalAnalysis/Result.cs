namespace ChemSharp.Molecules.ElementalAnalysis
{
    public struct Result
    {
        public string Formula;
        public double Err;
        public double[] Vec;

        public Result(string formula, double err, double[] vec)
        {
            Formula = formula;
            Err = err;
            Vec = vec;
        }
    }
}
