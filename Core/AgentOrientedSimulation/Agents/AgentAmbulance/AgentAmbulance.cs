using OSPABA;
using Simulation;
using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.IDGen;
using Core.EventSimulation.Statistics;

namespace Agents.AgentAmbulance
{
	//meta! id="13"
	public class AgentAmbulance : OSPABA.Agent
	{
        public List<AmbulanceA> AmbulancesA { get; set; } = new();
        public List<AmbulanceB> AmbulancesB { get; set; } = new();

        public PriorityQueue<MyMessage, (int, double)> QueueA { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> QueueB { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> QueueAny { get; set; } = new();
        public WeightedStatistic BusyAmbulanceAStat { get; set; } = new();
        public WeightedStatistic BusyAmbulanceBStat { get; set; } = new();
        private double _lastUpdateA = 0.0;
        private double _lastUpdateB = 0.0;
        public AgentAmbulance(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
        }

		override public void PrepareReplication()
		{
            AmbulancesA.Clear();
            AmbulancesB.Clear();
            QueueA.Clear();
            QueueB.Clear();
            QueueAny.Clear();
            BusyAmbulanceAStat.ClearValues();
            BusyAmbulanceBStat.ClearValues();
            _lastUpdateA = 0.0;
            _lastUpdateB = 0.0;

            base.PrepareReplication();
            // Setup component for the next replication
            for (int i = 0; i < ((MySimulation)MySim).CountOfAmbulancesA; i++)
            {
                AmbulanceA ambulanceA = new AmbulanceA(MySim);
                AmbulancesA.Add(ambulanceA);
            }
            for (int i = 0; i < ((MySimulation)MySim).CountOfAmbulancesB; i++)
            {
                AmbulanceB ambulanceB = new AmbulanceB(MySim);
                AmbulancesB.Add(ambulanceB);
            }
        }

        public void RecordStatA()
        {
            double busyCount = ((MySimulation)MySim).CountOfAmbulancesA - AmbulancesA.Count;
            double timeDelta = MySim.CurrentTime - _lastUpdateA;

            if (timeDelta > 0)
            {
                BusyAmbulanceAStat.AddValue(busyCount, timeDelta);
            }
            _lastUpdateA = MySim.CurrentTime;
        }

        public void RecordStatB()
        {
            double busyCount = ((MySimulation)MySim).CountOfAmbulancesB - AmbulancesB.Count;
            double timeDelta = MySim.CurrentTime - _lastUpdateB;

            if (timeDelta > 0)
            {
                BusyAmbulanceBStat.AddValue(busyCount, timeDelta);
            }
            _lastUpdateB = MySim.CurrentTime;
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
		{
			new ManagerAmbulance(SimId.ManagerAmbulance, MySim, this);
			AddOwnMessage(Mc.FreeAmbulance);
			AddOwnMessage(Mc.RequestAmbulanceAllocationB);
			AddOwnMessage(Mc.RequestAmbulanceAllocationA);
		}
		//meta! tag="end"
	}
}