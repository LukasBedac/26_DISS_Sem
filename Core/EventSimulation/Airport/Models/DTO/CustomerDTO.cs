using Core.EventSimulation.Airport.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models.DTO
{
    public class CustomerDTO
    {
        public int ID { get; set; }
        public int SelectedLine { get; set; }
        public int LuggageCount { get; set; }
        public Status Status { get; set; }
        public string LuggageIDs { get; set; }
        public string Arrivaltime { get; set; }

        public static CustomerDTO FromCustomer(Customer customer)
        {
            CustomerDTO dto = new CustomerDTO
            {
                ID = customer.ID,
                SelectedLine = customer.SelectedLine,
                LuggageCount = customer.LuggageCount,
                Status = customer.Status,
                Arrivaltime = string.Format($"{customer.ArrivalTime:F3}")

            };
            customer.Luggage.ForEach(x => dto.LuggageIDs += x.ID + ",");
            return dto;
        }
    }
}
