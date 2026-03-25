using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Statistics
{
    public class StatQueue<T>
    {
        private EventCore _core;
        private WeightedStatistic _weightedStatistic = new WeightedStatistic();
        private double _lastChangeInQueue = 0.0;
        public Queue<T> Queue { get; set; } = new Queue<T>();
        public double AverageQueueLength => _weightedStatistic.Mean;

        public StatQueue(EventCore core)
        {
            _core = core;
            _lastChangeInQueue = _core.CurrentTime;
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
            Queue.Clear();
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
    }
}
