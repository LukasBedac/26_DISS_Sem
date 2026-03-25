using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Distributions.Helpers
{
    public class Scope
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public double Probability { get; set; }

        public double TMin { get; set; }
        public double TModus { get; set; }
        public double TMax { get; set; }

        public Scope(int min, int max, double probability)
        {
            Min = min;
            Max = max;
            Probability = probability;
        }
        public Scope(double min, double modus, double max)
        {
            TMin = min;
            TModus = modus;
            TMax = max;
        }
    }
}
