using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions
{
    public class ContinuousUniform
    {
        private readonly Random _random;
        public double Min { get; set; }
        public double Max { get; set; }

        public ContinuousUniform(double min, double max)
        {
            Min = min;
            Max = max;
            _random = new Random(SeedGen.GetSeed());
        }

        public double NextDouble()
        {
            return _random.NextDouble() * (Max - Min) + Min;
        }
    }
}
