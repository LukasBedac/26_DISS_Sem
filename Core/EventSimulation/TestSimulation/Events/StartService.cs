using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.TestSimulation.Events
{
    public class StartService : Event
    {
        private TestSimCore _core;
        public StartService(EventCore core, double time) : base(core, time)
        {
            _core = (TestSimCore)core;
        }

        public override void Execute()
        {
            double serviceTime = _core.ServiceTimegenerator.NextDouble();

            _core.CurrentCustomer.EndWaitTime = Time;
            double waitTime = _core.CurrentCustomer.EndWaitTime - _core.CurrentCustomer.StartWaitTime;
            _core.WaitTimeStat.AddValue(waitTime);

            Event endService = new EndService(_core, Time + serviceTime);
            _core.PlanEvent(endService);
        }
    }
}
