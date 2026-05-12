using OSPABA;
using Simulation;
using Agents.AgentEnv;
using Core.Distributions;
using Core.AgentOrientedSimulation.Models;
using Core.Helpers;
using Core.AgentOrientedSimulation.Models.IDGen;
using Core.AgentOrientedSimulation.Models.Enums;
using OSPAnimator;
using System.Drawing;
using System.Windows.Media;

namespace Agents.AgentEnv.ContinualAssistants
{
	//meta! id="8"
	public class SchedulerByFoot : OSPABA.Scheduler
	{
		private Exponential _newArrival;
        public SchedulerByFoot(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
            _newArrival = new Exponential(1 /(9.57 * 60.0));
            //_newArrival = new Exponential(2 * 60);
        } 

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
        }

		//meta! sender="AgentEnv", id="9", type="Start"
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
					AssistantFinished(message);
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
	}
}