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
    public class Luggage : TimeEntity
    {
        public int ID { get; set; }
        public Customer Customer { get; set; }
        //public int BeltID { get; set; }
        public int SelectedLine { get; set; }
        public Status Status { get; set; }
        public Luggage(Customer customer) 
        {
            ID = IDGenerator.GetLuggageID();
            Customer = customer;     
        }
    }
}
