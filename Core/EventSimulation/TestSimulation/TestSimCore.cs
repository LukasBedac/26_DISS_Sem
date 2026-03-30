using Core.Distributions;
using Core.Distributions.Helpers;
using Core.EventSimulation.Models;
using Core.EventSimulation.Statistics;
using Core.EventSimulation.TestSimulation.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.TestSimulation
{
    public class TestSimCore : EventCore
    {
        //private const double SIMULATION_START_TIME = 32400; // 9:00 in seconds
        //private const double SIMULATION_END_TIME = 75600; // 21:00 in seconds

        private const double SIMULATION_START_TIME = 0;
        private const double SIMULATION_END_TIME = 1_000_000_000;

        //Generators
        public Exponential ArrivalGenerator { get; set; }
        public Triangular ServiceTimegenerator { get; set; }


        //Fields
        public TimeEntity CurrentCustomer { get; set; }
        public StatQueue<Car> ServiceQueue { get; set; }
        public bool IsBusy { get; set; }

        // Statistics
        public Statistic WaitTimeStat { get; set; }

        public TestSimCore(int replications) : base(replications)
        {
            
        }
        public override void BeforeSimulation()
        {
            CurrentTime = SIMULATION_START_TIME;
            EndTime = SIMULATION_END_TIME;
            ArrivalGenerator = new Exponential(8);
            ServiceTimegenerator = new Triangular(new Scope(3.0, 5.0, 10.0));

            WaitTimeStat = new Statistic();
            ServiceQueue = new StatQueue<Car>(this);
            IsBusy = false;

            Event firstArrival = new Arrival(this, CurrentTime);
            Events.Enqueue(firstArrival, firstArrival.Time);

            Calculate();
        }

        public override void AfterSimulation()
        {
            Console.WriteLine($"=== Simulation results ===");
            Console.WriteLine($"Average wait time: {WaitTimeStat.Mean:F5} min");
            Console.WriteLine($"Queue length: {ServiceQueue.AverageQueueLength:F5} cars");
        }
        /// <summary>
        /// AI Generated code - 1 Exaktny vypocet vysledkov replikacie
        /// </summary>
        public static void Calculate()
        {
            // ===== INPUT =====

            // Triangular parameters (minutes)
            double a = 3;   // min
            double c = 5;   // mode
            double b = 10;  // max

            // Arrival rate (cars per hour)
            double lambda = 1.0/8.0;

            // ===== SERVICE STATS =====

            // Mean service time E[S]
            double ES = (a + b + c) / 3.0;

            // Second moment E[S^2]
            double ES2 = (a * a + b * b + c * c - a * b - a * c - b * c) / 18.0 + (ES * ES);

            // Service rate μ (per minute)
            //double mu = 1.0 / ES;

            // ===== UTILIZATION =====

            double rho = lambda * ES;

            //if (rho >= 1)
            //{
            //    Console.WriteLine("System is unstable (ρ >= 1)");
            //    return;
            //}

            // ===== POLLACZEK–KHINCHINE =====

            // Average waiting time in queue
            double Wq = (lambda * ES2) / (2 * (1 - rho));

            // Average number of customers in queue
            double Lq = lambda * Wq;

            // ===== OUTPUT =====

            Console.WriteLine("=== M/G/1 Results ===");
            Console.WriteLine($"E[S] (mean service time): {ES:F5} min");
            Console.WriteLine($"E[S^2]: {ES2:F5}");
            Console.WriteLine($"Lambda: {lambda:F4} per min");
            //Console.WriteLine($"μ: {mu:F4} per min");
            //Console.WriteLine($"ρ (utilization): {rho:F2}");

            Console.WriteLine();
            Console.WriteLine($"Wq (avg waiting time): {Wq:F5} min");
            Console.WriteLine($"Lq (avg queue length): {Lq:F5} cars");
        }
    }
}
