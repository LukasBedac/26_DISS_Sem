using Agents.AgentDoctor;
using OSPABA;
using Agents.AgentHospital;
using Agents.AgentTransfers;
using Agents.AgentNurse;
using Agents.AgentOfUrgent;
using Agents.AgentEnv;
using Agents.AgentAmbulance;
using Agents.AgentSources;
using Agents.AgentExaminations;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{

        public int CountOfNurses { get; set; }
        public int CountOfDoctors { get; set; }
		public int CountOfAmbulancesA { get; set; } = 5;
		public int CountOfAmbulancesB { get; set; } = 7;

		public event System.Action AnimatorCreated;
        public event System.Action AnimatorRemoved;
        public MySimulation()
		{
            Init();
		}

        override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
		}

		override public void PrepareReplication()
		{
			AnimatorCreated = null;
			AnimatorRemoved = null;

            base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...

			if (AnimatorExists)
			{
				AnimatorCreated?.Invoke();
			}
		}

		override public void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();
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