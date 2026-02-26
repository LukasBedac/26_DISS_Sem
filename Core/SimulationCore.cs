namespace Core
{
    public class SimulationCore
    {
        public int Replications { get; set; }
        public bool Stopped { get; set; }
        public bool Paused { get; set; }
        public int ActualReplicationCount { get; set; }

        public virtual void BeforeSimulation() { }
        public virtual void AfterSimulation() { }

        public virtual void BeforeReplication() { }
        public virtual void AfterReplication() { }
        public virtual void OneReplication() { }

        public virtual void Run()
        {
            ActualReplicationCount = 0;
            Stopped = false;
            BeforeSimulation();
            while (ActualReplicationCount <= Replications)
            {
                if (Stopped)
                {
                    break;
                }
                BeforeReplication();
                OneReplication();
                AfterReplication();
            }
            AfterSimulation();
        }
    }
}
