using Core.EventSimulation.Airport;
using Core.EventSimulation.Airport.Events;
using Core.EventSimulation.Airport.Observer;
using Core.EventSimulation.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation
{
    public class EventCore : SimulationCore
    {
        private List<SimDelegate> _delegates = new();
        public double CurrentTime { get; set; } = 0.0;
        public double EndTime { get; set; } = 86_400; //24 hours in seconds
        public double WarmUpLength { get; set; } = 12 * 3600; //12 hours in seconds

        public bool Paused { get; set; }
        public bool TurboMode { get; set; }
        public int SimulationSpeed { get; set; } = 200;
        public int SleepInterval { get; set; }
        public int SleepMs { get; set; }
        /// <summary>
        /// Event, where priority is time of the event
        /// </summary>
        protected PriorityQueue<Event, double> Events { get; set; } = new PriorityQueue<Event, double>();

        public Event ActualEvent { get; set; }

        public EventCore(int replications) : base()
        {
            Replications = replications;
            EndTime += WarmUpLength;
        }

        public override void OneReplication()
        {
            while (Events.Count > 0 && !Stopped && CurrentTime < EndTime)
            {
                while (Paused)
                {
                    Thread.Sleep(250);
                }

                ActualEvent = Events.Dequeue();
                CurrentTime = ActualEvent.Time;
                ActualEvent.Execute();
            }
        }

        public void PlanEvent(Event _event)
        {
            if (_event == null) throw new Exception("Event is null");
            if (_event.Time < CurrentTime)
            {
                throw new Exception("New event time was lower than actual time");
            } else
            {
                Events.Enqueue(_event, _event.Time);
            }
        }
        protected void StartWatch()
        {
            SleepEvent sleepEvent = new SleepEvent(this, CurrentTime, SleepMs, SleepInterval);
            PlanEvent(sleepEvent);
        }

        public override void BeforeSimulation()
        {
            //First customer here
            
        }

        public override void BeforeReplication()
        {
            CurrentTime = 0.0;
        }

        public void AddDelegates(SimDelegate _delegate)
        {
            _delegates.Add(_delegate);
        }
        public void RefreshGUI()
        {
            foreach (SimDelegate _delegate in _delegates)
            {
                _delegate.Refresh((AirportSimulation)this);
            }
        }
    }
}
