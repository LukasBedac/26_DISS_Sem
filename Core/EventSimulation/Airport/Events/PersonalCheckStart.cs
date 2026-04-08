using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Airport.Models.Enums;
using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class PersonalCheckStart : Event
    {
        public Customer Customer { get; set; }
        public PersonalCheckStart(EventCore core, double time, Customer customer) : base(core, time)
        {
            Customer = customer;
        }
        public override void Execute()
        {
            double duration = ((AirportSimulation)Core).PersonalCheckDurationGenerator.NextDouble();
            if (!Core.TurboMode)
            {
                if (Customer.SelectedLine == 1)
                {
                    ((AirportSimulation)Core).Logs1.Enqueue($"Customer {Customer.ID} starts personal check at time {Time:F2}");
                    ((AirportSimulation)Core).Events1.Enqueue($"PersonalCheckStart: Event PersonalCheckEnd for Customer {Customer.ID} planned at time: {Time + duration:F2}");
                }
                else
                {
                    ((AirportSimulation)Core).Logs2.Enqueue($"Customer {Customer.ID} starts personal check at time {Time:F2}");
                    ((AirportSimulation)Core).Events2.Enqueue($"PersonalCheckStart: Event PersonalCheckEnd for Customer {Customer.ID} planned at time: {Time + duration:F2}");
                }
            }
            PersonalCheckEnd personalCheckEnd = new PersonalCheckEnd(Core, Time + duration, Customer);
            Core.PlanEvent(personalCheckEnd);
        }
    }
}
