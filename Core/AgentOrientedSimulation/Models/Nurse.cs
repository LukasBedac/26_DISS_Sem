using Core.AgentOrientedSimulation.Models.IDGen;
using Core.EventSimulation.Models;
using OSPAnimator;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Core.AgentOrientedSimulation.Models
{
    public class Nurse : Entity
    {
        public int ID { get; set; }
        public Patient? AssignedPatient { get; set; }
        public AmbulanceA? AmbulanceA { get; set; } = null;
        public AmbulanceB? AmbulanceB { get; set; } = null;
        public AnimItem AnimItem { get; set; }
        public bool AtEntrance { get; set; } = true;
        public Nurse(OSPABA.Simulation sim)
        {
            ID = IDs.GetNurseID();
            _sim = (MySimulation)sim;
            InitializeEntity();
        }

        public override void InitializeEntity()
        {
            AnimItem = new AnimShapeItem()
            {
                AnimShape = AnimShape.STAR,
                Color = Colors.Green,
                Size = 15
            };

            AnimItem.SetPosition(100 + ((ID - 1) * 20), 170);
            AnimItem.Visible = true;
            _sim.Animator.Register(AnimItem);
        }
    }
}
