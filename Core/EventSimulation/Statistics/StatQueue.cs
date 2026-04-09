using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Statistics
{
    public class StatQueue<T>
    {
        private EventCore _core;
        private WeightedStatistic _weightedStatistic = new WeightedStatistic();
        private Statistic _statistic = new Statistic();
        private double _lastChangeInQueue = 0.0;
        public Queue<T> Queue { get; set; }
        public double AverageQueueLength => _weightedStatistic.Mean;
        public double ReplicationMean => _statistic.Mean;
        public (double, double) ConfidenceInterval90 => _statistic.ConfidenceInterval90();
        public StatQueue(EventCore core, int maxCapacity = 0)
        {
            _core = core;
            _lastChangeInQueue = _core.CurrentTime;
            if (maxCapacity > 0)
            {
                Queue = new Queue<T>(maxCapacity);
            } else
            {
                Queue = new Queue<T>();
            }
        }

        public void Enqueue(T item)
        {
            UpdateStatistics();
            Queue.Enqueue(item);
        }

        public T Dequeue()
        {
            UpdateStatistics();
            return Queue.Dequeue();
        }

        public void Clear()
        {
            //Queue.Clear();
            _weightedStatistic.ClearValues();
            _lastChangeInQueue = 0.0;
        }
        private void UpdateStatistics()
        {
            double timeInterval = _core.CurrentTime - _lastChangeInQueue;
            int queueLength = Queue.Count;

            _weightedStatistic.AddValue(queueLength, timeInterval);
            _lastChangeInQueue = _core.CurrentTime;
        }

        public bool IsFull()
        {
            return Queue.Count >= Queue.Capacity;
        }

        public List<T> GetAll()
        {
            return Queue.ToList();
        }
    }
}
