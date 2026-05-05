using OSPABA;
using Simulation;
using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Stats;

namespace Agents.AgentOfUrgent
{
	//meta! id="10"
	public class AgentOfUrgent : OSPABA.Agent
	{
        public PriorityQueue<Patient, (int, double)> AmbulancesInitialPatientQueue { get; set; } = new();
        public PriorityQueue<Patient, (int, double)> AmbulancesExamPatientQueue { get; set; } = new();

        public StatQueue<Patient> AmbulancesInitialStatistic { get; set; }
        public StatQueue<Patient> AmbulancesExamStatistic { get; set; }
        public AgentOfUrgent(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();

			AmbulancesInitialStatistic = new StatQueue<Patient>(MySim);
			AmbulancesExamStatistic = new StatQueue<Patient>(MySim);
        }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
            // Setup component for the next replication
            AmbulancesInitialPatientQueue.Clear();
            AmbulancesInitialStatistic.Clear();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerOfUrgent(SimId.ManagerOfUrgent, MySim, this);
			AddOwnMessage(Mc.RequestResource);
			AddOwnMessage(Mc.InitalExamination);
			AddOwnMessage(Mc.Transfer);
			AddOwnMessage(Mc.PatientExamination);
			AddOwnMessage(Mc.Examination);
		}
		//meta! tag="end"
	}
}