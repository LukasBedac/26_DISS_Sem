using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions
{
    public class Triangular
    {
        private double _min;
        private double _modus;
        private double _max;
        private Random _random;
        public Triangular(Scope scope)
        {
            _min = scope.TMin;
            _modus = scope.TModus;
            _max = scope.TMax;
            _random = new Random(SeedGen.GetSeed());
        }

        public double NextDouble()
        {
            double u = _random.NextDouble();
            var fc = (_modus - _min) / (_max - _min);

            if (u < fc)
            {
                return _min + Math.Sqrt(u * (_max - _min) * (_modus - _min));
            } else
            {
                return _max - Math.Sqrt((1 - u) * (_max - _min) * (_max - _modus));
            }
        }
    }
}
