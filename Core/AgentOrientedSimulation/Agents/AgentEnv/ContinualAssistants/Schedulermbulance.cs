using Agents.AgentEnv;
using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.Enums;
using Core.AgentOrientedSimulation.Models.IDGen;
using Core.Distributions;
using OSPABA;
using OSPAnimator;
using Simulation;
using System.Windows.Media;

namespace Agents.AgentEnv.ContinualAssistants
{
	//meta! id="6"
	public class Schedulermbulance : OSPABA.Scheduler
	{
		private Erlang _newArrival;
        private int _arrivalsCounter = 0;
        public Schedulermbulance(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
            _newArrival = new Erlang(7, 0.836);
			//_newArrival = new Erlang(2, 0.2 * 60);
		}

        override public void PrepareReplication()
		{
			base.PrepareReplication();
            // Setup component for the next replication
            _arrivalsCounter = 0;
		}

		//meta! sender="AgentEnv", id="7", type="Start"
		public void ProcessStart(MessageForm message)
		{
			message.Code = Mc.PatientArrival;
			Hold(_newArrival.NextDouble(), message);
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.PatientArrival:
                    MessageForm newMessage = message.CreateCopy();
                    Hold(_newArrival.NextDouble(), newMessage);

                    Patient patient = CreatePatient();
                    MyMessage myMessage = (MyMessage)message;
                    myMessage.Patient = patient;
                    myMessage.Addressee = MyAgent;
                    Notice(myMessage);
                    break;
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
		public new AgentEnv MyAgent
		{
			get
			{
				return (AgentEnv)base.MyAgent;
			}
		}

        public Patient CreatePatient()
        {
			Patient patient = new Patient(MySim, 0, MySim.CurrentTime);
			return patient;
        }
    }
}