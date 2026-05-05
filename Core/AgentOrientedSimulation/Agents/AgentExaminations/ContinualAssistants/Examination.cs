using Agents.AgentExaminations;
using Core.AgentOrientedSimulation.Models;
using Core.Distributions;
using Core.Distributions.Helpers;
using OSPABA;
using OSPRNG;
using Simulation;

namespace Agents.AgentExaminations.ContinualAssistants
{
	//meta! id="54"
	public class Examination : OSPABA.Process
	{

		private Empiric _examinationWalkTimeGenerator;
		private ContinuousUniform _examinationAmbulanceTimeGenerator;
        public Examination(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			List<Scope> intervals = new List<Scope>()
			{
				new Scope(10, 12, 0.1),
				new Scope(12, 14, 0.6),
				new Scope(14, 18, 0.3)
            };
			_examinationWalkTimeGenerator = new Empiric(false, intervals);
			_examinationAmbulanceTimeGenerator = new ContinuousUniform(15, 30);
        }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentExaminations", id="55", type="Start"
		public void ProcessStart(MessageForm message)
		{
			MyMessage myMessage = (MyMessage)message;
			double examTime = 0;
            if (myMessage.Patient.Priority == 0)
			{
				examTime = _examinationAmbulanceTimeGenerator.NextDouble();
            } else
			{
				examTime = _examinationWalkTimeGenerator.NextDouble();
            }
			Hold(examTime, myMessage);
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