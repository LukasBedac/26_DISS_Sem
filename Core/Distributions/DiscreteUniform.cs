using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions
{
    public class DiscreteUniform
    {
        private readonly Random _random;
        public int Min { get; set; }
        public int Max { get; set; }

        public DiscreteUniform(int min, int max)
        {
            Min = min;
            Max = max;
            _random = new Random(SeedGen.GetSeed());
        }

        public double NextInt()
        {
            return _random.Next(Min, Max);
        }
    }
}
