using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions
{
    public class Exponential
    {
        private double _lambda;
        private Random _random;
        public Exponential(double ratePerHour)
        {
            _lambda = ratePerHour;
            _random = new Random(SeedGen.GetSeed());
        }
        public double NextDouble()
        {
            double u = _random.NextDouble();
            return -Math.Log(1 - u) / _lambda;
        }
    }
}
