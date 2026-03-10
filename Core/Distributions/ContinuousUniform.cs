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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">Minimum is inclusive - value can be generated</param>
        /// <param name="max">Maximum is exclusive - value won;t be generated</param>
        public ContinuousUniform(double min, double max)
        {
            Min = min;
            Max = max;
            _random = new Random(SeedGen.GetSeed());
        }

        public double NextDouble()
        {
            return Min + (_random.NextDouble() * (Max - Min));
        }
    }
}
