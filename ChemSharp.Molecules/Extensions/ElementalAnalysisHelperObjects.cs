namespace ChemSharp.Molecules.Extensions
{
    /// <summary>
    /// struct for impurity handling
    /// </summary>
    public class Impurity
    {
        public string Formula;
        public double Lower;
        public double Upper;
        public double Step;

        public Impurity(string formula, double lower, double upper, double step)
        {
            this.Formula = formula;
            this.Lower = lower;
            this.Upper = upper;
            this.Step = step;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Result
    {
        public string Formula;
        public double Err;
        public double[] Vec;

        public Result(string formula, double err, double[] vec)
        {
            this.Formula = formula;
            this.Err = err;
            this.Vec = vec;
        }
    }
}
