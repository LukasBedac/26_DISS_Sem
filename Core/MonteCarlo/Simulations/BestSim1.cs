using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo.Simulations
{
    public class BestSim1 : Sim1
    {
        public List<int> SimulationResults { get; set; }
        public TimeOnly StartTime { get; set; }
        public double NumberOfCorrectArrivals { get; set; } = 0;

        public BestSim1(int replications, TimeOnly time) : base(replications)
        {
            StartTime = time;
            SimulationResults = new List<int>(replications);
        }

        public override void BeforeReplication()
        {
            ActualTime = StartTime;
        }

        public override void AfterReplication()
        {
            if (ActualTime <= TimeOnly.Parse(FINISHTIME))
            {
                SimulationResults.Add(1);
            }
            else
            {
                SimulationResults.Add(0);
            }
        }

        public override void BeforeSimulation()
        {
            base.BeforeSimulation();
            SimulationResults.Clear();
        }

        public override void AfterSimulation()
        {
            NumberOfCorrectArrivals = (double)SimulationResults.Sum() / Replications;
        }
    }
}
