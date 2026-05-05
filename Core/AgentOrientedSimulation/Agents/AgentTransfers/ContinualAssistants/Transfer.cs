using Agents.AgentTransfers;
using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Enums;
using Core.Distributions;
using Core.Distributions.Helpers;
using OSPABA;
using OSPAnimator;
using Simulation;
using System.Drawing;

namespace Agents.AgentTransfers.ContinualAssistants
{
	//meta! id="57"
	public class Transfer : OSPABA.Process
	{
        private Triangular _pOFBeforeInitExamTransferGenerator;
        private ContinuousUniform _pBABeforeInitExamTransferGenerator;
        private Triangular _ambulanceTransferGenerator;
        private ContinuousUniform _ambulanceToEntrancePOFTransferGenerator;
        public Transfer(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
            _pOFBeforeInitExamTransferGenerator = new Triangular(new Scope(120.0, 150.0, 300.0));
			_pBABeforeInitExamTransferGenerator = new ContinuousUniform(90, 200);
			_ambulanceTransferGenerator = new Triangular(new Scope(15.0, 30.0, 45.0));
            _ambulanceToEntrancePOFTransferGenerator = new ContinuousUniform(150, 240);
        }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentTransfers", id="58", type="Start"
		public void ProcessStart(MessageForm message)
		{
			MyMessage myMessage = (MyMessage)message;
			Patient patient = myMessage.Patient;
			double transferTime = 0;
			double maxTransferTime = 0;

            if (patient.Status == PatientStatus.Arrived)
			{
				if (patient.Priority == 0)
				{
					transferTime = _pBABeforeInitExamTransferGenerator.NextDouble(); 
				} else if (patient.Priority == 6)
				{
					transferTime = _pOFBeforeInitExamTransferGenerator.NextDouble();
                }
                
				CreatePatientPath(myMessage, transferTime);

				patient.Status = PatientStatus.WaitingForInitialExamination;
				if (transferTime > maxTransferTime) maxTransferTime = transferTime;
                
            }  
            else if (patient.Status == PatientStatus.WaitingForInitialExamination)
            {
                transferTime = _ambulanceTransferGenerator.NextDouble();
                CreatePatientPath(myMessage, transferTime);
                if (transferTime > maxTransferTime) maxTransferTime = transferTime;
            }
            else if (patient.Status == PatientStatus.WaitingForExamination)
            {
                transferTime = _ambulanceTransferGenerator.NextDouble();
                CreatePatientPath(myMessage, transferTime);
                if (transferTime > maxTransferTime) maxTransferTime = transferTime;
            }
            else if (patient.Status == PatientStatus.Leaving)
            {
                transferTime = _ambulanceToEntrancePOFTransferGenerator.NextDouble();

                CreatePatientPath(myMessage, transferTime);

                patient.Status = PatientStatus.Leaving;
                if (transferTime > maxTransferTime) maxTransferTime = transferTime;

            }

            if (myMessage.Nurse != null)
			{
				double nurseTransferTime = _ambulanceTransferGenerator.NextDouble();

				CreateResourcePath(myMessage, nurseTransferTime, myMessage.Nurse);
				if (nurseTransferTime > maxTransferTime) maxTransferTime = nurseTransferTime;
            }
			if (myMessage.Doctor != null)
			{
				double doctorTransferTime = _ambulanceTransferGenerator.NextDouble();

                CreateResourcePath(myMessage, doctorTransferTime, null,  myMessage.Doctor);
				if (doctorTransferTime > maxTransferTime) maxTransferTime = doctorTransferTime;
            }
            Hold(maxTransferTime, message);
        }

