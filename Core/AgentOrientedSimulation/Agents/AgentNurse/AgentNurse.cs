using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.IDGen;
using OSPABA;
using Simulation;

namespace Agents.AgentNurse
{
	//meta! id="14"
	public class AgentNurse : OSPABA.Agent
	{
        public List<Nurse> Nurses { get; set; } = new();
        public List<Nurse> FreeNurses { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> Queue { get; set; } = new();
        public AgentNurse(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
			
        }

		override public void PrepareReplication()
		{
            Nurses.Clear();
            FreeNurses.Clear();
            Queue.Clear();

            base.PrepareReplication();
            // Setup component for the next replication
            for (int i = 0; i < ((MySimulation)MySim).CountOfNurses; i++)
            {
				Nurse nurse = new Nurse(MySim);
                Nurses.Add(nurse);
				FreeNurses.Add(nurse);
            }
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerNurse(SimId.ManagerNurse, MySim, this);
			AddOwnMessage(Mc.RequestNurseAllocation);
			AddOwnMessage(Mc.FreeNurse);
		}
		//meta! tag="end"
	}
}