using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Models;

namespace Core.EventSimulation.Airport.Events
{
    public class RontgenEnd : Event
    {
        public Luggage Luggage { get; set; }
        public RontgenEnd(EventCore core, double time, Luggage luggage) : base(core, time)
        {
            Luggage = luggage;
        }
        public override void Execute()
        {
            AirportSimulation airportCore = (AirportSimulation)Core;
            
            if (Luggage.SelectedLine == 1)
            {
                airportCore.RontgenMachine1.IsBusy = false;
                airportCore.RontgenMachine1.ActualLuggage = null;
                Luggage.Status = Models.Enums.Status.AT_OUTPUT_BELT;
                airportCore.OutputBelt1.Luggage.Enqueue(Luggage);
                
                if (!Core.TurboMode)
                {
                    airportCore.Logs1.Enqueue($"Luggage {Luggage.ID} finished rontgen and placed on output belt at time {Time:F2}");
                }
                
                if (airportCore.LuggagePickUpQueue1.Queue.Count > 0 && 
                    airportCore.LuggagePickUpQueue1.Queue.Peek().ID == Luggage.Customer.ID)
                {
                    if (!Core.TurboMode)
                    {
                        airportCore.Events1.Enqueue($"RontgenEnd: Event LuggageClaim for Customer {Luggage.Customer.ID} planned at time: {Time:F2}");
                    }
                    LuggageClaim luggageClaim = new LuggageClaim(Core, Time, Luggage.Customer);
                    Core.PlanEvent(luggageClaim);
                }

                if (airportCore.InputBelt1.Luggage.Queue.Count > 0 && !airportCore.OutputBelt1.Luggage.IsFull())
                {
                    Luggage nextLuggage = airportCore.InputBelt1.Luggage.Dequeue();
                    airportCore.RontgenMachine1.IsBusy = true;
                    airportCore.RontgenMachine1.ActualLuggage = nextLuggage;
                    if (!Core.TurboMode)
                    {
                        airportCore.Events1.Enqueue($"RontgenEnd: Event RontgenStart for Luggage {nextLuggage.ID} planned at time: {Time:F2}");
                    }
                    RontgenStart rontgenStart = new RontgenStart(Core, Time, nextLuggage);
                    Core.PlanEvent(rontgenStart);
                }

                if (Luggage.Customer.Luggage.Count > 0 && !airportCore.InputBelt1.Luggage.IsFull()
                    && Luggage.Customer.Status == Models.Enums.Status.LUGGAGE_STORE)
                {
                    if (!Core.TurboMode)
                    {
                        airportCore.Events1.Enqueue($"RontgenEnd: Event LuggageStorage for Customer {Luggage.Customer.ID} planned at time: {Time:F2}");
                    }
                    LuggageStorage luggageStorage = new LuggageStorage(Core, Time, Luggage.Customer);
                    Core.PlanEvent(luggageStorage);
                } 
                else
                {
                    if (airportCore.ArrivalQueue1.Queue.Count > 0 && !airportCore.InputBelt1.IsFull())
                    {
                        Customer nextCustomer = airportCore.ArrivalQueue1.Queue.Peek();
                        
                        if (!Core.TurboMode)
                        {
                            airportCore.Events1.Enqueue($"RontgenEnd: Event LuggageStorage for Customer {nextCustomer.ID} planned at time: {Time:F2}");
                        }
                        LuggageStorage luggageStorage = new LuggageStorage(Core, Time, nextCustomer);
                        Core.PlanEvent(luggageStorage);
                    }
                }
            }
            else
            {
                airportCore.RontgenMachine2.IsBusy = false;
                airportCore.RontgenMachine2.ActualLuggage = null;
                Luggage.Status = Models.Enums.Status.AT_OUTPUT_BELT;
                airportCore.OutputBelt2.Luggage.Enqueue(Luggage);
                if (!Core.TurboMode)
                {
                    airportCore.Logs2.Enqueue($"Luggage {Luggage.ID} finished rontgen and placed on output belt at time {Time:F2}");
                }
                
                if (airportCore.LuggagePickUpQueue2.Queue.Count > 0 && 
                    airportCore.LuggagePickUpQueue2.Queue.Peek().ID == Luggage.Customer.ID)
                {
                    if (!Core.TurboMode)
                    {
                        airportCore.Events2.Enqueue($"RontgenEnd: Event LuggageClaim for Customer {Luggage.Customer.ID} planned at time: {Time:F2}");
                    }
                    LuggageClaim luggageClaim = new LuggageClaim(Core, Time, Luggage.Customer);
                    Core.PlanEvent(luggageClaim);
                }

                if (airportCore.InputBelt2.Luggage.Queue.Count > 0 && !airportCore.OutputBelt2.Luggage.IsFull())
                {
                    Luggage nextLuggage = airportCore.InputBelt2.Luggage.Dequeue();
                    airportCore.RontgenMachine2.IsBusy = true;
                    airportCore.RontgenMachine2.ActualLuggage = nextLuggage;
                    
                    if (!Core.TurboMode)
                    {
                        airportCore.Events2.Enqueue($"RontgenEnd: Event RontgenStart for Luggage {nextLuggage.ID} planned at time: {Time:F2}");
                    }
                    RontgenStart rontgenStart = new RontgenStart(Core, Time, nextLuggage);
                    Core.PlanEvent(rontgenStart);
                }

                if (Luggage.Customer.Luggage.Count > 0 && !airportCore.InputBelt2.Luggage.IsFull()
                    && Luggage.Customer.Status == Models.Enums.Status.LUGGAGE_STORE)
                {
                    if (!Core.TurboMode)
                    {
                        airportCore.Events2.Enqueue($"RontgenEnd: Event LuggageStorage for Customer {Luggage.Customer.ID} planned at time: {Time:F2}");
                    }
                    LuggageStorage luggageStorage = new LuggageStorage(Core, Time, Luggage.Customer);
                    Core.PlanEvent(luggageStorage);
                }
                else
                {
                    if (airportCore.ArrivalQueue2.Queue.Count > 0 && !airportCore.InputBelt2.IsFull())
                    {
                        Customer nextCustomer = airportCore.ArrivalQueue2.Queue.Peek();

                        if (!Core.TurboMode)
                        {
                            airportCore.Events2.Enqueue($"RontgenEnd: Event LuggageStorage for Customer {nextCustomer.ID} planned at time: {Time:F2}");
                        }
                        LuggageStorage luggageStorage = new LuggageStorage(Core, Time, nextCustomer);
                        Core.PlanEvent(luggageStorage);
                    }
                }
            }
        } 
    }
}
