using OSPABA;
using Simulation;

namespace Agents.AgentHospital
{
	//meta! id="1"
	public class ManagerHospital : OSPABA.Manager
	{
		public ManagerHospital(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentEnv", id="19", type="Notice"
		public void ProcessPatientArrival(MessageForm message)
		{
			message.Code = Mc.PatientExamination;
			message.Addressee = MySim.FindAgent(SimId.AgentOfUrgent);
			Request(message);
        }

		//meta! sender="AgentOfUrgent", id="21", type="Response"
		public void ProcessPatientExamination(MessageForm message)
		{
			message.Code = Mc.PatientDeparture;
			message.Addressee = MySim.FindAgent(SimId.AgentEnv);
			Notice(message);
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
			case Mc.PatientArrival:
				ProcessPatientArrival(message);
			break;

			case Mc.PatientExamination:
				ProcessPatientExamination(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentHospital MyAgent
		{
			get
			{
				return (AgentHospital)base.MyAgent;
			}
		}
	}
}