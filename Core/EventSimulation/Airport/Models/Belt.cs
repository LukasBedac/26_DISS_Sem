using Core.EventSimulation.Models;
using Core.EventSimulation.Statistics;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models
{
    public class Belt : TimeEntity
    {
        private int _maxCrates;
        public int ID { get; set; }
        public int CrateID { get; set; }
        public StatQueue<Crate> Crates { get; set; }

        public Belt(int maxCrates, EventCore core) 
        {
            ID = IDGenerator.GetBeltID();
            _maxCrates = maxCrates;
            Crates = new StatQueue<Crate>(core);
            for (int i = 0; i < maxCrates; i++)
            {
                Crates.Enqueue(new Crate(ID));
            }
        }
    }
}
