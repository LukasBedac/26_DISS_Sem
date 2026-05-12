using Core.AgentOrientedSimulation.Models;
using OSPABA;
using Simulation;
using System.Windows.Interop;

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
            double timeInSystem = MySim.CurrentTime - myMessage.Patient.ArrivalTime;
            if (myMessage.Patient.CameByAmbulance)
            {
                MyAgent.ReplicationTimeInSystemAmbulance.AddValue(timeInSystem);
            }
            else
            {
                MyAgent.ReplicationTimeInSystemWalk.AddValue(timeInSystem);
            }

			if (!((MySimulation)MySim).IsTurboMode && myMessage.Patient.AnimItem != null)
			{
				MySim.Animator.Remove(myMessage.Patient.AnimItem);
			}
            myMessage.Patient.AnimItem = null;
        }

		//meta! sender="SchedulerByFoot", id="9", type="Finish"
		public void ProcessFinishSchedulerByFoot(MessageForm message)
		{
            ((MySimulation)MySim).CheckWarmUp();

            MyMessage myMessage = (MyMessage)message;
            MyAgent.TotalPatientsWalk++;

            myMessage.Patient = CreatePatient(6);

            MyMessage noticeMsg = (MyMessage)myMessage.CreateCopy();
            noticeMsg.Code = Mc.PatientArrival;
            noticeMsg.Addressee = MyAgent.Parent;
			Notice(noticeMsg);

			MyMessage newMessage = (MyMessage)myMessage.CreateCopy();
			newMessage.Code = Mc.Start;
			newMessage.Addressee = MyAgent.FindAssistant(SimId.SchedulerByFoot);
			StartContinualAssistant(newMessage);
        }

		//meta! sender="Schedulermbulance", id="7", type="Finish"
		public void ProcessFinishSchedulermbulance(MessageForm message)
		{
            ((MySimulation)MySim).CheckWarmUp();

			MyMessage myMessage = (MyMessage)message;
            MyAgent.TotalPatientsAmbulance++;

			myMessage.Patient = CreatePatient(0);

            MyMessage noticeMsg = (MyMessage)myMessage.CreateCopy();
            noticeMsg.Code = Mc.PatientArrival;
            noticeMsg.Addressee = MyAgent.Parent;
            Notice(noticeMsg);

            MyMessage newMessage = (MyMessage)myMessage.CreateCopy();
			newMessage.Code = Mc.Start;
            newMessage.Addressee = MyAgent.FindAssistant(SimId.Schedulermbulance);
            StartContinualAssistant(newMessage);
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			//MyMessage myMessage = (MyMessage)message;
			//double timeInSystem = MySim.CurrentTime - myMessage.Patient.ArrivalTime;
			switch (message.Code)
			{
				//case Mc.PatientArrival:
    //                if (myMessage.Patient.CameByAmbulance)
    //                {
    //                    MyAgent.ReplicationTimeInSystemAmbulance.AddValue(timeInSystem);
    //                    MyAgent.TotalPatientsAmbulance++;
    //                }
    //                else
    //                {
    //                    MyAgent.ReplicationTimeInSystemWalk.AddValue(timeInSystem);
    //                    MyAgent.TotalPatientsWalk++;
    //                }
    //                message.Addressee = MyAgent.Parent;
    //                Notice(message);
				//	break;
    //            default:
				//	throw new Exception("Default method selected instead of schedulers");
					
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
        public Patient CreatePatient(int priority)
        {
            Patient patient = new Patient(MySim, priority, MySim.CurrentTime);
            return patient;
        }
    }
}