using OSPABA;
using Simulation;
using Core.AgentOrientedSimulation.Models;

namespace Agents.AgentDoctor
{
	//meta! id="12"
	public class AgentDoctor : OSPABA.Agent
	{
        public List<Doctor> Doctors { get; set; } = new();
        public List<Doctor> FreeDoctors { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> Queue { get; set; } = new();
        public AgentDoctor(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
            
        }

		override public void PrepareReplication()
		{
            Doctors.Clear();
            FreeDoctors.Clear();
            Queue.Clear();

            base.PrepareReplication();
            // Setup component for the next replication
            for (int i = 0; i < ((MySimulation)MySim).CountOfDoctors; i++)
            {
				Doctor doctor = new Doctor(MySim);
                Doctors.Add(doctor);
                FreeDoctors.Add(doctor);
            }
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