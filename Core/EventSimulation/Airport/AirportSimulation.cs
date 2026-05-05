using Core.Distributions;
using Core.Distributions.Helpers;
using Core.EventSimulation.Airport.Events;
using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Statistics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport
{
    public class AirportSimulation : EventCore
    {
        #region Parameters
        private int _step = 10;
        private int _lastStep = 0;
        private const double _maxAverageTimeInSystem = 600.0; //10 minutes in seconds
        private const int _maxAverageArrivalQueueLength = 20;
        private const int _maxAveragePickUpQueueLength = 10;

        public int InputBeltMaxLuggage { get; set; }
        public int OutputBeltMaxLuggage { get; set; }
        public StatQueue<Customer> ArrivalQueue1 { get; set; }
        public StatQueue<Customer> ArrivalQueue2 { get; set; }
        public StatQueue<Customer> MetalDetectorQueue1 { get; set; }
        public StatQueue<Customer> MetalDetectorQueue2 { get; set; }
        public StatQueue<Customer> LuggagePickUpQueue1 { get; set; }
        public StatQueue<Customer> LuggagePickUpQueue2 { get; set; }
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
        public Queue<string> Logs1 { get; set; }
        public Queue<string> Logs2 { get; set; }
        public Queue<string> Events1 { get; set; }
        public Queue<string> Events2 { get; set; }
        public int AllCustomersNumber { get; set; } = 0;
        public List<double> ArrivalQueueReplicationValues { get; set; } = new();
        public bool IsGraphExperiment { get; set; }
        public double ArrivalRate { get; set; } = 0.0;

        public double TimeMultiplier { get; set; } = 1.0;
        public bool IsExperiment { get; set; } = false;
        public double StartingCustomersNumber { get; set; } = 5000.0;

        #endregion Parameters

        #region Generators
        public Exponential ArrivalGenerator { get; set; }
        public ContinuousUniform MetalDetectorDurationGenerator { get; set; }
        //public ContinuousUniform MetalDetectorGenerator2 { get; set; }
        public Random PersonalCheckGenerator { get; set; }
        public Triangular PersonalCheckDurationGenerator { get; set; }
        public ContinuousUniform RontgenMachineDurationGenerator { get; set; }
        //public ContinuousUniform RontgenMachineGenerator2 { get; set; }
        public Empiric NumberOfLuggagePerPersonGenerator { get; set; }
        public Random QueueDecisionGenerator { get; set; }

        #endregion Generators

        #region Statistics
        public Statistic TimeInSystemCustomers { get; set; }
        public Statistic TimeInSystemLuggage { get; set; }
        public Statistic AverageWaitingTimeToStoreLuggage { get; set; }
        public Statistic AverageWaitingTimeDetector { get; set; }
        public Statistic AverageWaitingTimeRontgen { get; set; }
        public Statistic ArrivalQueueStat { get; set; } = new Statistic();
        public Statistic MetalDetectorQueueStat { get; set; } = new Statistic();
        public Statistic LuggagePickUpQueueStat { get; set; } = new Statistic();
        public Statistic InputBeltStat { get; set; } = new Statistic();
        public Statistic OutputBeltStat { get; set; } = new Statistic();
        public Statistic AverageCustomersStat { get; set; } = new Statistic();
        public Statistic TimeInSystemCustomersGlobal { get; set; } = new Statistic();
        public Statistic ArrivalQueueStatGlobal { get; set; } = new Statistic();
        public Statistic MetalDetectorQueueStatGlobal { get; set; } = new Statistic();
        public Statistic LuggagePickUpQueueStatGlobal { get; set; } = new Statistic();
        public Statistic InputBeltStatGlobal { get; set; } = new Statistic();
        public Statistic OutputBeltStatGlobal { get; set; } = new Statistic();
        public Statistic AverageCustomersStatGlobal { get; set; } = new Statistic();
        #endregion Statistics

        public AirportSimulation(int replications, int inputBeltLuggage, int outputBeltLuggage, int sleepMs, int sleepInterval) : base(replications)
        {
            Replications = replications;
            InputBeltMaxLuggage = inputBeltLuggage;
            OutputBeltMaxLuggage = outputBeltLuggage;
            SleepMs = sleepMs;
            SleepInterval = sleepInterval;
        }

        public override void BeforeSimulation()
        {
            SetParameters();
            SetGenerators();
            SetStatistics();
        }

        public override void AfterSimulation()
        {
            if (IsExperiment)
            {
                ExperimentSettings();
                WriteToFile();
                ResetGlobalStats();
            }
        }

        private void ResetGlobalStats()
        {
            TimeInSystemCustomersGlobal = new Statistic();
            ArrivalQueueStatGlobal = new Statistic();
            MetalDetectorQueueStatGlobal = new Statistic();
            LuggagePickUpQueueStatGlobal = new Statistic();
            InputBeltStatGlobal = new Statistic();
            OutputBeltStatGlobal = new Statistic();
            AverageCustomersStatGlobal = new Statistic();
        }

        private void SetStatistics()
        {
            TimeInSystemCustomers = new Statistic();
            TimeInSystemLuggage = new Statistic();
            AverageWaitingTimeToStoreLuggage = new Statistic();
            AverageWaitingTimeDetector = new Statistic();
            AverageWaitingTimeRontgen = new Statistic();

            AllCustomersNumber = 0;
            AverageCustomersStat = new Statistic();
            ArrivalQueueStat = new Statistic();
            MetalDetectorQueueStat = new Statistic();
            LuggagePickUpQueueStat = new Statistic();
            InputBeltStat = new Statistic();
            OutputBeltStat = new Statistic();
            AverageCustomersStat = new Statistic();
        }

        private void SetParameters()
        {
            ArrivalQueue1 = new StatQueue<Customer>(this);
            ArrivalQueue2 = new StatQueue<Customer>(this);
            
            MetalDetectorQueue1 = new StatQueue<Customer>(this);
            MetalDetectorQueue2 = new StatQueue<Customer>(this);
            
            LuggagePickUpQueue1 = new StatQueue<Customer>(this);
            LuggagePickUpQueue2 = new StatQueue<Customer>(this);
            
            InputBelt1 = new Belt(InputBeltMaxLuggage, this);
            InputBelt2 = new Belt(InputBeltMaxLuggage, this);
            
            RontgenMachine1 = new RontgenMachine();
            RontgenMachine2 = new RontgenMachine();
            
            MetalDetector1 = new MetalDetector();
            MetalDetector2 = new MetalDetector();

            OutputBelt1 = new Belt(OutputBeltMaxLuggage, this);
            OutputBelt2 = new Belt(OutputBeltMaxLuggage, this);

            Employee1 = new Employee();
            Employee2 = new Employee();

            Logs1 = new Queue<string>();
            Logs2 = new Queue<string>();

            Events1 = new Queue<string>();
            Events2 = new Queue<string>();
        }

        private void SetGenerators()
        {
            if (IsGraphExperiment)
            {
                ArrivalGenerator = new Exponential(ArrivalRate);
            }
            else if (IsExperiment)
            {
                double inputStream = StartingCustomersNumber / 86_400.0;
                ArrivalGenerator = new Exponential(inputStream);
            }
            else
            {
                ArrivalGenerator = new Exponential(StartingCustomersNumber / 86_400.0);
            }

            MetalDetectorDurationGenerator = new ContinuousUniform(6, 27);

            RontgenMachineDurationGenerator = new ContinuousUniform(9, 46);
            Scope personalCheckScope = new Scope(10.0, 35.0, 120.0); 

            PersonalCheckDurationGenerator = new Triangular(personalCheckScope);
            PersonalCheckGenerator = new Random(SeedGen.GetSeed());

            List<Scope> luggageNumberScopes = new List<Scope>
            {
                new Scope(0, 1, 0.15 ),
                new Scope(1, 2, 0.68 ),
                new Scope(2, 3, 0.17 )
            };
            NumberOfLuggagePerPersonGenerator = new Empiric(true, luggageNumberScopes);

            QueueDecisionGenerator = new Random(SeedGen.GetSeed());
        }

        public override void AfterReplication()
        {
            ArrivalQueueStat.AddValue(ArrivalQueue1.AverageQueueLength + ArrivalQueue2.AverageQueueLength);
            MetalDetectorQueueStat.AddValue(MetalDetectorQueue1.AverageQueueLength + MetalDetectorQueue2.AverageQueueLength);
            LuggagePickUpQueueStat.AddValue(LuggagePickUpQueue1.AverageQueueLength + LuggagePickUpQueue2.AverageQueueLength);
            InputBeltStat.AddValue(InputBelt1.Luggage.AverageQueueLength + InputBelt2.Luggage.AverageQueueLength);
            OutputBeltStat.AddValue(OutputBelt1.Luggage.AverageQueueLength + OutputBelt2.Luggage.AverageQueueLength);
            AverageCustomersStat.AddValue(AllCustomersNumber);

            TimeInSystemCustomersGlobal.AddValue(TimeInSystemCustomers.Mean);
            ArrivalQueueStatGlobal.AddValue(ArrivalQueueStat.Mean);
            MetalDetectorQueueStatGlobal.AddValue(MetalDetectorQueueStat.Mean);
            LuggagePickUpQueueStatGlobal.AddValue(LuggagePickUpQueueStat.Mean);
            InputBeltStatGlobal.AddValue(InputBeltStat.Mean);
            OutputBeltStatGlobal.AddValue(OutputBeltStat.Mean);
            AverageCustomersStatGlobal.AddValue(AllCustomersNumber);

            RefreshGUI();
        }
            
        public override void BeforeReplication()
        {
            CurrentTime = 0.0;
            Events.Clear();
            AllCustomersNumber = 0;
            SetParameters();
            SetStatistics();

            if (!TurboMode && !IsExperiment)
            {
                StartWatch();
            }

            PlanEvent(new WarmUpEndEvent(this, WarmUpLength));

            Arrival firstArrival = new Arrival(this, CurrentTime);
            PlanEvent(firstArrival);
        }

        public void ExperimentSettings()
        {
            double differenceTimeInSystem = TimeInSystemCustomersGlobal.Mean - _maxAverageTimeInSystem;
            double differenceArrivalQueue = ArrivalQueueStatGlobal.Mean - _maxAverageArrivalQueueLength;
            double differencePickUpQueue = LuggagePickUpQueueStatGlobal.Mean - _maxAveragePickUpQueueLength;

            bool allnegative = differenceTimeInSystem < 0.0 && differenceArrivalQueue < 0.0 && differencePickUpQueue < 0.0;

            int direction = allnegative ? 1 : -1;

            if (_lastStep != 0 && Math.Sign(_lastStep) != direction)
            {
                _step = Math.Max(1, _step / 2);
            }

            _lastStep = direction * _step;
            StartingCustomersNumber += _lastStep;

            //if (differenceTimeInSystem < 0.0 &&
            //    differenceArrivalQueue < 0.0 &&
            //    differencePickUpQueue < 0.0)
            //{
            //    if ((Math.Abs(differenceTimeInSystem) > 200.0 ||
            //    Math.Abs(differenceArrivalQueue) > 5.0))
            //    {
            //        _step = 100;
            //    }
            //    else if ((Math.Abs(differenceTimeInSystem) > 100.0 ||
            //    Math.Abs(differenceArrivalQueue) > 3.0))
            //    {
            //        _step = 10;
            //    }
            //    else
            //    {
            //        _step = 1;
            //    }
            //}
            //else
            //{
            //    if ((Math.Abs(differenceTimeInSystem) < 200.0 ||
            //    Math.Abs(differenceArrivalQueue) < 5.0))
            //    {
            //        _step = -100;
            //    }
            //    else if ((Math.Abs(differenceTimeInSystem) < 100.0 ||
            //    Math.Abs(differenceArrivalQueue) < 3.0))
            //    {
            //        _step = -10;
            //    }
            //    else
            //    {
            //        _step = -1;
            //    }
            //}

            //if (TimeInSystemCustomers.Mean < _maxAverageTimeInSystem ||
            //    MetalDetectorQueueStatGlobal.Mean < _maxAverageArrivalQueueLength ||
            //    LuggagePickUpQueueStatGlobal.Mean < _maxAveragePickUpQueueLength)
            //{
            //    StartingCustomersNumber += _step;
            //}
            //else
            //{
            //    StartingCustomersNumber -= _step;
            //}
        }

        public void WriteToFile()
        {
            string path = $"AirportSimulation_Exp{Replications}.csv";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                //writer.WriteLine($"Customers;Average Time in System; Average Metal Detector Queue length; Average PickUp Queue length");
                writer.WriteLine($"{AverageCustomersStatGlobal.Mean};{TimeInSystemCustomersGlobal.Mean};{ArrivalQueueStatGlobal.Mean};{LuggagePickUpQueueStatGlobal.Mean}");
            }
        }

        public void EndWarmUp()
        {
            SetStatistics();

            ArrivalQueue1.Clear();
            ArrivalQueue2.Clear();
            MetalDetectorQueue1.Clear();
            MetalDetectorQueue2.Clear();

            AllCustomersNumber = 0;
            ArrivalQueueStat = new Statistic();
            MetalDetectorQueueStat = new Statistic();
            LuggagePickUpQueueStat = new Statistic();
            InputBeltStat = new Statistic();
            OutputBeltStat = new Statistic();

        }
    }
}