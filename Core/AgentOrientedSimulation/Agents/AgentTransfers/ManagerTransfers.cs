using OSPABA;
using Simulation;

namespace Agents.AgentTransfers
{
	//meta! id="44"
	public class ManagerTransfers : OSPABA.Manager
	{
		public ManagerTransfers(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentOfUrgent", id="59", type="Request"
		public void ProcessTransfer(MessageForm message)
		{
            message.Addressee = MyAgent.FindAssistant(SimId.Transfer);
            StartContinualAssistant(message);
        }

		//meta! sender="Transfer", id="58", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
            message.Code = Mc.Transfer;
            Response(message);
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
			case Mc.Transfer:
				ProcessTransfer(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentTransfers MyAgent
		{
			get
			{
				return (AgentTransfers)base.MyAgent;
			}
		}
	}
}