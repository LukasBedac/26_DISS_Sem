using OSPABA;
using Simulation;
using Core.AgentOrientedSimulation.Models;
using Core.EventSimulation.Statistics;

namespace Agents.AgentDoctor
{
	//meta! id="12"
	public class AgentDoctor : OSPABA.Agent
	{
        public List<Doctor> Doctors { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> Queue { get; set; } = new();
        public WeightedStatistic BusyDoctorStat { get; set; } = new();
		private double _lastUpdateDoctors = 0.0;
        public AgentDoctor(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
            
        }

		override public void PrepareReplication()
		{
            Doctors.Clear();
            Queue.Clear();
			BusyDoctorStat.ClearValues();
			_lastUpdateDoctors = 0.0;

            base.PrepareReplication();
            // Setup component for the next replication
            for (int i = 0; i < ((MySimulation)MySim).CountOfDoctors; i++)
            {
				Doctor doctor = new Doctor(MySim);
                Doctors.Add(doctor);
            }
        }

        public void RecordDoctorStat()
        {
            double total = ((MySimulation)MySim).CountOfDoctors;
            double busyCount = total - Doctors.Count;
            double timeDelta = MySim.CurrentTime - _lastUpdateDoctors;

            if (timeDelta > 0)
            {
                BusyDoctorStat.AddValue(busyCount, timeDelta);
            }
            _lastUpdateDoctors = MySim.CurrentTime;
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
		{
			new ManagerDoctor(SimId.ManagerDoctor, MySim, this);
			AddOwnMessage(Mc.FreeDoctor);
			AddOwnMessage(Mc.RequestDoctorAllocation);
		}
		//meta! tag="end"
	}
}