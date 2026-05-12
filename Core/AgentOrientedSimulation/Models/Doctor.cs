using Core.AgentOrientedSimulation.Models.IDGen;
using Core.EventSimulation.Models;
using OSPAnimator;
using Simulation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Core.AgentOrientedSimulation.Models
{
    public class Doctor : Entity
    {
        public int ID { get; set; }
        public Patient? AssignedPatient { get; set; }
        public AmbulanceA? AmbulanceA { get; set; } = null;
        public AmbulanceB? AmbulanceB { get; set; } = null;
        public AnimItem AnimItem { get; set; }
        public bool AtEntrance { get; set; } = true;
        public Doctor(OSPABA.Simulation sim)
        {
            ID = IDs.GetDoctorID();
            _sim = (MySimulation)sim;
            InitializeEntity();
        }

        public override void InitializeEntity()
        {
            AnimItem = new AnimShapeItem()
            {
                AnimShape = AnimShape.CROSS,
                Color = Colors.Blue,
                Size = 15,
                ZIndex = 2
            };

            AnimItem.SetPosition(100 + ((ID - 1) * 20), 130);
            AnimItem.Visible = true;
            if(_sim.AnimatorExists) _sim.Animator.Register(AnimItem);
        }

        public override void AnimTransfer(double currentTime, double duration, PointF[] path)
        {
            AnimItem.StartAnim(currentTime, duration, path);
        }
    }
}
