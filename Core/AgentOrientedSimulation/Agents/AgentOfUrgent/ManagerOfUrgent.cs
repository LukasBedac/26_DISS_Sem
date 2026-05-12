using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Enums;
using OSPABA;
using Simulation;
using System.Drawing;
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
            ((MySimulation)MySim).CheckWarmUp();

            MyMessage myMessage = (MyMessage)message;
			Patient patient = myMessage.Patient;
            if (patient.Status == PatientStatus.WaitingForInitialExamination)
			{
                MyAgent.AmbulancesInitialPatientQueue.Dequeue();
				if (patient.CameByAmbulance)
				{
					MyAgent.PatientAmbulanceInitExamQueueStat.Dequeue();

				} else
				{
					MyAgent.PatientWalkInInitExamQueueStat.Dequeue();
				}
				patient.EndWaitTime = MySim.CurrentTime;
				if (patient.CameByAmbulance)
				{
					MyAgent.InitialExamWaitAmbulanceStat.AddValue(patient.EndWaitTime - patient.StartWaitTime);
				} else
				{
					MyAgent.InitialExamWaitWalkInStat.AddValue(patient.EndWaitTime - patient.StartWaitTime);
				}
            } 
			else if (patient.Status == PatientStatus.WaitingForExamination)
			{
                MyAgent.AmbulancesExamPatientQueue.Dequeue();
				if (patient.CameByAmbulance)
				{
					MyAgent.PatientAmbulanceExaminationQueueStat.Dequeue();
				} else
				{
					MyAgent.PatientWalkInExaminationQueueStat.Dequeue();
				}
				patient.EndTimeFromEntranceToExam = MySim.CurrentTime;
				if (patient.CameByAmbulance)
				{
					MyAgent.ExaminationWaitAmbulanceStat.AddValue(patient.EndTimeFromEntranceToExam - patient.StartTimeFromEntranceToExam);
                } else
				{
					MyAgent.ExaminationWaitWalkInStat.AddValue(patient.EndTimeFromEntranceToExam - patient.StartTimeFromEntranceToExam);
				}
            }

            //Transfer for patient, nurse and doctor
            MyMessage groupTransfer = (MyMessage)message.CreateCopy();
			groupTransfer.Code = Mc.Transfer;
			groupTransfer.Addressee = MySim.FindAgent(SimId.AgentTransfers);
			groupTransfer.Patient = patient;
			groupTransfer.Nurse = myMessage.Nurse;
			groupTransfer.Doctor = myMessage.Doctor;
			groupTransfer.AmbulanceA = myMessage.AmbulanceA;
			groupTransfer.AmbulanceB = myMessage.AmbulanceB;
            Request(groupTransfer);
        }

		//meta! sender="AgentExaminations", id="60", type="Response"
		public void ProcessInitalExamination(MessageForm message)
		{
            ((MySimulation)MySim).CheckWarmUp();

            MyMessage mymessage = (MyMessage)message;
            Patient patient = mymessage.Patient;

            MyMessage freeNurse = (MyMessage)message.CreateCopy();
			freeNurse.Code = Mc.FreeNurse;
			freeNurse.Addressee = MySim.FindAgent(SimId.AgentSources);
			freeNurse.Nurse = mymessage.Nurse;
            Notice(freeNurse);

            MyMessage freeAmb = (MyMessage)message.CreateCopy();
			freeAmb.Code = Mc.FreeAmbulance;
			freeAmb.Addressee = MySim.FindAgent(SimId.AgentSources);
			freeAmb.AmbulanceB = mymessage.AmbulanceB;
            Notice(freeAmb);

            mymessage.Nurse = null;
            mymessage.AmbulanceB = null;

			if (patient.CameByAmbulance)
			{
				patient.AnimItem.MoveTo(MySim.CurrentTime, 1 ,new PointF(100, 350));
			} else
			{
                patient.AnimItem.MoveTo(MySim.CurrentTime, 1 ,new PointF(100, 300));
            }
            patient.Status = PatientStatus.WaitingForExamination;

			patient.StartWaitTime = MySim.CurrentTime;

            MyAgent.AmbulancesExamPatientQueue.Enqueue(patient, (patient.Priority, patient.ArrivalTime));
            if (patient.CameByAmbulance)
			{
				MyAgent.PatientAmbulanceExaminationQueueStat.Enqueue(patient);
			} else
			{
				MyAgent.PatientWalkInExaminationQueueStat.Enqueue(patient);
            }

			MyMessage resourceMessage = (MyMessage)message.CreateCopy();
			resourceMessage.Code = Mc.RequestResource;
			resourceMessage.Addressee = MySim.FindAgent(SimId.AgentSources);
			resourceMessage.Patient = patient;
            Request(resourceMessage);
        }

		//meta! sender="AgentTransfers", id="59", type="Response"
		public void ProcessTransfer(MessageForm message)
		{
            ((MySimulation)MySim).CheckWarmUp();

            MyMessage myMessage = (MyMessage)message;
            Patient patient = myMessage.Patient;

			if (patient.Status == PatientStatus.Arrived)
			{
				patient.StartWaitTime = MySim.CurrentTime;

                MyAgent.AmbulancesInitialPatientQueue.Enqueue(patient, (patient.Priority, patient.ArrivalTime));
                if (patient.CameByAmbulance)
				{
					MyAgent.PatientAmbulanceInitExamQueueStat.Enqueue(patient);
                } else
				{
					MyAgent.PatientWalkInInitExamQueueStat.Enqueue(patient);
                }

				patient.Status = PatientStatus.WaitingForInitialExamination;

                MyMessage resourcesMessage = (MyMessage)message.CreateCopy();
                resourcesMessage.Code = Mc.RequestResource;
                resourcesMessage.Addressee = MySim.FindAgent(SimId.AgentSources);
                resourcesMessage.Patient = patient;
                Request(resourcesMessage);
            }
			else if (patient.Status == PatientStatus.WaitingForInitialExamination)
			{
				patient.Status = PatientStatus.InitialExamination;

                MyMessage initExamMessage = (MyMessage)message.CreateCopy();
                initExamMessage.Code = Mc.InitalExamination;
                initExamMessage.Addressee = MySim.FindAgent(SimId.AgentExaminations);
                initExamMessage.Patient = patient;
                initExamMessage.AmbulanceB = myMessage.AmbulanceB;
                initExamMessage.Nurse = myMessage.Nurse;
                Request(initExamMessage);
            }
			else if (patient.Status == PatientStatus.WaitingForExamination)
			{
				patient.Status = PatientStatus.Examination;

                MyMessage examMessage = (MyMessage)message.CreateCopy();
                examMessage.Code = Mc.Examination;
                examMessage.Addressee = MySim.FindAgent(SimId.AgentExaminations);
                examMessage.Patient = patient;
                examMessage.Nurse = myMessage.Nurse;
                Request(examMessage);
            } 
			else if (patient.Status == PatientStatus.Leaving)
			{
				MyMessage responseMessage = (MyMessage)message.CreateCopy();
				responseMessage.Code = Mc.PatientExamination;
				responseMessage.Addressee = MyAgent.Parent;
				responseMessage.Patient = patient;
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
            MyMessage transferMessage = (MyMessage)myMessage.CreateCopy();
            transferMessage.Code = Mc.Transfer;
            transferMessage.Addressee = MySim.FindAgent(SimId.AgentTransfers);
            transferMessage.Patient = patient;
			Request(transferMessage);
		}

		//meta! sender="AgentExaminations", id="61", type="Response"
		public void ProcessExamination(MessageForm message)
		{
            ((MySimulation)MySim).CheckWarmUp();

            MyMessage mymessage = (MyMessage)message;
            Patient patient = mymessage.Patient;

			patient.EndTimeFromEntranceToExam = MySim.CurrentTime;

            MyMessage freeNurse = (MyMessage)message.CreateCopy();
            freeNurse.Code = Mc.FreeNurse;
			freeNurse.Addressee = MySim.FindAgent(SimId.AgentSources);
			freeNurse.Nurse = mymessage.Nurse;
            Notice(freeNurse);

			MyMessage freeDoctor = (MyMessage)message.CreateCopy();
            freeDoctor.Code = Mc.FreeDoctor;
            freeDoctor.Addressee = MySim.FindAgent(SimId.AgentSources);
            freeDoctor.Doctor = mymessage.Doctor;	
			Notice(freeDoctor);

            MyMessage freeAmb = (MyMessage)message.CreateCopy();
            freeAmb.Code = Mc.FreeAmbulance;
            freeAmb.Addressee = MySim.FindAgent(SimId.AgentSources);
            freeAmb.AmbulanceB = mymessage.AmbulanceB;
			freeAmb.AmbulanceA = mymessage.AmbulanceA;
            Notice(freeAmb);

            mymessage.Nurse = null;
            mymessage.Doctor = null;
            mymessage.AmbulanceA = null;
            mymessage.AmbulanceB = null;

            patient.Status = PatientStatus.Leaving;
            MyMessage transferMessage = (MyMessage)message.CreateCopy();
            transferMessage.Code = Mc.Transfer;
            transferMessage.Addressee = MySim.FindAgent(SimId.AgentTransfers);
            transferMessage.Patient = patient;
            Request(transferMessage);
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

			case Mc.Examination:
				ProcessExamination(message);
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