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
        private int _min;
        private int _max;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">Minimum is inclusive - value can be generated</param>
        /// <param name="max">Maximum is exclusive - value won;t be generated</param>
        public DiscreteUniform(int min, int max)
        {
            _min = min;
            _max = max;
            _random = new Random(SeedGen.GetSeed());
        }

        public int NextInt()
        {
            return _random.Next(_min, _max);
        }
    }
}
