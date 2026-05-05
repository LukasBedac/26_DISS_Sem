using OSPABA;
using Simulation;

namespace Agents.AgentDoctor
{
	//meta! id="12"
	public class ManagerDoctor : OSPABA.Manager
	{
		public ManagerDoctor(int id, OSPABA.Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentSources", id="67", type="Notice"
		public void ProcessFreeDoctor(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            MyAgent.FreeDoctors.Add(msg.Doctor);

            if (MyAgent.Queue.Count > 0)
            {
                MyMessage nextMsg = MyAgent.Queue.Dequeue();
                nextMsg.Doctor = MyAgent.FreeDoctors[0];
                MyAgent.FreeDoctors.RemoveAt(0);
                Response(nextMsg);
            }
        }

		//meta! sender="AgentSources", id="26", type="Request"
		public void ProcessRequestDoctorAllocation(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            if (MyAgent.FreeDoctors.Count > 0)
            {
                msg.Doctor = MyAgent.FreeDoctors[0];
                MyAgent.FreeDoctors.RemoveAt(0);
                Response(msg);
            }
            else MyAgent.Queue.Enqueue(msg, (msg.Patient.Priority, msg.Patient.ArrivalTime));
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.FreeDoctor:
				ProcessFreeDoctor(message);
			break;

			case Mc.RequestDoctorAllocation:
				ProcessRequestDoctorAllocation(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentDoctor MyAgent
		{
			get
			{
				return (AgentDoctor)base.MyAgent;
			}
		}
	}
}