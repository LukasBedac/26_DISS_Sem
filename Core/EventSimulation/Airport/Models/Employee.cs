using Core.EventSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models
{
    public class Employee : TimeEntity
    {
        public int ID { get; set; }
        public bool IsBusy { get; set; }
        public Customer ActualCustomer { get; set; }
    }
}
