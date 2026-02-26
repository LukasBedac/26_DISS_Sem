using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions.Helpers
{
    public static class SeedGen
    {
        private static readonly Random _random = new Random();

        public static int GetSeed()
        {
            lock (_random)
            {
                return _random.Next();
            }
        }

    }
}
