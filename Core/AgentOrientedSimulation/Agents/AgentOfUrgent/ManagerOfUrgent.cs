using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Enums;
using OSPABA;
using Simulation;
using System.Net.Cache;

namespace Agents.AgentOfUrgent
{
	//meta! id="10"
	public class ManagerOfUrgent : OSPABA.Manager
	{
		public ManagerOfUrgent(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentSources", id="79", type="Response"
		public void ProcessRequestResource(MessageForm message)
		{
            MyMessage myMessage = (MyMessage)message;
			Patient patient = myMessage.Patient;
            if (patient.Status == PatientStatus.WaitingForInitialExamination)
			{
				patient.Status = PatientStatus.InitialExamination;
			} 
			else if (patient.Status == PatientStatus.WaitingForExamination)
			{
				patient.Status = PatientStatus.Examination;
            }

            MyAgent.AmbulancesInitialPatientQueue.Dequeue();
            MyAgent.AmbulancesInitialStatistic.Dequeue();

            //Transfer for patient, nurse and doctor
            MyMessage groupTransfer = new MyMessage(MySim)
			{
                Code = Mc.Transfer,
                Addressee = MySim.FindAgent(SimId.AgentTransfers),
				Patient = patient,
				Nurse = myMessage.Nurse,
				Doctor = myMessage.Doctor,
				AmbulanceA = myMessage.AmbulanceA,
				AmbulanceB = myMessage.AmbulanceB
            };

            Request(groupTransfer);
        }

		//meta! sender="AgentExaminations", id="60", type="Response"
		public void ProcessInitalExamination(MessageForm message)
		{
            MyMessage mymessage = (MyMessage)message;
            Patient patient = mymessage.Patient;

            MyMessage freeNurse = new MyMessage(MySim) 
			{ 
				Code = Mc.FreeNurse,
				Addressee = MySim.FindAgent(SimId.AgentSources),
                Nurse = mymessage.Nurse 
			};
            Notice(freeNurse);

            MyMessage freeAmb = new MyMessage(MySim) 
			{ 
				Code = Mc.FreeAmbulance, 
				Addressee = MySim.FindAgent(SimId.AgentSources),
                AmbulanceB = mymessage.AmbulanceB 
			};
            Notice(freeAmb);

            mymessage.Nurse = null;
            mymessage.AmbulanceB = null;

            patient.Status = PatientStatus.WaitingForExamination;

            MyAgent.AmbulancesExamPatientQueue.Enqueue(patient, (patient.Priority, patient.ArrivalTime));
            MyAgent.AmbulancesExamStatistic.Enqueue(patient);

            MyMessage resourceMessage = new MyMessage(MySim) 
			{ 
				Code = Mc.RequestResource, 
				Addressee = MySim.FindAgent(SimId.AgentSources),
                Patient = patient 
			};
            Request(resourceMessage);
        }

		//meta! sender="AgentTransfers", id="59", type="Response"
		public void ProcessTransfer(MessageForm message)
		{
            MyMessage myMessage = (MyMessage)message;
            Patient patient = myMessage.Patient;

			if (patient.Status == PatientStatus.Arrived)
			{
                MyAgent.AmbulancesInitialPatientQueue.Enqueue(patient, (patient.Priority, patient.ArrivalTime));
                MyAgent.AmbulancesInitialStatistic.Enqueue(patient);

				patient.Status = PatientStatus.WaitingForInitialExamination;

                MyMessage resourcesMessage = new MyMessage(MySim)
                {
                    Code = Mc.RequestResource,
                    Addressee = MySim.FindAgent(SimId.AgentSources),
                    Patient = patient
                };
                Request(resourcesMessage);
            }
			else if (patient.Status == PatientStatus.WaitingForInitialExamination)
			{
				patient.Status = PatientStatus.InitialExamination;

                MyMessage initExamMessage = new MyMessage(MySim)
                {
                    Code = Mc.InitalExamination,
                    Addressee = MySim.FindAgent(SimId.AgentExaminations),
                    Patient = patient,
					AmbulanceB = myMessage.AmbulanceB,
					Nurse = myMessage.Nurse
                };
                Request(initExamMessage);
            }
			else if (patient.Status == PatientStatus.WaitingForExamination)
			{
				patient.Status = PatientStatus.Examination;

                MyMessage examMessage = new MyMessage(MySim)
                {
                    Code = Mc.InitalExamination,
                    Addressee = MySim.FindAgent(SimId.AgentExaminations),
                    Patient = patient,
                    AmbulanceA = myMessage.AmbulanceA,
                    Nurse = myMessage.Nurse,
					Doctor = myMessage.Doctor
                };
                Request(examMessage);
            } else if (patient.Status == PatientStatus.Leaving)
			{
				MyMessage responseMessage = new MyMessage(MySim)
				{
					Code = Mc.PatientExamination,
					Addressee = MyAgent.Parent,
					Patient = patient
				};
				Response(responseMessage);
            }
        }

		//meta! sender="AgentHospital", id="21", type="Request"
		public void ProcessPatientExamination(MessageForm message)
		{
            //Request from hospital for examination
			MyMessage myMessage = (MyMessage)message;
            Patient patient = myMessage.Patient;


            //Request transfer for patient
            MyMessage transferMessage = new MyMessage(MySim)
            {
                Code = Mc.Transfer,
                Addressee = MySim.FindAgent(SimId.AgentTransfers),
                Patient = patient
            };
			Request(transferMessage);
		}

		//meta! sender="AgentExaminations", id="61", type="Response"
		public void ProcessExamination(MessageForm message)
		{
            MyMessage mymessage = (MyMessage)message;
            Patient patient = mymessage.Patient;

            MyMessage freeNurse = new MyMessage(MySim)
            {
                Code = Mc.FreeNurse,
                Addressee = MySim.FindAgent(SimId.AgentSources),
                Nurse = mymessage.Nurse
            };
            Notice(freeNurse);

			MyMessage freeDoctor = new MyMessage(MySim)
			{
				Code = Mc.FreeDoctor,
                Addressee = MySim.FindAgent(SimId.AgentSources),
                Doctor = mymessage.Doctor
			};
			Notice(freeDoctor);

            MyMessage freeAmb = new MyMessage(MySim)
            {
                Code = Mc.FreeAmbulance,
                Addressee = MySim.FindAgent(SimId.AgentSources),
                AmbulanceB = mymessage.AmbulanceB,
				AmbulanceA = mymessage.AmbulanceA
            };
            Notice(freeAmb);

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

			case Mc.Transfer:
				ProcessTransfer(message);
			break;

			case Mc.PatientExamination:
				ProcessPatientExamination(message);
			break;

			case Mc.InitalExamination:
				ProcessInitalExamination(message);
			break;

			case Mc.Examination:
				ProcessExamination(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentOfUrgent MyAgent
		{
			get
			{
				return (AgentOfUrgent)base.MyAgent;
			}
		}
	}
}