using OSPABA;
using Simulation;

namespace Agents.AgentSources
{
	//meta! id="65"
	public class AgentSources : OSPABA.Agent
	{
		public AgentSources(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerSources(SimId.ManagerSources, MySim, this);
			AddOwnMessage(Mc.FreeResource);
			AddOwnMessage(Mc.RequestNurseAllocation);
			AddOwnMessage(Mc.RequestResource);
			AddOwnMessage(Mc.RequestDoctorAllocation);
			AddOwnMessage(Mc.RequestAmbulanceAllocationB);
			AddOwnMessage(Mc.RequestAmbulanceAllocationA);
		}
		//meta! tag="end"
	}
}