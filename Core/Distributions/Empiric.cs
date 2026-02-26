using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions
{
    public class Empiric
    {
        private readonly double E = 0.00001;
        private List<Scope> _intervals;
        private List<Random>? _randoms;

        public bool IsDiscrete { get; }

        public Empiric(bool isDiscrete, List<Scope> intervals) 
        {
            IsDiscrete = isDiscrete;
            _intervals = intervals;
            CheckIntervals();
            SetIntervals();

        }

        private void CheckIntervals()
        {
            double prob = 0;

            _intervals.ForEach(interval => prob += interval.Probability);

            if (prob != 1 + E)
            {
                throw new Exception("Probablity is not equal to 1, check values");
            }
        }

        private void SetIntervals()
        {
            _randoms = new List<Random>();
            foreach (Scope interval in _intervals)
            {
                _randoms.Add(new Random(SeedGen.GetSeed()));
            }

            if (_randoms.Count != _intervals.Count) 
            {
                throw new Exception("Invalid number of generators and intervals");
            }
        }

        public int NextInt()
        {
            if (!IsDiscrete)
            {
                throw new InvalidOperationException("This method can be used only with discrete values.");
            }
            
            double cumulativeProb = 0;

            for (int i = 0; i < _intervals.Count; i++)
            {
                cumulativeProb += _intervals[i].Probability;
                //if ()
            }
            throw new Exception("This should be unreachable code - Empiric distribution, discrete value return");
        }

        public double NextDouble()
        {
            if (IsDiscrete)
            {
                throw new InvalidOperationException("This method can be used only with continuoues values.");
            }
            return 0;
        }

    }
}
