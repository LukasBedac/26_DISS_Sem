using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Statistics
{
    public class WeightedStatistic
    {
        private double _sumWeightedValues;
        private double _sumWeights;

        public double Mean => _sumWeightedValues / _sumWeights;

        public void AddValue(double value, double weight)
        {
            _sumWeightedValues += value * weight;
            _sumWeights += weight;
        }

        public void ClearValues()
        {
                _sumWeightedValues = 0.0;
                _sumWeights = 0.0;
        }
    }
}
