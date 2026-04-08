using Core.EventSimulation.Airport.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models.DTO
{
    public class LuggageDTO
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int SelectedLine { get; set; }
        public Status Status { get; set; }

        public static LuggageDTO FromLuggage(Luggage luggage)
        {
            return new LuggageDTO
            {
                ID = luggage.ID,
                CustomerID = luggage.Customer.ID,
                SelectedLine = luggage.SelectedLine,
                Status = luggage.Status
            };
        }
    }
}
