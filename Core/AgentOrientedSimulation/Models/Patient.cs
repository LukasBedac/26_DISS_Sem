using Core.AgentOrientedSimulation.Models.Enums;
using Core.AgentOrientedSimulation.Models.IDGen;
using Core.EventSimulation.Models;
using OSPABA;
using OSPAnimator;
using Simulation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Entity = Core.EventSimulation.Models.Entity;

namespace Core.AgentOrientedSimulation.Models
{
    public class Patient : Entity
    {
        public int ID { get; set; }
        public int Priority { get; set; }
        public bool CameByAmbulance { get; set; }
        public PatientStatus Status { get; set; }
        public AmbulanceA? AmbulanceA { get; set; } = null;
        public AmbulanceB? AmbulanceB { get; set; } = null;
        public AnimItem AnimItem { get; set; }
        public double StartTimeFromEntranceToExam { get; set; }
        public double EndTimeFromEntranceToExam { get; set; } = 0.0;
        public Patient(OSPABA.Simulation sim, int priority, double arrivalTime)
        {
            _sim = (MySimulation)sim;
            ID = IDs.GetPatientID();
            Priority = priority;
            ArrivalTime = arrivalTime;
            StartTimeFromEntranceToExam = arrivalTime;
            CameByAmbulance = (priority == 0);
            InitializeEntity();
        }

        public override void InitializeEntity()
        {
            Status = PatientStatus.Arrived;
            AnimItem = new AnimShapeItem()
            {
                AnimShape = AnimShape.CIRCLE,
                Color = Priority == 0 ? Colors.Red : Colors.Green,
                Size = 15,
                ZIndex = 2
            };

            AnimItem.SetPosition(0, 0);
            if (_sim.AnimatorExists) _sim.Animator.Register(AnimItem);
        }
            
        public override void AnimTransfer(double currentTime, double duration, PointF[] path)
        {
            AnimItem.StartAnim(currentTime, duration, path);
        }
    }
}
