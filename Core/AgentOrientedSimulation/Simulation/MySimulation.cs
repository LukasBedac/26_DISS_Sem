using Agents.AgentAmbulance;
using Agents.AgentDoctor;
using Agents.AgentEnv;
using Agents.AgentExaminations;
using Agents.AgentHospital;
using Agents.AgentNurse;
using Agents.AgentOfUrgent;
using Agents.AgentSources;
using Agents.AgentTransfers;
using Core.AgentOrientedSimulation.Models.IDGen;
using Core.EventSimulation.Statistics;
using OSPABA;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{

        public Statistic GlobalPatientsAll { get; set; } = new();
        public Statistic GlobalPatientsWalkIn { get; set; } = new();
        public Statistic GlobalPatientsAmbulance { get; set; } = new();
        public Statistic GlobalTimeInSystemWalk { get; set; } = new();
        public Statistic GlobalTimeInSystemAmbulance { get; set; } = new();

        public Statistic GlobalUtilizationNurses { get; set; } = new();
        public Statistic GlobalUtilizationDoctors { get; set; } = new();
        public Statistic GlobalUtilizationAmbulanceA { get; set; } = new();
        public Statistic GlobalUtilizationAmbulanceB { get; set; } = new();

		public Statistic GlobalInitialExamWaitWalkIn { get; set; } = new();
        public Statistic GlobalInitialExamWaitAmbulance { get; set; } = new();
        public Statistic GlobalExaminationWaitWalkIn { get; set; } = new();
        public Statistic GlobalExaminationWaitAmbulance { get; set; } = new();
        
		public Statistic GlobalInitialExamQueueLength { get; set; } = new();
        public Statistic GlobalExaminationQueueLength { get; set; } = new();
        
		public Statistic GlobalInitialExamWalkInQueueStat { get; set; } = new();
        public Statistic GlobalInitialExamAmbulanceQueueStat { get; set; } = new();
        public Statistic GlobalExaminationWalkInQueueStat { get; set; } = new();
        public Statistic GlobalExaminationAmbulanceQueueStat { get; set; } = new();
        public Statistic GlobalInitialExamAvgQueueStat { get; set; } = new();
        public Statistic GlobalExamAvgQueueStat { get; set; } = new();

        public int CountOfNurses { get; set; }
        public int CountOfDoctors { get; set; }
		public int CountOfAmbulancesA { get; set; } = 5;
		public int CountOfAmbulancesB { get; set; } = 7;
		public bool IsWarmUpDone { get; set; } = false;
		public bool IsTurboMode { get; set; } = false;
		public double WarmUpTime { get; set; } = 0;
		public int Variant { get; set; } = 1;

        public event System.Action AnimatorCreated;
        public event System.Action AnimatorRemoved;
        public System.Action OnReplicationFinishedAction { get; set; }
        public MySimulation()
		{
            Init();
		}

        override public void PrepareSimulation()
        {
            base.PrepareSimulation();
            ClearStats();

            // Create global statistcis
        }

        private void ClearStats()
        {
            GlobalPatientsAll.ClearValues();
            GlobalPatientsWalkIn.ClearValues();
            GlobalPatientsAmbulance.ClearValues();

            GlobalTimeInSystemWalk.ClearValues();
            GlobalTimeInSystemAmbulance.ClearValues();
            
			GlobalUtilizationNurses.ClearValues();
            GlobalUtilizationDoctors.ClearValues();
            GlobalUtilizationAmbulanceA.ClearValues();
            GlobalUtilizationAmbulanceB.ClearValues();
            
			GlobalInitialExamWaitWalkIn.ClearValues();
            GlobalInitialExamWaitAmbulance.ClearValues();
			GlobalExaminationWaitWalkIn.ClearValues();
            GlobalExaminationWaitAmbulance.ClearValues();
            
			GlobalInitialExamQueueLength.ClearValues();
            GlobalExaminationQueueLength.ClearValues();
            
			GlobalInitialExamWalkInQueueStat.ClearValues();
            GlobalInitialExamAmbulanceQueueStat.ClearValues();
			GlobalInitialExamAvgQueueStat.ClearValues();

            GlobalExaminationWalkInQueueStat.ClearValues();
            GlobalExaminationAmbulanceQueueStat.ClearValues();
            GlobalExamAvgQueueStat.ClearValues();

        }

        override public void PrepareReplication()
		{
			AnimatorCreated = null;
			AnimatorRemoved = null;

			IsWarmUpDone = false;

            base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...

			if (AnimatorExists)
			{
				AnimatorCreated?.Invoke();
			}
		}

		override public void ReplicationFinished()
		{
			IDs.ResetIDs();
            GlobalPatientsAll.AddValue(AgentEnv.TotalPatientsWalk + AgentEnv.TotalPatientsAmbulance);
			GlobalPatientsWalkIn.AddValue(AgentEnv.TotalPatientsWalk);
			GlobalPatientsAmbulance.AddValue(AgentEnv.TotalPatientsAmbulance);
			GlobalTimeInSystemWalk.AddValue(AgentEnv.ReplicationTimeInSystemWalk.Mean);
			GlobalTimeInSystemAmbulance.AddValue(AgentEnv.ReplicationTimeInSystemAmbulance.Mean);

			double runTime = CurrentTime - WarmUpTime;
			if (runTime <= 0) runTime = 1;

			if (AgentNurse.Nurses.Count > 0)
			{
				GlobalUtilizationNurses.AddValue(AgentNurse.BusyNurseStat.Mean / CountOfNurses);
			}
			if (AgentDoctor.Doctors.Count > 0)
			{
				GlobalUtilizationDoctors.AddValue(AgentDoctor.BusyDoctorStat.Mean / CountOfDoctors);
			}
			if (AgentAmbulance.AmbulancesA.Count > 0)
			{ 
				GlobalUtilizationAmbulanceA.AddValue(AgentAmbulance.BusyAmbulanceAStat.Mean / CountOfAmbulancesA);
			}
            if (AgentAmbulance.AmbulancesB.Count > 0)
			{ 
				GlobalUtilizationAmbulanceB.AddValue(AgentAmbulance.BusyAmbulanceBStat.Mean / CountOfAmbulancesB);
            }
            if (AgentOfUrgent.InitialExamWaitWalkInStat.GetNumberOfValues() > 0)
            {
                GlobalInitialExamWaitWalkIn.AddValue(AgentOfUrgent.InitialExamWaitWalkInStat.Mean);
            }
			if (AgentOfUrgent.InitialExamWaitAmbulanceStat.GetNumberOfValues() > 0)
			{
				GlobalInitialExamWaitAmbulance.AddValue(AgentOfUrgent.InitialExamWaitAmbulanceStat.Mean);
			}
            if (AgentOfUrgent.ExaminationWaitWalkInStat.GetNumberOfValues() > 0)
            {
                GlobalExaminationWaitWalkIn.AddValue(AgentOfUrgent.ExaminationWaitWalkInStat.Mean);
            }
            if (AgentOfUrgent.ExaminationWaitAmbulanceStat.GetNumberOfValues() > 0)
            {
                GlobalExaminationWaitAmbulance.AddValue(AgentOfUrgent.ExaminationWaitAmbulanceStat.Mean);
            }

			GlobalInitialExamAmbulanceQueueStat.AddValue(AgentOfUrgent.PatientAmbulanceInitExamQueueStat.AverageQueueLength);
			GlobalInitialExamWalkInQueueStat.AddValue(AgentOfUrgent.PatientWalkInInitExamQueueStat.AverageQueueLength);
			GlobalExaminationAmbulanceQueueStat.AddValue(AgentOfUrgent.PatientAmbulanceExaminationQueueStat.AverageQueueLength);
			GlobalExaminationWalkInQueueStat.AddValue(AgentOfUrgent.PatientWalkInExaminationQueueStat.AverageQueueLength);

			GlobalInitialExamAvgQueueStat.AddValue(AgentOfUrgent.PatientAmbulanceInitExamQueueStat.AverageQueueLength + AgentOfUrgent.PatientWalkInInitExamQueueStat.AverageQueueLength);
			GlobalExamAvgQueueStat.AddValue(GlobalExaminationAmbulanceQueueStat.Mean + GlobalExaminationWalkInQueueStat.Mean);

            // Collect local statistics into global, update UI, etc...
            base.ReplicationFinished();
			IsWarmUpDone = false;
            OnReplicationFinishedAction?.Invoke();
        }

		public void CheckWarmUp()
		{
			
            if (IsTurboMode && !IsWarmUpDone && CurrentTime >= WarmUpTime)
			{
                IDs.ResetIDs();
                IsWarmUpDone = true;

				AgentEnv.TotalPatientsWalk = 0;
				AgentEnv.TotalPatientsAmbulance = 0;
				AgentEnv.ReplicationTimeInSystemWalk.ClearValues();
				AgentEnv.ReplicationTimeInSystemAmbulance.ClearValues();

				AgentOfUrgent.InitialExamWaitWalkInStat.ClearValues();
				AgentOfUrgent.InitialExamWaitAmbulanceStat.ClearValues();
				AgentOfUrgent.ExaminationWaitWalkInStat.ClearValues();
                AgentOfUrgent.ExaminationWaitAmbulanceStat.ClearValues();

                AgentOfUrgent.PatientWalkInInitExamQueueStat.Clear();
				AgentOfUrgent.PatientAmbulanceInitExamQueueStat.Clear();
				AgentOfUrgent.PatientWalkInExaminationQueueStat.Clear();
				AgentOfUrgent.PatientAmbulanceExaminationQueueStat.Clear();


				AgentAmbulance.BusyAmbulanceAStat.ClearValues();
				AgentAmbulance.BusyAmbulanceBStat.ClearValues();
				AgentDoctor.BusyDoctorStat.ClearValues();
				AgentNurse.BusyNurseStat.ClearValues();

				//ClearStats();

                double t = CurrentTime;
				foreach (var nurse in AgentNurse.Nurses)
				{
					nurse.TotalBusyTime = 0;
					if (nurse.IsBusy) nurse.LastStartBusyTime = t;
				}
				foreach (var doc in AgentDoctor.Doctors)
				{
					doc.TotalBusyTime = 0;
					if (doc.IsBusy) doc.LastStartBusyTime = t;
				}
				foreach (var ambA in AgentAmbulance.AmbulancesA)
				{
					ambA.TotalBusyTime = 0;
					if (ambA.IsBusy) ambA.LastStartBusyTime = t;
				}
				foreach (var ambB in AgentAmbulance.AmbulancesB)
				{
					ambB.TotalBusyTime = 0;
					if (ambB.IsBusy) ambB.LastStartBusyTime = t;
				}
			}
		}

        override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentHospital = new AgentHospital(SimId.AgentHospital, this, null);
			AgentEnv = new AgentEnv(SimId.AgentEnv, this, AgentHospital);
			AgentOfUrgent = new AgentOfUrgent(SimId.AgentOfUrgent, this, AgentHospital);
			AgentTransfers = new AgentTransfers(SimId.AgentTransfers, this, AgentOfUrgent);
			AgentExaminations = new AgentExaminations(SimId.AgentExaminations, this, AgentOfUrgent);
			AgentSources = new AgentSources(SimId.AgentSources, this, AgentOfUrgent);
			AgentDoctor = new AgentDoctor(SimId.AgentDoctor, this, AgentSources);
			AgentAmbulance = new AgentAmbulance(SimId.AgentAmbulance, this, AgentSources);
			AgentNurse = new AgentNurse(SimId.AgentNurse, this, AgentSources);
		}
		public AgentHospital AgentHospital
		{ get; set; }
		public AgentEnv AgentEnv
		{ get; set; }
		public AgentOfUrgent AgentOfUrgent
		{ get; set; }
		public AgentTransfers AgentTransfers
		{ get; set; }
		public AgentExaminations AgentExaminations
		{ get; set; }
		public AgentSources AgentSources
		{ get; set; }
		public AgentDoctor AgentDoctor
		{ get; set; }
		public AgentAmbulance AgentAmbulance
		{ get; set; }
		public AgentNurse AgentNurse
		{ get; set; }
		//meta! tag="end"
	}
}