using Core.EventSimulation.Airport.Models;
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
        public Customer Customer { get; set; }
        public PersonalCheckEnd(EventCore core, double time, Customer customer) : base(core, time)
        {
            Customer = customer;
        }
        public override void Execute()
        {
            AirportSimulation airportCore = (AirportSimulation)Core;

            if (Customer.SelectedLine == 1)
            {
                airportCore.Employee1.IsBusy = false;
                airportCore.Employee1.ActualCustomer = null;
                if (!Core.TurboMode)
                {
                    airportCore.Logs1.Enqueue($"Customer {Customer.ID} finished personal check at time {Time:F2}");
                }
            }
            else 
            {
                airportCore.Employee2.IsBusy = false;
                airportCore.Employee2.ActualCustomer = null;
                if (!Core.TurboMode)
                {
                    airportCore.Logs2.Enqueue($"Customer {Customer.ID} finished personal check at time {Time:F2}");
                }
            }

            if (Customer.LuggageCount == 0)
            {
                LuggageClaim.LeaveSystem(Core, Customer);
                MetalDetectorEnd.MetalDetectorStart(Core, Customer);
                return;
            }

            MetalDetectorEnd.ClaimLuggage(Core, Customer);
            MetalDetectorEnd.MetalDetectorStart(Core, Customer);
        }
    }
}
