namespace Core
{
    public class SimulationCore
    {
        public const int NUMBEROFPOINTS = 1000; 
        public int Replications { get; set; }
        public bool Stopped { get; set; }
        public bool Paused { get; set; }
        public int ActualReplication { get; set; }

        public virtual void BeforeSimulation() { }
        public virtual void AfterSimulation() { }

        public virtual void BeforeReplication() { }
        public virtual void AfterReplication() { }
        public virtual void OneReplication() { }

        public virtual void Run()
        {
            ActualReplication = 0;
            Stopped = false;
            BeforeSimulation();
            while (ActualReplication < Replications)
            {
                if (Stopped)
                {
                    break;
                }
                BeforeReplication();
                OneReplication();
                ActualReplication++;
                AfterReplication();
            }
            AfterSimulation();
        }
    }
}
