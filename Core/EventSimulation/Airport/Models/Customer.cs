using Core.EventSimulation.Airport.Models.Enums;
using Core.EventSimulation.Models;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models
{
    public class Customer : TimeEntity
    {
        public int ID { get; set; }
        public int SelectedLine { get; set; }
        public int LuggageCount { get; set; }
        public Status Status { get; set; } = Status.ARRIVED;
        public List<Luggage> Luggage { get; set; } = new List<Luggage>();

        public Customer(int luggageCount)
        {
            ID = IDGenerator.GetCustomerID();

            LuggageCount = luggageCount;

            for (int i = 0; i < luggageCount; i++)
            {
                Luggage.Add(new Luggage(this));
            }
        }

        public bool HasAllLuggage()
        {
            return Luggage.Count == LuggageCount;
        }

        public void SetLuggageLine()
        {
            foreach (Luggage luggage in Luggage)
            {
                luggage.SelectedLine = SelectedLine;
            }
        }

        public double GetTimeInSystem()
        {
            return LeaveTime - ArrivalTime;
        }
    }
}
