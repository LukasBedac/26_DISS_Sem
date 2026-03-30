using Core.EventSimulation.Models;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models
{
    public class Crate : TimeEntity
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int BeltID { get; set; }

        public Crate(int beltID) 
        {
            ID = IDGenerator.GetCrateID();
            BeltID = beltID;
        }
    }
}
