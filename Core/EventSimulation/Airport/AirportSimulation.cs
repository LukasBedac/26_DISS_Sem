using Core.Distributions;
using Core.Distributions.Helpers;
using Core.EventSimulation.Airport.Events;
using Core.EventSimulation.Airport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport
{
    public class AirportSimulation : EventCore
    {
        #region Parameters
        public int InputBeltMaxCrates { get; set; }
        public int OutputBeltMaxCrates { get; set; }
        public Queue<Customer> ArrivalQueue1 { get; set; }
        public Queue<Customer> ArrivalQueue2 { get; set; }
        public Queue<Customer> MetalDetectorQueue1 { get; set; }
        public Queue<Customer> MetalDetectorQueue2 { get; set; }
        public Queue<Customer> CratePickUpQueue1 { get; set; }
        public Queue<Customer> CratePickUpQueue2 { get; set; }
        public Belt InputBelt1 { get; set; }
        public Belt InputBelt2 { get; set; }
        public RontgenMachine RontgenMachine1 { get; set; }
        public RontgenMachine RontgenMachine2 { get; set; }
        public Belt OutputBelt1 { get; set; }
        public Belt OutputBelt2 { get; set; }
        public MetalDetector MetalDetector1 { get; set; }
        public MetalDetector MetalDetector2 { get; set; }
        public Employee Employee1 { get; set; }
        public Employee Employee2 { get; set; }

        #endregion Parameters

        #region Generators
        public Exponential ArrivalGenerator { get; set; }
        public ContinuousUniform MetalDetectorDurationGenerator { get; set; }
        //public ContinuousUniform MetalDetectorGenerator2 { get; set; }
        public Random PersonalCheckGenerator { get; set; }
        public Triangular PersonalCheckDurationGenerator { get; set; }
        public ContinuousUniform RontgenMachineDurationGenerator { get; set; }
        //public ContinuousUniform RontgenMachineGenerator2 { get; set; }
        public Empiric NumberOfCratesPerPersonGenerator { get; set; }

        #endregion Generators

        #region Statistics

        #endregion Statistics
        
        public AirportSimulation(int replications, int inputBeltCrates, int outputBeltCrates) : base(replications)
        {
            InputBeltMaxCrates = inputBeltCrates;
            OutputBeltMaxCrates = outputBeltCrates;
        }

        public override void BeforeSimulation()
        {
            Arrival firstArrival = new Arrival(this, CurrentTime);
            Events.Enqueue(firstArrival, firstArrival.Time);

            SetParameters();
            SetGenerators();
        }
        private void SetParameters()
        {
            ArrivalQueue1 = new Queue<Customer>();
            ArrivalQueue2 = new Queue<Customer>();
            
            MetalDetectorQueue1 = new Queue<Customer>();
            MetalDetectorQueue2 = new Queue<Customer>();
            
            CratePickUpQueue1 = new Queue<Customer>();
            CratePickUpQueue2 = new Queue<Customer>();
            
            InputBelt1 = new Belt(InputBeltMaxCrates, this);
            InputBelt2 = new Belt(InputBeltMaxCrates, this);
            
            RontgenMachine1 = new RontgenMachine();
            RontgenMachine2 = new RontgenMachine();
            
            MetalDetector1 = new MetalDetector();
            MetalDetector2 = new MetalDetector();

            OutputBelt1 = new Belt(OutputBeltMaxCrates, this);
            OutputBelt2 = new Belt(OutputBeltMaxCrates, this);

            Employee1 = new Employee();
            Employee2 = new Employee();
        }

        private void SetGenerators()
        {
            ArrivalGenerator = new Exponential(50);
            MetalDetectorDurationGenerator = new ContinuousUniform(6, 27);

            RontgenMachineDurationGenerator = new ContinuousUniform(9, 46);
            Scope personalCheckScope = new Scope(10.0, 35.0, 120.0); 

            PersonalCheckDurationGenerator = new Triangular(personalCheckScope);
            PersonalCheckGenerator = new Random(SeedGen.GetSeed());

            List<Scope> crateNumberScopes = new List<Scope>
            {
                new Scope(0, 1, 0.15 ),
                new Scope(1, 2, 0.68 ),
                new Scope(2, 3, 0.17 )
            };
            NumberOfCratesPerPersonGenerator = new Empiric(true, crateNumberScopes);
        }
    }
}