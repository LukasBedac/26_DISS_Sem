using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Airport.Models.Enums;
using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Events
{
    public class Arrival : Event
    {
        private Customer _customer;
        public Arrival(EventCore core, double time) : base(core, time)
        {
            int luggageCount = ((AirportSimulation)core).NumberOfLuggagePerPersonGenerator.NextInt();
            ((AirportSimulation)core).AllCustomersNumber++;
            //((AirportSimulation)core).AllLuggageNumber += luggageCount;
            _customer = new Customer(luggageCount);
            _customer.ArrivalTime = time;
        }
        public override void Execute()
        {
            AirportSimulation airportCore = (AirportSimulation)Core;

            double newArrivalTime = Core.CurrentTime + airportCore.ArrivalGenerator.NextDouble();
            if (!Core.TurboMode) { 
                airportCore.Events1.Enqueue($"Arrival: Event Arrival planned at time: {newArrivalTime:F2}");
                airportCore.Events2.Enqueue($"Arrival: Event Arrival planned at time: {newArrivalTime:F2}");
            }
            Arrival newArrival = new Arrival(Core, newArrivalTime);
            Core.PlanEvent(newArrival);


            _customer.Status = Status.IN_QUEUE;
            if (airportCore.ArrivalQueue1.Queue.Count < airportCore.ArrivalQueue2.Queue.Count)
            {
                _customer.SelectedLine = 1;
                _customer.SetLuggageLine();
                if (!Core.TurboMode)
                {
                    airportCore.Logs1.Enqueue($"Customer {_customer.ID} arrived at time {Time:F2} with {_customer.LuggageCount} luggage(s).");
                }
                if (airportCore.ArrivalQueue1.Queue.Count == 0)
                {
                    airportCore.ArrivalQueue1.Enqueue(_customer);
                    SetLuggageAndEvent();
                    return;
                } else
                {
                    airportCore.ArrivalQueue1.Enqueue(_customer);
                }

            }
            else if (airportCore.ArrivalQueue1.Queue.Count > airportCore.ArrivalQueue2.Queue.Count)
            {
                _customer.SelectedLine = 2;
                _customer.SetLuggageLine();
                if (!Core.TurboMode)
                {
                    airportCore.Logs2.Enqueue($"Customer {_customer.ID} arrived at time {Time:F2} with {_customer.LuggageCount} luggage(s).");
                }
                if (airportCore.ArrivalQueue2.Queue.Count == 0)
                {
                    airportCore.ArrivalQueue2.Enqueue(_customer);
                    SetLuggageAndEvent();
                    return;
                } else
                {
                    airportCore.ArrivalQueue2.Enqueue(_customer);
                }

            } 
            else
            {
                double queueDecision = airportCore.QueueDecisionGenerator.NextDouble();
                if (queueDecision < 0.5)
                {
                    _customer.SelectedLine = 1;
                    _customer.SetLuggageLine();
                    if (!Core.TurboMode)
                    {
                        airportCore.Logs1.Enqueue($"Customer {_customer.ID} arrived at time {Time:F2} with {_customer.LuggageCount} luggage(s).");
                    }
                    if (airportCore.ArrivalQueue1.Queue.Count == 0)
                    {
                        airportCore.ArrivalQueue1.Enqueue(_customer);
                        SetLuggageAndEvent();
                        return;
                    }
                    else
                    {
                        airportCore.ArrivalQueue1.Enqueue(_customer);
                    }

                } else
                {
                    _customer.SelectedLine = 2;
                    _customer.SetLuggageLine();
                    if (!Core.TurboMode)
                    {
                        airportCore.Logs2.Enqueue($"Customer {_customer.ID} arrived at time {Time:F2} with {_customer.LuggageCount} luggage(s).");
                    }
                    if (airportCore.ArrivalQueue2.Queue.Count == 0)
                    {
                        airportCore.ArrivalQueue2.Enqueue(_customer);
                        SetLuggageAndEvent();
                        return;
                    }
                    else
                    {
                        airportCore.ArrivalQueue2.Enqueue(_customer);
                    }
                }
            }
        }
        public void SetLuggageAndEvent()
        {
            if (!Core.TurboMode)
            {
                if (_customer.SelectedLine == 1)
                {
                    ((AirportSimulation)Core).Events1.Enqueue($"Arrival: Event LuggageStorage for Customer {_customer.ID} planned at time: {Time:F2}");
                }
                else
                {
                    ((AirportSimulation)Core).Events2.Enqueue($"Arrival: Event LuggageStorage for Customer {_customer.ID} planned at time: {Time:F2}");
                }
            }
            _customer.Status = Status.LUGGAGE_STORE;
            LuggageStorage luggageStorage = new LuggageStorage(Core, Time, _customer);
            Core.PlanEvent(luggageStorage);
        }
    }
}
