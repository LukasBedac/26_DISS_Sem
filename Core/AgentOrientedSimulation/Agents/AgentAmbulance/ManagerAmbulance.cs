using OSPABA;
using Simulation;
using System.Windows.Interop;

namespace Agents.AgentAmbulance
{
	//meta! id="13"
	public class ManagerAmbulance : OSPABA.Manager
	{
		public ManagerAmbulance(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentSources", id="69", type="Notice"
		public void ProcessFreeAmbulance(MessageForm message)
		{
			MyMessage msg = (MyMessage)message;
			if (msg.AmbulanceA != null)
			{
				MyAgent.FreeA.Add(msg.AmbulanceA);
				if (MyAgent.QueueA.Count > 0)
				{
					MyMessage nextMsg = MyAgent.QueueA.Dequeue();
					nextMsg.AmbulanceA = MyAgent.FreeA[0]; MyAgent.FreeA.RemoveAt(0);
					Response(nextMsg);
				}
				else if (MyAgent.QueueAny.Count > 0)
				{
					MyMessage nextMsg = MyAgent.QueueAny.Dequeue();
					nextMsg.AmbulanceA = MyAgent.FreeA[0]; MyAgent.FreeA.RemoveAt(0);
					Response(nextMsg);
				}
			}
			if (msg.AmbulanceB != null)
			{
				MyAgent.FreeB.Add(msg.AmbulanceB);
				if (MyAgent.QueueB.Count > 0)
				{
					MyMessage nextMsg = MyAgent.QueueB.Dequeue();
					nextMsg.AmbulanceB = MyAgent.FreeB[0]; MyAgent.FreeB.RemoveAt(0);
					Response(nextMsg);
				}
				else if (MyAgent.QueueAny.Count > 0)
				{
					MyMessage nextMsg = MyAgent.QueueAny.Dequeue();
					nextMsg.AmbulanceB = MyAgent.FreeB[0]; MyAgent.FreeB.RemoveAt(0);
					Response(nextMsg);
				}
			}
		}

		//meta! sender="AgentSources", id="23", type="Request"
		public void ProcessRequestAmbulanceAllocationB(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            if (MyAgent.FreeB.Count > 0)
            {
                msg.AmbulanceB = MyAgent.FreeB[0];
                MyAgent.FreeB.RemoveAt(0);
                Response(msg);
            }
            else MyAgent.QueueB.Enqueue(msg, (msg.Patient.Priority, msg.Patient.ArrivalTime));
        }

		//meta! sender="AgentSources", id="22", type="Request"
		public void ProcessRequestAmbulanceAllocationA(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            int prio = msg.Patient.Priority;

            if (prio == 1 || prio == 2)
            {
                if (MyAgent.FreeA.Count > 0) 
				{ 
					msg.AmbulanceA = MyAgent.FreeA[0];
					MyAgent.FreeA.RemoveAt(0);
					Response(msg);
				}
                else MyAgent.QueueA.Enqueue(msg, (prio, msg.Patient.ArrivalTime));
            }
            else if (prio == 5)
            {
                if (MyAgent.FreeB.Count > 0) 
				{ 
					msg.AmbulanceB = MyAgent.FreeB[0];
					MyAgent.FreeB.RemoveAt(0);
					Response(msg);
				}
                else MyAgent.QueueB.Enqueue(msg, (prio, msg.Patient.ArrivalTime));
            }
            else
            {
                if (MyAgent.FreeA.Count > 0) 
				{
					msg.AmbulanceA = MyAgent.FreeA[0];
					MyAgent.FreeA.RemoveAt(0); 
					Response(msg); 
				}
                else if (MyAgent.FreeB.Count > 0) 
				{ 
					msg.AmbulanceB = MyAgent.FreeB[0]; 
					MyAgent.FreeB.RemoveAt(0); Response(msg); 
				}
                else MyAgent.QueueAny.Enqueue(msg, (prio, msg.Patient.ArrivalTime));
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
			case Mc.FreeAmbulance:
				ProcessFreeAmbulance(message);
			break;

			case Mc.RequestAmbulanceAllocationA:
				ProcessRequestAmbulanceAllocationA(message);
			break;

			case Mc.RequestAmbulanceAllocationB:
				ProcessRequestAmbulanceAllocationB(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentAmbulance MyAgent
		{
			get
			{
				return (AgentAmbulance)base.MyAgent;
			}
		}
	}
}