using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class PersonalCheckEnd : Event
    {
        public PersonalCheckEnd(EventCore core, double time) : base(core, time)
        {
        }
    }
}
