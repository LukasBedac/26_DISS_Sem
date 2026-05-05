using OSPABA;
using Simulation;

namespace Agents.AgentEnv
{
	//meta! id="2"
	public class ManagerEnv : OSPABA.Manager
	{
		public ManagerEnv(int id, OSPABA.Simulation mySim, Agent myAgent) :
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
            MyMessage startFoot = new MyMessage(MySim);
            startFoot.Addressee = MyAgent.FindAssistant(SimId.SchedulerByFoot);
            StartContinualAssistant(startFoot);

            MyMessage startAmbulance = new MyMessage(MySim);
            startAmbulance.Addressee = MyAgent.FindAssistant(SimId.Schedulermbulance);
            StartContinualAssistant(startAmbulance);
        }

		//meta! sender="AgentHospital", id="20", type="Notice"
		public void ProcessPatientDeparture(MessageForm message)
		{
            MyMessage myMessage = (MyMessage)message;

			MyAgent.PatientTimeInSystem.AddValue(MySim.CurrentTime - myMessage.Patient.ArrivalTime);
        }

		//meta! sender="SchedulerByFoot", id="9", type="Finish"
		public void ProcessFinishSchedulerByFoot(MessageForm message)
		{
			message.Addressee = MyAgent.Parent;
			Notice(message);

			MyMessage newMessage = (MyMessage)message.CreateCopy();
			newMessage.Addressee = MyAgent.FindAssistant(SimId.SchedulerByFoot);
			StartContinualAssistant(newMessage);
        }

		//meta! sender="Schedulermbulance", id="7", type="Finish"
		public void ProcessFinishSchedulermbulance(MessageForm message)
		{
            message.Addressee = MyAgent.Parent;
            Notice(message);

            MyMessage newMessage = (MyMessage)message.CreateCopy();
            newMessage.Addressee = MyAgent.FindAssistant(SimId.Schedulermbulance);
            StartContinualAssistant(newMessage);
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.PatientArrival:
                    message.Addressee = MyAgent.Parent;
                    Notice(message);
					break;
                default:
					throw new Exception("Default method selected instead of schedulers");
					
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
			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.SchedulerByFoot:
					ProcessFinishSchedulerByFoot(message);
				break;

				case SimId.Schedulermbulance:
					ProcessFinishSchedulermbulance(message);
				break;
				}
			break;

			case Mc.PatientDeparture:
				ProcessPatientDeparture(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentEnv MyAgent
		{
			get
			{
				return (AgentEnv)base.MyAgent;
			}
		}
	}
}