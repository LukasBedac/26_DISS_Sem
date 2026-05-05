using OSPABA;
using Simulation;
using Agents.AgentTransfers.ContinualAssistants;
using Core.Distributions;

namespace Agents.AgentTransfers
{
	//meta! id="44"
	public class AgentTransfers : OSPABA.Agent
	{
		

		public AgentTransfers(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerTransfers(SimId.ManagerTransfers, MySim, this);
			new Transfer(SimId.Transfer, MySim, this);
			AddOwnMessage(Mc.Transfer);
		}
		//meta! tag="end"
	}
}