using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Airport.Models.Enums;
using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class MetalDetectorStart : Event
    {
        public Customer Customer { get; set; }
        public MetalDetectorStart(EventCore core, double time, Customer customer) : base(core, time)
        {
            Customer = customer;
        }
        public override void Execute()
        {
            AirportSimulation airportCore = (AirportSimulation)Core;
            Customer.Status = Status.DETECTOR_SCANNING;
            double duration = airportCore.MetalDetectorDurationGenerator.NextDouble();
            if (!Core.TurboMode)
            {
                if (Customer.SelectedLine == 1)
                {
                    airportCore.Logs1.Enqueue($"Customer {Customer.ID} started metal scan at time {Time:F2}");
                    airportCore.Events1.Enqueue($"MetalDetectorStart: Event MetalDetectorEnd for Customer {Customer.ID} planned at time: {Time + duration:F2}");

                }
                else
                {
                    airportCore.Logs2.Enqueue($"Customer {Customer.ID} started metal scan at time {Time:F2}");
                    airportCore.Events2.Enqueue($"MetalDetectorStart: Event MetalDetectorEnd for Customer {Customer.ID} planned at time: {Time + duration:F2}");
                }
            }
            MetalDetectorEnd metalDetectorEnd = new MetalDetectorEnd(Core, Time + duration, Customer);
            Core.PlanEvent(metalDetectorEnd);
        }
    }
}
