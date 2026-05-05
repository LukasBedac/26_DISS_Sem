using OSPABA;
using Simulation;
using Agents.AgentEnv.ContinualAssistants;
using Core.EventSimulation.Statistics;

namespace Agents.AgentEnv
{
	//meta! id="2"
	public class AgentEnv : OSPABA.Agent
	{
		public Statistic PatientTimeInSystem { get; set; } = new();
        public AgentEnv(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
			AddOwnMessage(Mc.PatientArrival);
        }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			PatientTimeInSystem.ClearValues();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEnv(SimId.ManagerEnv, MySim, this);
			new SchedulerByFoot(SimId.SchedulerByFoot, MySim, this);
			new Schedulermbulance(SimId.Schedulermbulance, MySim, this);
			AddOwnMessage(Mc.PatientDeparture);
		}
		//meta! tag="end"
	}
}