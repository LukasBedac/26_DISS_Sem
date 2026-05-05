using Agents.AgentExaminations;
using Core.AgentOrientedSimulation.Models;
using Core.Distributions;
using Core.Distributions.Helpers;
using OSPABA;
using Simulation;

namespace Agents.AgentExaminations.ContinualAssistants
{
	//meta! id="52"
	public class InitialExamination : OSPABA.Process
	{
		private Empiric _initialExaminationWalkTimeGenerator;
		private DiscreteUniform _initialExaminationAmbulanceGenerator;
        private Random _random = new Random(SeedGen.GetSeed());

        public InitialExamination(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			List<Scope> intervals = new List<Scope>()
			{
				new Scope(3, 5, 0.6),
				new Scope(5, 9, 0.4)
			};
            _initialExaminationWalkTimeGenerator = new Empiric(false, intervals);
			_initialExaminationAmbulanceGenerator = new DiscreteUniform(4, 9);
        }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentExaminations", id="53", type="Start"
		public void ProcessStart(MessageForm message)
		{
            MyMessage myMessage = (MyMessage)message;
            Patient patient = myMessage.Patient;
            double examinationTime = 0;

            if (patient.Priority == 6)
            {
                examinationTime = _initialExaminationWalkTimeGenerator.NextDouble();
                patient.Priority = DecidePriorioty(false);
            }
            else
            {
                examinationTime = _initialExaminationAmbulanceGenerator.NextInt();
                patient.Priority = DecidePriorioty(true);
            }
            Hold(examinationTime, myMessage);
        }

        private int DecidePriorioty(bool arriveByAmbulance)
        {
            double randomValue = _random.NextDouble();
            if (arriveByAmbulance)
            {
                if (randomValue < 0.3) return 1;
                else if (randomValue < 0.55) return 2;
                else if (randomValue < 0.75) return 3;
                else if (randomValue < 0.90) return 4;
                else return 5;
            }
            else
            {
                if (randomValue < 0.1) return 1;
                else if (randomValue < 0.3) return 2;
                else if (randomValue < 0.45) return 3;
                else if (randomValue < 0.7) return 4;
                else return 5;
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
		public new AgentExaminations MyAgent
		{
			get
			{
				return (AgentExaminations)base.MyAgent;
			}
		}
	}
}