using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Stats;
using Core.EventSimulation.Statistics;
using OSPABA;
using Simulation;

namespace Agents.AgentOfUrgent
{
	//meta! id="10"
	public class AgentOfUrgent : OSPABA.Agent
	{
        public PriorityQueue<Patient, (int, double)> AmbulancesInitialPatientQueue { get; set; } = new();
        public PriorityQueue<Patient, (int, double)> AmbulancesExamPatientQueue { get; set; } = new();


        public Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient> PatientWalkInInitExamQueueStat { get; set; }
        public Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient> PatientAmbulanceInitExamQueueStat { get; set; }
        public Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient> PatientWalkInExaminationQueueStat { get; set; }
        public Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient> PatientAmbulanceExaminationQueueStat { get; set; }

        public Statistic InitialExamWaitWalkInStat { get; set; } = new();
        public Statistic InitialExamWaitAmbulanceStat { get; set; } = new();
        public Statistic ExaminationWaitWalkInStat { get; set; } = new();
        public Statistic ExaminationWaitAmbulanceStat { get; set; } = new();

        public AgentOfUrgent(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();

			PatientWalkInInitExamQueueStat = new Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient>(MySim);
			PatientAmbulanceInitExamQueueStat = new Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient>(MySim);
			PatientWalkInExaminationQueueStat = new Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient>(MySim);
			PatientAmbulanceExaminationQueueStat = new Core.AgentOrientedSimulation.Models.Stats.StatQueue<Patient>(MySim);
        }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
            // Setup component for the next replication
            AmbulancesInitialPatientQueue.Clear();
			AmbulancesExamPatientQueue.Clear();
			PatientWalkInInitExamQueueStat.Clear();
			PatientAmbulanceInitExamQueueStat.Clear();
			PatientWalkInExaminationQueueStat.Clear();
			PatientAmbulanceExaminationQueueStat.Clear();

			InitialExamWaitWalkInStat.ClearValues();
			InitialExamWaitAmbulanceStat.ClearValues();
			ExaminationWaitWalkInStat.ClearValues();
			ExaminationWaitAmbulanceStat.ClearValues();
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