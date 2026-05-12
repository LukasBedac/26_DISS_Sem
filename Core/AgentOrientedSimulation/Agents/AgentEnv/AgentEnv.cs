using OSPABA;
using Simulation;
using Agents.AgentEnv.ContinualAssistants;
using Core.EventSimulation.Statistics;

namespace Agents.AgentEnv
{
	//meta! id="2"
	public class AgentEnv : OSPABA.Agent
	{
        public Statistic ReplicationTimeInSystemWalk { get; set; } = new();
        public Statistic ReplicationTimeInSystemAmbulance { get; set; } = new();
        public int TotalPatientsWalk { get; set; } = 0;
        public int TotalPatientsAmbulance { get; set; } = 0;
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
            TotalPatientsWalk = 0;
            TotalPatientsAmbulance = 0;
            ReplicationTimeInSystemWalk.ClearValues();
            ReplicationTimeInSystemAmbulance.ClearValues();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEnv(SimId.ManagerEnv, MySim, this);
			new Schedulermbulance(SimId.Schedulermbulance, MySim, this);
			new SchedulerByFoot(SimId.SchedulerByFoot, MySim, this);
			AddOwnMessage(Mc.PatientDeparture);
		}
		//meta! tag="end"
	}
}