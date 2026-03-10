using Core.Distributions;
using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo
{
    public class BaseCore : SimulationCore
    {
        protected const string DECELERATIONTIME = "6:30";
        protected const string STARTTIME = "6:00";

        protected const string GREEN = "green";
        protected const string RED = "red";
        protected const string BLACK = "black";
        protected const string BLUE = "blue";

        protected double _path1 = 0.0;
        protected double _path2 = 0.0;
        protected double _path3 = 0.0;

        protected TimeOnly ActualTime { get; set; }
        protected DiscreteUniform RedPath { get; set; }
        protected ContinuousUniform GreePath { get; set; }
        protected Empiric BlackPath { get; set; }
        protected Empiric BluePath { get; set; }
        protected ContinuousUniform Slowness { get; set; }

        public double AverageTime { get; set; } = 0.0;
        public int GraphPercentCut { get; set; }
        public int GraphPoints { get; set; } = 0;
        public Action<int, double> OnUpdate { get; set; }


        public BaseCore(int replications)
        {
            
            Replications = replications;
        }

        public override void Run()
        {
            base.Run();
        }

        public override void BeforeSimulation()
        {
            CalculategraphPoints();
            RedPath = new DiscreteUniform(55, 76);
            GreePath = new ContinuousUniform(50, 80);
            BlackPath = new Empiric(false, new List<Scope>
            {
                new Scope(10, 20, 0.1),
                new Scope(20, 32, 0.5),
                new Scope(32, 45, 0.2),
                new Scope(45, 75, 0.15),
                new Scope(75, 85, 0.05)
            });
            BluePath = new Empiric(true, new List<Scope>
            {
                new Scope(15, 29, 0.2),
                new Scope(29, 45, 0.4),
                new Scope(45, 65, 0.4)
            });
            Slowness = new ContinuousUniform(10, 25);
        }

        public override void BeforeReplication()
        {
            ActualTime = TimeOnly.Parse(STARTTIME);
        }

        public override void AfterReplication()
        {
            if (ActualReplication > 0)
            {
                AverageTime = AverageTime + (ActualTime.ToTimeSpan().TotalSeconds - AverageTime) / ActualReplication;
                //Console.WriteLine($"Replication: {ActualReplicationCount}, Actual time: {ActualTime}, Average time: {TimeSpan.FromSeconds(AverageTime).ToString(@"h\:mm\:ss")}");
                //if (ActualReplication % 10_000 == 0)
                if (ActualReplication > ((Replications / 100) * GraphPercentCut) && ActualReplication % GraphPoints == 0)
                {
                    UpdateGUI();
                }
            }
        }

        public override void AfterSimulation()
        {
            //AverageTime = AverageTime + (ActualTime.ToTimeSpan().TotalSeconds - AverageTime) / ActualReplicationCount;
            Console.WriteLine($"Replication: {ActualReplication}, Average time: {TimeSpan.FromSeconds(AverageTime).ToString(@"h\:mm\:ss")}");
        }

        protected double CalculatePathTime(double km, string color, bool canBeSlowed = false)
        {
            switch (color)
            {
                case GREEN:
                    if (ActualTime >= TimeOnly.Parse(DECELERATIONTIME) && canBeSlowed)
                    {
                        return km / (GreePath.NextDouble() * ((100 - Slowness.NextDouble()) / 100));
                    }
                    else
                    {
                        return km / GreePath.NextDouble();
                    }
                case RED:
                    if (ActualTime >= TimeOnly.Parse(DECELERATIONTIME) && canBeSlowed)
                    {
                        return km / (RedPath.NextInt() * ((100 - Slowness.NextDouble()) / 100));
                    }
                    else
                    {
                        return km / RedPath.NextInt();
                    }
                case BLACK:
                    if (ActualTime >= TimeOnly.Parse(DECELERATIONTIME) && canBeSlowed)
                    {
                        return km / (BlackPath.NextDouble() * ((100 - Slowness.NextDouble()) / 100));
                    }
                    else
                    {
                        return km / BlackPath.NextDouble();
                    }
                case BLUE:
                    if (ActualTime >= TimeOnly.Parse(DECELERATIONTIME) && canBeSlowed)
                    {
                        return km / (BluePath.NextInt() * ((100 - Slowness.NextDouble()) / 100));
                    }
                    else
                    {
                        return km / BluePath.NextInt();
                    }
                default:
                    throw new Exception("Color not found");
            }
        }

        protected void CalculateBestPathTime()
        {
            double min = Math.Min(_path1, Math.Min(_path2, _path3));
            ActualTime = ActualTime.AddHours(min);
        }

        protected void ResetPaths()
        {
            _path1 = 9999.0;
            _path2 = 9999.0;
            _path3 = 9999.0;
        }

        public void UpdateGUI()
        {
            OnUpdate?.Invoke(ActualReplication, AverageTime);
        }

        private void CalculategraphPoints()
        {
            int repToSkip = (int)((Replications / 100) * GraphPercentCut);
            int replicationsToShow = Replications - repToSkip;
            GraphPoints = replicationsToShow / NUMBEROFPOINTS < 1 ? 1 : replicationsToShow / NUMBEROFPOINTS;
        }
    }
}
