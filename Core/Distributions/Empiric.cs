using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions
{
    public class Empiric
    {
        private readonly double E = 0.000001;
        private List<Scope> _intervals;
        private List<Random>? _randoms;
        private Random _probabilityRandom;
        public bool IsDiscrete { get; }

        public Empiric(bool isDiscrete, List<Scope> intervals) 
        {
            IsDiscrete = isDiscrete;
            _intervals = intervals;
            CheckIntervals();
            SetRandoms();
            _probabilityRandom = new Random(SeedGen.GetSeed());
        }

        private void CheckIntervals()
        {
            double prob = 0;

            _intervals.ForEach(interval => prob += interval.Probability);

            if (prob != 1)
            {
                throw new Exception("Probablity is not equal to 1, check values");
            }
        }

        private void SetRandoms()
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
            double randomValue = _probabilityRandom.NextDouble();
            for (int i = 0; i < _intervals.Count; i++)
            {
                cumulativeProb += _intervals[i].Probability;
                if (randomValue < cumulativeProb + E)
                {
                    if (_randoms == null || _randoms[i] == null) throw new Exception("Generator for random values is null, check inicialization");
                    int returnNumber = GenerateIntNumber(i);
                    return returnNumber;
                }
            }
            throw new Exception("This should be unreachable code - [Empiric distribution, discrete value return]");
        }
        
        public double NextDouble()
        {
            if (IsDiscrete)
            {
                throw new InvalidOperationException("This method can be used only with continuoues values.");
            }

            double cumulativeProb = 0;
            double randomValue = _probabilityRandom.NextDouble();

            for (int i = 0; i < _intervals.Count; i++)
            {
                cumulativeProb += _intervals[i].Probability;
                if (randomValue < cumulativeProb + E)
                {
                    if (_randoms == null || _randoms[i] == null) throw new Exception("Generator for random values is null, check inicialization");
                    double returnNumber = GenerateDoubleNumber(i);
                    return returnNumber;
                }
            }
            throw new Exception("This should be unreachable code - [Empiric distribution, continoues value return]");

        }
        /// <summary>
        /// Method for generating number in correct interval
        /// </summary>
        /// <param name="i"> Decides the correct interval</param>
        private int GenerateIntNumber(int i)
        {
            if (_randoms != null && i > _randoms.Count - 1) throw new IndexOutOfRangeException("Index of random is greater than intervals in distribution");

            int min = _intervals[i].Min;
            int max = _intervals[i].Max;

            return _randoms[i].Next(min, max);

        }

        private double GenerateDoubleNumber(int i)
        {
            if (_randoms != null && i > _randoms.Count - 1) throw new IndexOutOfRangeException("Index of random is greater than intervals in distribution");

            int min = _intervals[i].Min;
            int max = _intervals[i].Max;

            return min + (_randoms[i].NextDouble() * (max - min));
        }
    }
}
