using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.TestSimulation.Events
{
    public class EndService : Event
    {
        private TestSimCore _core;
        public EndService(EventCore core, double time) : base(core, time)
        {
            _core = (TestSimCore)core;
        }

        public override void Execute()
        {
            _core.CurrentCustomer.LeaveTime = Time;

            if (_core.ServiceQueue.Queue.Count > 0)
            {
                Car car = _core.ServiceQueue.Dequeue();
                
                // === DUPLICATE CODE IN ARRIVAL - time errors ===
                //car.EndWaitTime = Time;                
                //double waitTime = car.EndWaitTime - car.StartWaitTime;
                //_core.WaitTimeStat.AddValue(waitTime);

                _core.CurrentCustomer = car;

                Event startService = new StartService(_core, Time);
                _core.PlanEvent(startService);
            }
            else
            {
                _core.IsBusy = false;
            }
        }
    }
}
