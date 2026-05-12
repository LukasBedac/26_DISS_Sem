using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Statistics
{
    public class Statistic
    {
        private static readonly double ZValueFor90PercentConfidence = 1.645;
        private static readonly double ZValueFor95PercentConfidence = 1.96;
        private static readonly double[] StudentsTDistributionCriticalValues = 
            [12.706, 4.303, 3.182, 2.776, 2.571, 
            2.447, 2.365, 2.306, 2.262, 2.228, 
            2.201, 2.179, 2.160, 2.145, 2.131, 
            2.120, 2.110, 2.101, 2.093, 2.086, 
            2.080, 2.074, 2.069, 2.064, 2.060, 
            2.056, 2.052, 2.048, 2.045];

        private double _sumValues;
        private double _sumValuesSquared;
        private int _numberOfValues;

        public double Mean => _numberOfValues == 0 ? 0.0 : _sumValues / _numberOfValues;
        public double StandartDeviation => 
            Math.Sqrt((_sumValuesSquared - _sumValues * _sumValues / _numberOfValues) / (_numberOfValues - 1));

        public (double, double) ConfidenceInterval90()
        {
            if (_numberOfValues == 0)
            {
                return (double.NaN, double.NaN);
            }

            if (_numberOfValues < 30)
            {
                double t = StudentsTDistributionCriticalValues[_numberOfValues - 1];
                double ht = t * StandartDeviation / Math.Sqrt(_numberOfValues);
                return (Mean - ht, Mean + ht);
            }

            double hz = StandartDeviation * ZValueFor90PercentConfidence / Math.Sqrt(_numberOfValues);
            return (Mean - hz, Mean + hz);
        }

        public (double, double) ConfidenceInterval95()
        {
            if (_numberOfValues == 0)
            {
                return (double.NaN, double.NaN);
            }

            if (_numberOfValues < 30)
            {
                double t = StudentsTDistributionCriticalValues[_numberOfValues - 1];
                double ht = t * StandartDeviation / Math.Sqrt(_numberOfValues);
                return (Mean - ht, Mean + ht);
            }

            double hz = StandartDeviation * ZValueFor95PercentConfidence / Math.Sqrt(_numberOfValues);
            return (Mean - hz, Mean + hz);
        }

        public void AddValue(double value)
        {
            _sumValues += value;
            _sumValuesSquared += value * value;
            _numberOfValues++;
        }

        public void ClearValues()
        {
            _sumValues = 0.0;
            _sumValuesSquared = 0.0;
            _numberOfValues = 0;
        }
        public int GetNumberOfValues()
        {
            return _numberOfValues;
        }
    }
}
