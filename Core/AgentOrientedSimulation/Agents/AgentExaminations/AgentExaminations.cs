using OSPABA;
using Simulation;
using Agents.AgentExaminations.ContinualAssistants;

namespace Agents.AgentExaminations
{
	//meta! id="47"
	public class AgentExaminations : OSPABA.Agent
	{
		public AgentExaminations(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerExaminations(SimId.ManagerExaminations, MySim, this);
			new Examination(SimId.Examination, MySim, this);
			new InitialExamination(SimId.InitialExamination, MySim, this);
			AddOwnMessage(Mc.InitalExamination);
			AddOwnMessage(Mc.Examination);
		}
		//meta! tag="end"
	}
}