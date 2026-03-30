using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Models
{
    public class TimeEntity
    {
        public double ArrivalTime { get; set; }
        public double LeaveTime { get; set; }

        public double StartWaitTime { get; set; }
        public double EndWaitTime { get; set; }
    }
}
