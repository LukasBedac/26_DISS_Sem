using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Models
{
    public class Event
    {
        public EventCore Core { get; set; }
        public double Time { get; set; }
        public Event(EventCore core, double time)
        {
            Core = core;
            Time = time;
        }

        public virtual void Execute() { }
    }
}
