using OSPABA;
using Simulation;
using Core.AgentOrientedSimulation.Models;
using Core.AgentOrientedSimulation.Models.IDGen;
using Core.EventSimulation.Statistics;

namespace Agents.AgentNurse
{
	//meta! id="14"
	public class AgentNurse : OSPABA.Agent
	{
        public List<Nurse> Nurses { get; set; } = new();
        public PriorityQueue<MyMessage, (int, double)> Queue { get; set; } = new();
        public WeightedStatistic BusyNurseStat { get; set; } = new();
		private double _lastUpdateNurses = 0.0;
        public AgentNurse(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
			
        }

		override public void PrepareReplication()
		{
            Nurses.Clear();
            Queue.Clear();
            BusyNurseStat.ClearValues();
			_lastUpdateNurses = 0.0;

            base.PrepareReplication();
            // Setup component for the next replication
            for (int i = 0; i < ((MySimulation)MySim).CountOfNurses; i++)
            {
				Nurse nurse = new Nurse(MySim);
                Nurses.Add(nurse);
            }
        }

        public void RecordNurseStat()
        {
            double total = ((MySimulation)MySim).CountOfNurses;
            double busyCount = total - Nurses.Count;
            double timeDelta = MySim.CurrentTime - _lastUpdateNurses;

            if (timeDelta > 0)
            {
                BusyNurseStat.AddValue(busyCount, timeDelta);
            }
            _lastUpdateNurses = MySim.CurrentTime;
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
		{
			new ManagerNurse(SimId.ManagerNurse, MySim, this);
			AddOwnMessage(Mc.RequestNurseAllocation);
			AddOwnMessage(Mc.FreeNurse);
		}
		//meta! tag="end"
	}
}