        private void CreateResourcePath(MyMessage myMessage, double transferTime, Nurse? nurse = null, Doctor? doctor = null)
        {
			//PointF[] path = null;

            PointF targetAmbulancePos = new PointF(370, 170);
            if (myMessage.AmbulanceA != null)
            {
                targetAmbulancePos = myMessage.AmbulanceA.CenterPosition;
            }
            else if (myMessage.AmbulanceB != null)
            {
                targetAmbulancePos = myMessage.AmbulanceB.CenterPosition;
            }

            if (nurse != null)
            {
                List<PointF> path = new List<PointF>();
                PointF currentPos = nurse.AnimItem.GetPosition(MySim.CurrentTime);
                path.Add(currentPos);

                if (nurse.AtEntrance)
                {
                    path.Add(new PointF(370, currentPos.Y));
                    path.Add(new PointF(370, 170));
                }
                else
                {
                    path.Add(new PointF(currentPos.X, 170));
                }

                path.Add(targetAmbulancePos);

                nurse.AnimTransfer(MySim.CurrentTime, transferTime, path.ToArray());
                nurse.AtEntrance = false;
            }
            if (doctor != null)
            {
                List<PointF> path = new List<PointF>();
                PointF currentPos = doctor.AnimItem.GetPosition(MySim.CurrentTime);
                path.Add(currentPos);

                if (doctor.AtEntrance)
                {
                    path.Add(new PointF(370, currentPos.Y));
                    path.Add(new PointF(370, 170));
                }
                else
                {
                    path.Add(new PointF(currentPos.X, 170));
                }

                path.Add(targetAmbulancePos);

                doctor.AnimTransfer(MySim.CurrentTime, transferTime, path.ToArray());
                doctor.AtEntrance = false;
            }
        }

        private void CreatePatientPath(MyMessage myMessage, double transferTime)
		{
			Patient patient = myMessage.Patient;

			PointF[] transferPath = null;

			if (patient.Status == PatientStatus.Arrived)
            {
                transferPath = PatientArrivedPath(patient);
            }
            else if (patient.Status == PatientStatus.WaitingForInitialExamination ||
                     patient.Status == PatientStatus.WaitingForExamination)
            {
                transferPath = PatientBetweenAmbulancesPath(myMessage);
            }
            else if (patient.Status == PatientStatus.Leaving)
            {
                transferPath = PatientLeavingPath(patient);

            }
            patient.AnimTransfer(MySim.CurrentTime, transferTime, transferPath);
        }

        private PointF[] PatientLeavingPath(Patient patient)
        {
            PointF[] transferPath = new PointF[]
            {
                new PointF(patient.AnimItem.GetPosition(MySim.CurrentTime).X, 170),
                new PointF(800, 170)
            };
            if (patient.AmbulanceA != null)
            {
                transferPath.Append(new PointF(patient.AmbulanceA.CenterPosition.X, patient.AmbulanceA.CenterPosition.Y));
            }
            else if (patient.AmbulanceB != null)
            {
                transferPath.Append(new PointF(patient.AmbulanceB.CenterPosition.X, patient.AmbulanceB.CenterPosition.Y));
            }

            return transferPath;
        }

        private PointF[] PatientBetweenAmbulancesPath(MyMessage myMessage)
        {
            Patient patient = myMessage.Patient;
            List<PointF> transferPath = new List<PointF>();

            PointF currentPos = patient.AnimItem.GetPosition(MySim.CurrentTime);
            transferPath.Add(currentPos);

            transferPath.Add(new PointF(300, currentPos.Y));
            transferPath.Add(new PointF(300, 170));

            if (myMessage.AmbulanceA != null)
            {
                transferPath.Add(myMessage.AmbulanceA.CenterPosition);
            }
            else if (myMessage.AmbulanceB != null)
            {
                transferPath.Add(myMessage.AmbulanceB.CenterPosition);
            }

            return transferPath.ToArray();
        }

        private PointF[] PatientArrivedPath(Patient patient)
        {
            PointF[] transferPath;
            if (patient.Priority == 0)
            {
                transferPath = new PointF[]
                {
                    new PointF(50, 70),
                    new PointF(200, 70)
                };
            }
            else
            {
                transferPath = new PointF[]
                {
                    new PointF(50, 50),
                    new PointF(200, 50)
                };
            }

            return transferPath;
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
                
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
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