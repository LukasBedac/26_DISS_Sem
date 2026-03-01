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
        public Scope(int min, int max, double probability)
        {
            Min = min;
            Max = max;
            Probability = probability;
        }
    }
}
