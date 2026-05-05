using OSPABA;
using Simulation;

namespace Agents.AgentExaminations
{
	//meta! id="47"
	public class ManagerExaminations : OSPABA.Manager
	{
		public ManagerExaminations(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentOfUrgent", id="60", type="Request"
		public void ProcessInitalExamination(MessageForm message)
		{
			message.Addressee = MyAgent.FindAssistant(SimId.InitialExamination);
			StartContinualAssistant(message);
        }

		//meta! sender="Examination", id="55", type="Finish"
		public void ProcessFinishExamination(MessageForm message)
		{
            message.Code = Mc.Examination;
            Response(message);
        }

		//meta! sender="InitialExamination", id="53", type="Finish"
		public void ProcessFinishInitialExamination(MessageForm message)
		{
			message.Code = Mc.InitalExamination;
            Response(message);
        }

		//meta! sender="AgentOfUrgent", id="61", type="Request"
		public void ProcessExamination(MessageForm message)
		{
			message.Addressee = MyAgent.FindAssistant(SimId.Examination);
			StartContinualAssistant(message);
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
			case Mc.Examination:
				ProcessExamination(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.Examination:
					ProcessFinishExamination(message);
				break;

				case SimId.InitialExamination:
					ProcessFinishInitialExamination(message);
				break;
				}
			break;

			case Mc.InitalExamination:
				ProcessInitalExamination(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentExaminations MyAgent
		{
			get
			{
				return (AgentExaminations)base.MyAgent;
			}
		}
	}
}