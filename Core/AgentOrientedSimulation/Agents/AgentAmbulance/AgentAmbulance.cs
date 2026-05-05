using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.IDGen;
using OSPABA;
using Simulation;

namespace Agents.AgentAmbulance
{
	//meta! id="13"
	public class AgentAmbulance : OSPABA.Agent
	{
        public List<AmbulanceA> AmbulancesA { get; set; } = new();
        public List<AmbulanceB> AmbulancesB { get; set; } = new();
        public List<AmbulanceA> FreeA { get; set; } = new();
        public List<AmbulanceB> FreeB { get; set; } = new();

        public PriorityQueue<MyMessage, (int, double)> QueueA { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> QueueB { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> QueueAny { get; set; } = new();
        public AgentAmbulance(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
            
        }

		override public void PrepareReplication()
		{
            AmbulancesA.Clear();
            AmbulancesB.Clear();
            FreeA.Clear();
            FreeB.Clear();
            QueueA.Clear();
            QueueB.Clear();
            QueueAny.Clear();

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