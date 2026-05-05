using OSPABA;
using Simulation;
using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Stats;

namespace Agents.AgentHospital
{
	//meta! id="1"
	public class AgentHospital : OSPABA.Agent
	{
        public AgentHospital(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerHospital(SimId.ManagerHospital, MySim, this);
			AddOwnMessage(Mc.PatientArrival);
			AddOwnMessage(Mc.PatientExamination);
		}
		//meta! tag="end"
	}
}