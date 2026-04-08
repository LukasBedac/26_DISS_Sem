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
    public class LuggageStorage : Event
    {
        public Customer Customer { get; set; }
        public LuggageStorage(EventCore core, double time, Customer customer) : base(core, time)
        {
            Customer = customer;
        }
        public override void Execute()
        {
            AirportSimulation airportCore = (AirportSimulation)Core;
            if (Customer.SelectedLine == 1)
            {
                Line1Setting(airportCore);
            } 
            else
            {
                Line2Setting(airportCore);
            }
        }
        public void Line1Setting(AirportSimulation airportCore)
        {
            if (airportCore.InputBelt1.IsFull())
            {
                //Wait
                return;
            } 
            else
            {
                if (Customer.Luggage.Count == 0)
                {
                    Line1SetDetectorAndLuggageStore(airportCore);
                }
                else
                {
                    Customer.Status = Status.LUGGAGE_STORE;
                    airportCore.InputBelt1.Luggage.Enqueue(Customer.Luggage.First());
                    if (!Core.TurboMode)
                    {
                        airportCore.Logs1.Enqueue($"Customer {Customer.ID} put luggage {Customer.Luggage.First().ID} at input belt at time {Time:F2}");
                    }
                    Customer.Luggage.RemoveAt(0);
                    if (!airportCore.OutputBelt1.IsFull() && !airportCore.RontgenMachine1.IsBusy)
                    {
                        airportCore.RontgenMachine1.IsBusy = true;
                        Luggage luggageToScan = airportCore.InputBelt1.Luggage.Dequeue();
                        airportCore.RontgenMachine1.ActualLuggage = luggageToScan;
                        if (!Core.TurboMode)
                        {
                            airportCore.Events1.Enqueue($"LuggageStorage: Event RontgenStart for Luggage {luggageToScan.ID} planned at time: {Time:F2}");
                        }
                        RontgenStart rontgenStart = new RontgenStart(Core, Time, luggageToScan);
                        Core.PlanEvent(rontgenStart);
                    }
                    
                    if (Customer.Luggage.Count > 0)
                    {
                        if (!Core.TurboMode)
                        {
                            airportCore.Events1.Enqueue($"LuggageStorage: Event LuggageStorage for Customer {Customer.ID} planned at time: {Time:F2}");
                        }
                        LuggageStorage newEvent = new LuggageStorage(Core, Time, Customer);
                        Core.PlanEvent(newEvent);
                    } 
                    else
                    {
                        Line1SetDetectorAndLuggageStore(airportCore);
                    }
                }
            }
        }

        public void Line2Setting(AirportSimulation airportCore)
        {
            if (airportCore.InputBelt2.IsFull())
            {
                //Wait
                return;
            }
            else
            {
                if (Customer.Luggage.Count == 0)
                {
                    Line2SetDetectorAndLuggageStore(airportCore);
                }
                else
                {
                    Customer.Status = Status.LUGGAGE_STORE;
                    airportCore.InputBelt2.Luggage.Enqueue(Customer.Luggage.First());
                    Customer.Luggage.First().Status = Status.IN_QUEUE;
                    if (!Core.TurboMode)
                    {
                        airportCore.Logs2.Enqueue($"Customer {Customer.ID} put luggage {Customer.Luggage.First().ID} at input belt at time {Time:F2}");
                    }
                    Customer.Luggage.RemoveAt(0);
                    if (!airportCore.OutputBelt2.IsFull() && !airportCore.RontgenMachine2.IsBusy)
                    {
                        airportCore.RontgenMachine2.IsBusy = true;
                        Luggage luggageToScan = airportCore.InputBelt2.Luggage.Dequeue();
                        airportCore.RontgenMachine2.ActualLuggage = luggageToScan ;
                        
                        if (!Core.TurboMode)
                        {
                            airportCore.Events2.Enqueue($"LuggageStorage: Event RontgenStart for Luggage {luggageToScan.ID} planned at time: {Time:F2}");
                        }
                        RontgenStart rontgenStart = new RontgenStart(Core, Time, luggageToScan);
                        Core.PlanEvent(rontgenStart);
                    }

                    if (Customer.Luggage.Count > 0)
                    {
                        if (!Core.TurboMode)
                        {
                            airportCore.Events2.Enqueue($"LuggageStorage: Event LuggageStorage for Customer {Customer.ID} planned at time: {Time:F2}");
                        }
                        LuggageStorage newEvent = new LuggageStorage(Core, Time, Customer);
                        Core.PlanEvent(newEvent);
                    } 
                    else
                    {
                        Line2SetDetectorAndLuggageStore(airportCore);
                    }
                }
            }
        }
        private void Line1SetDetectorAndLuggageStore(AirportSimulation airportCore)
        {
            if (airportCore.ArrivalQueue1.Queue.Count == 0 ||
                airportCore.ArrivalQueue1.Queue.Peek().ID != Customer.ID)
            {
                return;
            }

            if (!airportCore.MetalDetector1.IsBusy && airportCore.MetalDetectorQueue1.Queue.Count < 1)
            {
                airportCore.MetalDetector1.IsBusy = true;
                airportCore.MetalDetector1.ActualCustomer = Customer;
                airportCore.ArrivalQueue1.Dequeue();
                if (!Core.TurboMode)
                {
                    airportCore.Events1.Enqueue($"LuggageStorage: Event MetalDetector for Customer {Customer.ID} planned at time: {Time:F2}");
                }
                MetalDetectorStart newEvent = new MetalDetectorStart(Core, Time, Customer);
                Core.PlanEvent(newEvent);
            }
            else
            {
                airportCore.MetalDetectorQueue1.Enqueue(Customer);
                airportCore.ArrivalQueue1.Dequeue();
            }

            if (airportCore.ArrivalQueue1.Queue.Count > 0)
            {
                Customer nextCustomer = airportCore.ArrivalQueue1.Queue.Peek();

                if (!Core.TurboMode)
                {
                    airportCore.Events1.Enqueue($"LuggageStorage: Event LuggageStorage for Customer: {nextCustomer.ID} planned at time: {Time:F2}");
                }
                LuggageStorage newEvent = new LuggageStorage(Core, Time, nextCustomer);
                Core.PlanEvent(newEvent);
            }
        }

        private void Line2SetDetectorAndLuggageStore(AirportSimulation airportCore)
        {

            if (airportCore.ArrivalQueue2.Queue.Count == 0 ||
                airportCore.ArrivalQueue2.Queue.Peek().ID != Customer.ID)
            {
                return;
            }

            if (!airportCore.MetalDetector2.IsBusy && airportCore.MetalDetectorQueue2.Queue.Count < 1)
            {
                airportCore.MetalDetector2.IsBusy = true;
                airportCore.MetalDetector2.ActualCustomer = Customer;
                airportCore.ArrivalQueue2.Dequeue();

                if (!Core.TurboMode)
                {
                    airportCore.Events2.Enqueue($"LuggageStorage: Event MetalDetector for Customer {Customer.ID} planned at time: {Time:F2}");
                }
                MetalDetectorStart newEvent = new MetalDetectorStart(Core, Time, Customer);
                Core.PlanEvent(newEvent);
            }
            else
            {
                airportCore.MetalDetectorQueue2.Enqueue(Customer);
                airportCore.ArrivalQueue2.Dequeue();
            }

            if (airportCore.ArrivalQueue2.Queue.Count > 0)
            {
                Customer nextCustomer = airportCore.ArrivalQueue2.Queue.Peek();

                if (!Core.TurboMode)
                {
                    airportCore.Events2.Enqueue($"LuggageStorage: Event LuggageStorage for Customer: {nextCustomer.ID} planned at time: {Time:F2}");
                }
                LuggageStorage newEvent = new LuggageStorage(Core, Time, nextCustomer);
                Core.PlanEvent(newEvent);
            }
        }
    }
}