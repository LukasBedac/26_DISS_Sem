using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class LuggageClaim : Event
    {
        Customer Customer { get; set; }
        public LuggageClaim(EventCore core, double time, Customer customer) : base(core, time)
        {
            Customer = customer;
        }
        public override void Execute()
        {
            if (Customer.HasAllLuggage() || Customer.Status == Models.Enums.Status.CHECKED)
            {
                return;
            }

            AirportSimulation airportCore = (AirportSimulation)Core;
            if (Customer.LuggageCount == 0)
            {
                LeaveSystem(Core, Customer);
                if (!Core.TurboMode)
                {
                    if (Customer.SelectedLine == 1)
                    {
                        airportCore.Logs1.Enqueue($"Customer {Customer.ID} left the system at time {Time:F2}");
                    }
                    else
                    {
                        airportCore.Logs2.Enqueue($"Customer {Customer.ID} left the system at time {Time:F2}");
                    }
                }
                return;
            }

            if (Customer.SelectedLine == 1)
            {
                if (airportCore.OutputBelt1.Luggage.Queue.Count > 0 && 
                    airportCore.OutputBelt1.Luggage.Queue.Peek().Customer.ID == Customer.ID)
                {
                    Luggage luggage = airportCore.OutputBelt1.Luggage.Dequeue();
                    
                    if (!Core.TurboMode)
                    {
                        airportCore.Logs1.Enqueue($"Customer {Customer.ID} claimed luggage {luggage.ID} at time {Time:F2}");
                    }

                    Customer.Luggage.Add(luggage);
                    if (Customer.HasAllLuggage())
                    {
                        airportCore.LuggagePickUpQueue1.Dequeue();
                        LeaveSystem(Core, Customer);
                        if (!Core.TurboMode)
                        {
                            airportCore.Logs1.Enqueue($"Customer {Customer.ID} left the system at time {Time:F2}");
                        }
                    }
                    else
                    {
                        if (!Core.TurboMode)
                        {
                            airportCore.Events1.Enqueue($"LuggageClaim: Event LuggageClaim for Customer {Customer.ID} planned at time: {Time:F2}");
                        }
                        LuggageClaim luggageClaim = new LuggageClaim(Core, Time, Customer);
                        Core.PlanEvent(luggageClaim);
                    }
                    if (!airportCore.RontgenMachine1.IsBusy &&
                        airportCore.InputBelt1.Luggage.Queue.Count > 0 &&
                        !airportCore.OutputBelt1.IsFull())
                    {
                        airportCore.RontgenMachine1.IsBusy = true;
                        Luggage nextLuggage = airportCore.InputBelt1.Luggage.Dequeue();
                        airportCore.RontgenMachine1.ActualLuggage = nextLuggage;

                        if (!Core.TurboMode)
                        {
                            airportCore.Events1.Enqueue($"LuggageClaim: Event RontgenStart for Luggage {nextLuggage.ID} planned at time: {Time:F2}");
                        }
                        RontgenStart rontgenStart = new RontgenStart(Core, Time, nextLuggage);
                        Core.PlanEvent(rontgenStart);
                    }
                }
                if (airportCore.LuggagePickUpQueue1.Queue.Count > 0 && Customer.ID != airportCore.LuggagePickUpQueue1.Queue.Peek().ID)
                {
                    Customer nextCustomer = airportCore.LuggagePickUpQueue1.Queue.Peek();

                    if (!Core.TurboMode)
                    {
                        airportCore.Events1.Enqueue($"LuggageClaim: Event LuggageClaim for Customer {nextCustomer.ID} planned at time: {Time:F2}");
                    }
                    LuggageClaim luggageClaim = new LuggageClaim(Core, Time, nextCustomer);
                    Core.PlanEvent(luggageClaim);
                }
            } 
            else
            {
                if (airportCore.OutputBelt2.Luggage.Queue.Count > 0 &&
                    airportCore.OutputBelt2.Luggage.Queue.Peek().Customer.ID == Customer.ID)
                {
                    Luggage luggage = airportCore.OutputBelt2.Luggage.Dequeue();

                    if (!Core.TurboMode)
                    {
                        airportCore.Logs2.Enqueue($"Customer {Customer.ID} claimed luggage {luggage.ID} at time {Time:F2}");
                    }
                    Customer.Luggage.Add(luggage);
                    if (Customer.HasAllLuggage())
                    {
                        airportCore.LuggagePickUpQueue2.Dequeue();
                        LeaveSystem(Core, Customer);
                        if (!Core.TurboMode)
                        {
                            airportCore.Logs2.Enqueue($"Customer {Customer.ID} left the system at time {Time:F2}");
                        }
                    }
                    else
                    {
                        if (!Core.TurboMode)
                        {
                            airportCore.Events2.Enqueue($"LuggageClaim: Event LuggageClaim for Customer {Customer.ID} planned at time: {Time:F2}");
                        }

                        LuggageClaim luggageClaim = new LuggageClaim(Core, Time, Customer);
                        Core.PlanEvent(luggageClaim);
                    }
                    if (!airportCore.RontgenMachine2.IsBusy &&
                        airportCore.InputBelt2.Luggage.Queue.Count > 0 &&
                        !airportCore.OutputBelt2.IsFull())
                    {
                        airportCore.RontgenMachine2.IsBusy = true;
                        Luggage nextLuggage = airportCore.InputBelt2.Luggage.Dequeue();
                        airportCore.RontgenMachine2.ActualLuggage = nextLuggage;

                        if (!Core.TurboMode)
                        {
                            airportCore.Events1.Enqueue($"LuggageClaim: Event RontgenStart for Luggage {nextLuggage.ID} planned at time: {Time:F2}");
                        }
                        RontgenStart rontgenStart = new RontgenStart(Core, Time, nextLuggage);
                        Core.PlanEvent(rontgenStart);
                    }
                }
                if (airportCore.LuggagePickUpQueue2.Queue.Count > 0 && Customer.ID != airportCore.LuggagePickUpQueue2.Queue.Peek().ID)
                {
                    Customer nextCustomer = airportCore.LuggagePickUpQueue2.Queue.Peek();

                    if (!Core.TurboMode)
                    {
                        airportCore.Events2.Enqueue($"LuggageClaim: Event LuggageClaim for Customer {nextCustomer.ID} planned at time: {Time:F2}");
                    }
                    LuggageClaim luggageClaim = new LuggageClaim(Core, Time, nextCustomer);
                    Core.PlanEvent(luggageClaim);
                }
            }
        }

        public static void LeaveSystem(EventCore core, Customer customer)
        {
            customer.LeaveTime = core.CurrentTime;
            customer.Status = Models.Enums.Status.CHECKED;
            ((AirportSimulation)core).TimeInSystemCustomers.AddValue(customer.GetTimeInSystem());
        }
    }
}
