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
    public class RontgenStart : Event
    {
        public Luggage Luggage { get; set; }
        public RontgenStart(EventCore core, double time, Luggage luggage) : base(core, time)
        {
            Luggage = luggage;
        }
        public override void Execute()
        {
            AirportSimulation airportCore = (AirportSimulation)Core;
            double duration = airportCore.RontgenMachineDurationGenerator.NextDouble();

            if (Luggage.SelectedLine == 1)
            {
                airportCore.RontgenMachine1.IsBusy = true;
                airportCore.RontgenMachine1.ActualLuggage = Luggage;
                if (!Core.TurboMode)
                {
                    airportCore.Logs1.Enqueue($"Luggage {Luggage.ID} started rontgen scanning at time {Time:F2}");
                    airportCore.Events1.Enqueue($"RontgenStart: Event RontgenEnd for Luggage {Luggage.ID} planned at time: {Time + duration:F2}");
                }
            }
            else
            {
                airportCore.RontgenMachine2.IsBusy = true;
                airportCore.RontgenMachine2.ActualLuggage = Luggage;
                if (!Core.TurboMode)
                {
                    airportCore.Logs2.Enqueue($"Luggage {Luggage.ID} started rontgen scanning at time {Time:F2}");
                    airportCore.Events2.Enqueue($"RontgenStart: Event RontgenEnd for Luggage {Luggage.ID} planned at time: {Time + duration:F2}");
                }
            }

            Luggage.Status = Status.RONTGEN_SCANNING;
            RontgenEnd rontgenEnd = new RontgenEnd(Core, Time + duration, Luggage);
            Core.PlanEvent(rontgenEnd);
        }
    }
}
