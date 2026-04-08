using Core.EventSimulation.Models;
using Core.EventSimulation.Statistics;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models
{
    public class Belt : TimeEntity
    {
        private int _maxLuggage;
        public int ID { get; set; }
        public StatQueue<Luggage> Luggage { get; set; }

        public Belt(int maxLuggage, EventCore core)
        {
            ID = IDGenerator.GetBeltID();
            _maxLuggage = maxLuggage;
            Luggage = new StatQueue<Luggage>(core, maxLuggage);
        }

        public bool IsFull()
        {
            return Luggage.Queue.Count >= _maxLuggage;
        }

        public List<Luggage> GetAllLuggage()
        {
            return Luggage.Queue.ToList();
        }
    }
}
