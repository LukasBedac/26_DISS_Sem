using Core.EventSimulation.Models;
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
        public List<int> CratesID { get; set; } = new List<int>();

    }
}
