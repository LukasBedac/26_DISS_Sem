using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Enums;
using OSPABA;
using Simulation;

namespace Agents.AgentSources
{
	//meta! id="65"
	public class ManagerSources : OSPABA.Manager
	{
        private PriorityQueue<MyMessage, (int, double)> _initialExamqueue = new();
        private PriorityQueue<MyMessage, (int, double)> _examQueue = new();
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
            _initialExamqueue.Clear();
            _examQueue.Clear();
        }

		//meta! sender="AgentNurse", id="25", type="Response"
		public void ProcessRequestNurseAllocation(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            if (msg.Patient.Status == PatientStatus.WaitingForInitialExamination)
            {
                msg.Code = Mc.RequestResource;
                AssignResources(msg);
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
            MySimulation sim = (MySimulation)MySim;

            if (sim.Variant == 1)
            {
			    if (patient.Status == PatientStatus.WaitingForInitialExamination)
			    {
                    _initialExamqueue.Enqueue(myMessage, (myMessage.Patient.Priority, myMessage.Patient.ArrivalTime));
        //            myMessage.Code = Mc.RequestAmbulanceAllocationB;
				    //myMessage.Addressee = MySim.FindAgent(SimId.AgentAmbulance);
        //            Request(myMessage);
                } else
			    {
                    _examQueue.Enqueue(myMessage, (myMessage.Patient.Priority, myMessage.Patient.ArrivalTime));
        //            myMessage.Code = Mc.RequestAmbulanceAllocationA;
				    //myMessage.Addressee = MySim.FindAgent(SimId.AgentAmbulance);
        //            Request(myMessage);
                }

                TryAllocateVariant1();

            } 
            else if (sim.Variant == 2)
            {

            }
            else //3 for now, can be expanded later - testing variant withotu check
            {
                TryAllocateAllResources(myMessage);
            }

        }

        

        //meta! sender="AgentDoctor", id="26", type="Response"
        public void ProcessRequestDoctorAllocation(MessageForm message)
		{
            MyMessage msg = (MyMessage)message;
            msg.Code = Mc.RequestResource;

			AssignResources(msg);
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

		//meta! sender="AgentOfUrgent", id="85", type="Notice"
		public void ProcessFreeResource(MessageForm message)
		{
			MyMessage myMessage = (MyMessage)message;

            MyMessage freeAmbulanceMsg = (MyMessage)myMessage.CreateCopy();
            freeAmbulanceMsg.Code = Mc.FreeAmbulance;
            freeAmbulanceMsg.Addressee = MySim.FindAgent(SimId.AgentAmbulance);
            if (myMessage.AmbulanceA != null)
            {
                freeAmbulanceMsg.AmbulanceA = myMessage.AmbulanceA;
            }
            else
            {
                freeAmbulanceMsg.AmbulanceB = myMessage.AmbulanceB;
            }
            Notice(freeAmbulanceMsg);

            if (myMessage.Nurse != null)
            {
                MyMessage freeNurseMsg = (MyMessage)myMessage.CreateCopy();
                freeNurseMsg.Code = Mc.FreeNurse;
                freeNurseMsg.Addressee = MySim.FindAgent(SimId.AgentNurse);
                Notice(freeNurseMsg);
            }

            if (myMessage.Doctor != null)
            {
                MyMessage freeDoctorMsg = (MyMessage)myMessage.CreateCopy();
                freeDoctorMsg.Code = Mc.FreeDoctor;
                freeDoctorMsg.Addressee = MySim.FindAgent(SimId.AgentDoctor);
                Notice(freeDoctorMsg);
            }
			UnAssignResources(myMessage);

            //Try allocate again for next top message
            if (((MySimulation)MySim).Variant == 1)
            {
                TryAllocateVariant1();
            }
        }

        private void UnAssignResources(MyMessage msg)
        {
            if (msg.AmbulanceA != null)
            {
                msg.AmbulanceA.TotalBusyTime += (MySim.CurrentTime - msg.AmbulanceA.LastStartBusyTime);
                msg.AmbulanceA.IsBusy = false;
                msg.AmbulanceA.AssignedPatient = null;
                msg.AmbulanceA.Doctor = null;
                msg.AmbulanceA.Nurse = null;
                msg.Patient.AmbulanceA = null;
            }
            if (msg.AmbulanceB != null)
            {
                msg.AmbulanceB.TotalBusyTime += (MySim.CurrentTime - msg.AmbulanceB.LastStartBusyTime);
                msg.AmbulanceB.IsBusy = false;
                msg.AmbulanceB.AssignedPatient = null;
                msg.AmbulanceB.Doctor = null;
                msg.AmbulanceB.Nurse = null;
                msg.Patient.AmbulanceB = null;
            }
            if (msg.Doctor != null)
            {
                msg.Doctor.TotalBusyTime += (MySim.CurrentTime - msg.Doctor.LastStartBusyTime);
                msg.Doctor.IsBusy = false;
                msg.Doctor.AssignedPatient = null;
                msg.Doctor.AmbulanceA = null;
                msg.Doctor.AmbulanceB = null;
            }
            if (msg.Nurse != null)
            {
                msg.Nurse.TotalBusyTime += (MySim.CurrentTime - msg.Nurse.LastStartBusyTime);
                msg.Nurse.IsBusy = false;
                msg.Nurse.AssignedPatient = null;
                msg.Nurse.AmbulanceA = null;
                msg.Nurse.AmbulanceB = null;
            }
            
        }

        private void AssignResources(MyMessage msg)
        {
            if (msg.AmbulanceA != null)
			{
				msg.AmbulanceA.IsBusy = true;
                msg.AmbulanceA.LastStartBusyTime = MySim.CurrentTime;
                msg.AmbulanceA.AssignedPatient = msg.Patient;
				if (msg.Doctor != null)
				{
					msg.AmbulanceA.Doctor = msg.Doctor;
                    msg.Doctor.AssignedPatient = msg.Patient;
                    msg.Doctor.IsBusy = true;
                    msg.Doctor.LastStartBusyTime = MySim.CurrentTime;
                }
				msg.AmbulanceA.Nurse = msg.Nurse;

				msg.Doctor.AmbulanceA = msg.AmbulanceA;
				msg.Nurse.AmbulanceA = msg.AmbulanceA;
				msg.Patient.AmbulanceA = msg.AmbulanceA;


            } else
			{
                msg.AmbulanceB.IsBusy = true;
                msg.AmbulanceB.LastStartBusyTime = MySim.CurrentTime;
                msg.AmbulanceB.AssignedPatient = msg.Patient;
                msg.AmbulanceB.Nurse = msg.Nurse;

                if (msg.Doctor != null)
                {
                    msg.AmbulanceB.Doctor = msg.Doctor;
                    msg.Doctor.AssignedPatient = msg.Patient;
                    msg.Doctor.IsBusy = true;
                    msg.Doctor.LastStartBusyTime = MySim.CurrentTime;
                }

                msg.Nurse.AmbulanceB = msg.AmbulanceB;
				msg.Patient.AmbulanceB = msg.AmbulanceB;
            }
			
            msg.Nurse.IsBusy = true;
            msg.Nurse.AssignedPatient = msg.Patient;
            msg.Nurse.LastStartBusyTime = MySim.CurrentTime;
        }

        private void TryAllocateVariant1()
        {
            var sim = (MySimulation)MySim;
            bool allocated = true;

            while (allocated)
            {
                allocated = false;

                int freeDocs = sim.AgentDoctor.Doctors.Count;
                int freeNurses = sim.AgentNurse.Nurses.Count;
                int freeAmbA = sim.AgentAmbulance.AmbulancesA.Count;
                int freeAmbB = sim.AgentAmbulance.AmbulancesB.Count;

                if (_examQueue.Count > 0)
                {
                    var firstMessage = _examQueue.Peek();

                    if (freeDocs > 0 && freeNurses > 0 && (freeAmbA > 0 || freeAmbB > 0))
                    {
                        _examQueue.Dequeue();
                        TryAllocateAllResources(firstMessage);
                        allocated = true;
                        continue;
                    }
                }

                if (_initialExamqueue.Count > 0)
                {
                    var firstMessage = _initialExamqueue.Peek();

                    if (freeNurses > 0 && freeAmbB > 0)
                    {
                        _initialExamqueue.Dequeue();
                        TryAllocateAllResources(firstMessage);
                        allocated = true;
                        continue;
                    }
                }
            }
        }

        private void TryAllocateAllResources(MyMessage msg)
        {
            if (msg.Patient.Status == PatientStatus.WaitingForInitialExamination)
            {
                msg.Code = Mc.RequestAmbulanceAllocationB;
                msg.Addressee = MySim.FindAgent(SimId.AgentAmbulance);
                Request(msg);
            }
            else
            {
                msg.Code = Mc.RequestAmbulanceAllocationA;
                msg.Addressee = MySim.FindAgent(SimId.AgentAmbulance);
                Request(msg);
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
			case Mc.FreeResource:
				ProcessFreeResource(message);
			break;

			case Mc.RequestResource:
				ProcessRequestResource(message);
			break;

			case Mc.RequestDoctorAllocation:
				ProcessRequestDoctorAllocation(message);
			break;

			case Mc.RequestAmbulanceAllocationB:
				ProcessRequestAmbulanceAllocationB(message);
			break;

			case Mc.RequestAmbulanceAllocationA:
				ProcessRequestAmbulanceAllocationA(message);
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