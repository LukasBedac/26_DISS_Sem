using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Enums;
using OSPABA;
using Simulation;

namespace Agents.AgentSources
{
	//meta! id="65"
	public class ManagerSources : OSPABA.Manager
	{
		public ManagerSources(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentNurse", id="25", type="Response"
		public void ProcessRequestNurseAllocation(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            if (msg.Patient.Status == PatientStatus.WaitingForInitialExamination)
            {
                msg.Code = Mc.RequestResource;
                Response(msg);
            }
            else
            {
                msg.Code = Mc.RequestDoctorAllocation;
                msg.Addressee = MySim.FindAgent(SimId.AgentDoctor);
                Request(msg);
            }
        }

		//meta! sender="AgentOfUrgent", id="79", type="Request"
		public void ProcessRequestResource(MessageForm message)
		{
            MyMessage myMessage = (MyMessage)message;
            Patient patient = myMessage.Patient;

			if (patient.Status == PatientStatus.WaitingForInitialExamination)
			{
                myMessage.Code = Mc.RequestAmbulanceAllocationB;
				myMessage.Addressee = MySim.FindAgent(SimId.AgentAmbulance);
                Request(myMessage);
            } else
			{
                myMessage.Code = Mc.RequestAmbulanceAllocationA;
				myMessage.Addressee = MySim.FindAgent(SimId.AgentAmbulance);
                Request(myMessage);
            }
        }

		//meta! sender="AgentDoctor", id="26", type="Response"
		public void ProcessRequestDoctorAllocation(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            msg.Code = Mc.RequestResource;
            Response(msg);
        }

		//meta! sender="AgentAmbulance", id="23", type="Response"
		public void ProcessRequestAmbulanceAllocationB(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            msg.Code = Mc.RequestNurseAllocation;
            msg.Addressee = MySim.FindAgent(SimId.AgentNurse);
            Request(msg);
        }

		//meta! sender="AgentAmbulance", id="22", type="Response"
		public void ProcessRequestAmbulanceAllocationA(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            msg.Code = Mc.RequestNurseAllocation;
            msg.Addressee = MySim.FindAgent(SimId.AgentNurse);
            Request(msg);
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
			case Mc.RequestResource:
				ProcessRequestResource(message);
			break;

			case Mc.RequestAmbulanceAllocationA:
				ProcessRequestAmbulanceAllocationA(message);
			break;

			case Mc.RequestDoctorAllocation:
				ProcessRequestDoctorAllocation(message);
			break;

			case Mc.RequestAmbulanceAllocationB:
				ProcessRequestAmbulanceAllocationB(message);
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
		public new AgentSources MyAgent
		{
			get
			{
				return (AgentSources)base.MyAgent;
			}
		}
	}
}