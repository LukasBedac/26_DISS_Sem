using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class PersonalCheckStart : Event
    {
        public PersonalCheckStart(EventCore core, double time) : base(core, time)
        {
        }
    }
}
