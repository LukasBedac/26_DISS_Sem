using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class MetalDetectorEnd : Event
    {
        public Customer Customer { get; set; }
        public MetalDetectorEnd(EventCore core, double time, Customer customer) : base(core, time)
        {
            Customer = customer;
        }
        public override void Execute()
        {
            AirportSimulation airportCore = (AirportSimulation)Core;
            if (!Core.TurboMode)
            {
                if (Customer.SelectedLine == 1)
                {
                    airportCore.Logs1.Enqueue($"Customer {Customer.ID} finished metal detector at time {Time:F2}");
                }
                else
                {
                    airportCore.Logs2.Enqueue($"Customer {Customer.ID} finished metal detector at time {Time:F2}");
                }
            }
            if (airportCore.PersonalCheckGenerator.NextDouble() < 0.19)
            {
                Customer.Status = Models.Enums.Status.PERSONAL_CHECK;
                if (Customer.SelectedLine == 1)
                {
                    airportCore.Employee1.IsBusy = true;
                    airportCore.Employee1.ActualCustomer = Customer;
                    if (!Core.TurboMode)
                    {
                        airportCore.Events1.Enqueue($"Event PersonalCheck for Customer {Customer.ID} planned at time: {Time:F2}");
                    }
                }
                else
                {
                    airportCore.Employee2.IsBusy = true;
                    airportCore.Employee2.ActualCustomer = Customer;
                    if (!Core.TurboMode)
                    {
                        airportCore.Events2.Enqueue($"Event PersonalCheck for Customer {Customer.ID} planned at time: {Time:F2}");
                    }
                }

                PersonalCheckStart personalCheckStart = new PersonalCheckStart(Core, Time, Customer);
                Core.PlanEvent(personalCheckStart);
            }
            else
            {
                ClaimLuggage(Core, Customer);
                MetalDetectorStart(Core, Customer);
            }
        }

        public static void MetalDetectorStart(EventCore core, Customer customer)
        {
            AirportSimulation airportCore = (AirportSimulation)core;
            if (customer.SelectedLine == 1)
            {
                airportCore.MetalDetector1.ActualCustomer = null;
                if (airportCore.MetalDetectorQueue1.Queue.Count > 0)
                {
                    Customer nextCustomer = airportCore.MetalDetectorQueue1.Dequeue();
                    airportCore.MetalDetector1.ActualCustomer = nextCustomer;

                    if (!core.TurboMode)
                    {
                        airportCore.Events1.Enqueue($"MetalDetectorEnd: Event MetalDetectorStart for Customer {nextCustomer.ID} planned at time: {core.CurrentTime:F2}");
                    }
                    
                    MetalDetectorStart metalDetectorStart = new MetalDetectorStart(core, core.CurrentTime, nextCustomer);
                    core.PlanEvent(metalDetectorStart);
                }
                else
                {
                    airportCore.MetalDetector1.IsBusy = false;
                }
            }
            else
            {

                airportCore.MetalDetector2.ActualCustomer = null;
                if (airportCore.MetalDetectorQueue2.Queue.Count > 0)
                {
                    Customer nextCustomer = airportCore.MetalDetectorQueue2.Dequeue();
                    airportCore.MetalDetector2.ActualCustomer = nextCustomer;

                    if (!core.TurboMode)
                    {
                        airportCore.Events2.Enqueue($"MetalDetectorEnd: Event MetalDetectorStart for Customer {nextCustomer.ID} planned at time: {core.CurrentTime:F2}");
                    }
                    
                    MetalDetectorStart metalDetectorStart = new MetalDetectorStart(core, core.CurrentTime, nextCustomer);
                    core.PlanEvent(metalDetectorStart);
                }
                else
                {
                    airportCore.MetalDetector2.IsBusy = false;
                }
            }
        }

        public static void ClaimLuggage(EventCore core, Customer customer)
        {
            AirportSimulation airportCore = (AirportSimulation)core;

            if (customer.LuggageCount == 0)
            {
                LuggageClaim.LeaveSystem(core, customer);
                return;
            }

            if (customer.SelectedLine == 1)
            {
                if (airportCore.LuggagePickUpQueue1.Queue.Count < 1)
                {
                    airportCore.LuggagePickUpQueue1.Enqueue(customer);
                    customer.Status = Models.Enums.Status.LUGGAGE_PICKUP;

                    if (!core.TurboMode)
                    {
                        airportCore.Events1.Enqueue($"MetalDetectorEnd: Event LuggageClaim for Customer {customer.ID} planned at time: {core.CurrentTime:F2}");
                    }
                    
                    LuggageClaim luggageClaim = new LuggageClaim(core, core.CurrentTime, customer);
                    core.PlanEvent(luggageClaim);
                }
                else
                {
                    airportCore.LuggagePickUpQueue1.Enqueue(customer);
                }
            }
            else
            {
                if (airportCore.LuggagePickUpQueue2.Queue.Count < 1)
                {
                    airportCore.LuggagePickUpQueue2.Enqueue(customer);
                    customer.Status = Models.Enums.Status.LUGGAGE_PICKUP;

                    if (!core.TurboMode)
                    {
                        airportCore.Events2.Enqueue($"MetalDetectorEnd: Event LuggageClaim for Customer {customer.ID} planned at time: {core.CurrentTime:F2}");
                    }
                    
                    LuggageClaim luggageClaim = new LuggageClaim(core, core.CurrentTime, customer);
                    core.PlanEvent(luggageClaim);
                }
                else
                {
                    airportCore.LuggagePickUpQueue2.Enqueue(customer);

                }
            }
        }
    }
}
