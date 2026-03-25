using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.TestSimulation.Events
{
    public class Arrival : Event
    {
        private TestSimCore _core;
        private Car _car;
        public Arrival(EventCore core, double time) : base(core, time)
        {
            _core = (TestSimCore)core;
            _car = new Car();
        }

        public override void Execute()
        {
            Event newArrival = new Arrival(_core, Time + _core.ArrivalGenerator.NextDouble());
            _core.PlanEvent(newArrival);

            _car.ArrivalTime = Time;
            _car.StartWaitTime = Time;

            if (_core.ServiceQueue.Queue.Count < 1 && !_core.IsBusy)
            {
                _core.IsBusy = true;
                _core.CurrentCustomer = _car;
                Event startService = new StartService(_core, Time);
                _core.PlanEvent(startService);
            }
            else
            {
                _core.ServiceQueue.Enqueue(_car);
            }
        }
    }
}
