using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class SleepEvent : Event
    {
        private double _sleepMs;
        private int _interval;
        public SleepEvent(EventCore core, double time, double sleepMs, int interval) : base(core, time)
        {
            _sleepMs = sleepMs;
            _interval = interval;
        }

        public override void Execute()
        {
            SleepEvent newEvent = new SleepEvent(Core, Time + _interval, _sleepMs, _interval);
            Core.PlanEvent(newEvent);

            Core.RefreshGUI();
            int sleep = (int)(_sleepMs / ((AirportSimulation)Core).TimeMultiplier);
            Thread.Sleep(sleep);
        }
    }
}
