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
        private double _min;
        private double _max;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">Minimum is inclusive - value can be generated</param>
        /// <param name="max">Maximum is exclusive - value won;t be generated</param>
        public ContinuousUniform(double min, double max)
        {
            _min = min;
            _max = max;
            _random = new Random(SeedGen.GetSeed());
        }

        public double NextDouble()
        {
            return _min + (_random.NextDouble() * (_max - _min));
        }
    }
}
