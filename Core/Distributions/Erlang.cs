using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions
{
    public class Erlang
    {
        private readonly int _k;
        private readonly double _lambda;
        private readonly Exponential _exponential;

        public double Mean => _k / _lambda;
        public double Variance => _k / (_lambda * _lambda);

        public Erlang(int shape, double ratePerSeconds)
        {
            if (shape < 1)
                throw new ArgumentOutOfRangeException(nameof(shape), "Shape parameter k must be >= 1.");
            if (ratePerSeconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(ratePerSeconds), "Rate parameter lambda must be > 0.");

            _k = shape;
            _lambda = ratePerSeconds;
            _exponential = new Exponential(ratePerSeconds);
            
        }

        public double NextDouble()
        {
            double sum = 0.0;
            for (int i = 0; i < _k; i++)
                sum += _exponential.NextDouble();
            return sum;
        }
    }
}
