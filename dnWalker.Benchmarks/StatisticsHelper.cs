using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public static class StatisticsHelper
    {
        public static (double mean, double delta) GetConfidenceInterval(double[] samples, double cfd = 0.95)
        {
            (double m, double s) = MathNet.Numerics.Statistics.Statistics.MeanStandardDeviation(samples);

            if (s == 0) return (m, 0.0);

            int dof = samples.Length - 1;
            double alpha = (1 - cfd) / 2;
            double t = Math.Abs(MathNet.Numerics.Distributions.StudentT.InvCDF(0, 1, dof, alpha));
            double dlt = t * (s / Math.Sqrt(samples.Length));

            return (m, dlt);
        }
    }
}
