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
        public double CurrentTime { get; set; } = 0.0;
        public double EndTime { get; set; } = 0.0;
        /// <summary>
        /// Event, where priority is time of the event
        /// </summary>
        protected PriorityQueue<Event, double> Events { get; set; } = new PriorityQueue<Event, double>();

        public Event ActualEvent { get; set; }

        public EventCore(int replications) : base()
        {
            Replications = replications;
        }

        public override void OneReplication()
        {
            while (Events.Count > 0 && !Stopped && CurrentTime < EndTime)
            {
                //pause here

                ActualEvent = Events.Dequeue();
                CurrentTime = ActualEvent.Time;
                ActualEvent.Execute();
            }
            ActualReplication++;
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

        public override void BeforeSimulation()
        {
            //First customer here
        }

        public override void BeforeReplication()
        {
            CurrentTime = 0.0;
        }
        public override void AfterReplication()
        {
            Events.Clear();
        }
    }
}
