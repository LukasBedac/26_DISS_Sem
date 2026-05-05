using OSPABA;
using Simulation;
using System.Windows.Interop;

namespace Agents.AgentNurse
{
	//meta! id="14"
	public class ManagerNurse : OSPABA.Manager
	{
        public ManagerNurse(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentSources", id="25", type="Request"
		public void ProcessRequestNurseAllocation(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            if (MyAgent.FreeNurses.Count > 0)
            {
                msg.Nurse = MyAgent.FreeNurses[0];
                MyAgent.FreeNurses.RemoveAt(0);
                Response(msg);
            }
            else MyAgent.Queue.Enqueue(msg, (msg.Patient.Priority, msg.Patient.ArrivalTime));
        
		}

		//meta! sender="AgentSources", id="68", type="Notice"
		public void ProcessFreeNurse(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            MyAgent.FreeNurses.Add(msg.Nurse);

            if (MyAgent.Queue.Count > 0)
            {
                MyMessage nextMsg = MyAgent.Queue.Dequeue();
                nextMsg.Nurse = MyAgent.FreeNurses[0];
                MyAgent.FreeNurses.RemoveAt(0);
                Response(nextMsg);
            }
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
			case Mc.FreeNurse:
				ProcessFreeNurse(message);
			break;

			case Mc.RequestNurseAllocation:
				ProcessRequestNurseAllocation(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentNurse MyAgent
		{
			get
			{
				return (AgentNurse)base.MyAgent;
			}
		}
	}
}