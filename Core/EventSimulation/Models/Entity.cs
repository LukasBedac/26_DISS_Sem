using OSPABA;
using Simulation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Models
{
    public class Entity
    {
        protected MySimulation _sim { get; set; }
        public double ArrivalTime { get; set; }
        public double LeaveTime { get; set; }
        public double StartWaitTime { get; set; }
        public double EndWaitTime { get; set; }
        public bool IsBusy { get; set; } = false;
        public double TotalBusyTime { get; set; } = 0.0;
        public double LastStartBusyTime { get; set; } = 0.0;
        public virtual void AnimTransfer(double currentTime, double duration, PointF[] path) { }
        public virtual void InitializeEntity() { }
    }
}
