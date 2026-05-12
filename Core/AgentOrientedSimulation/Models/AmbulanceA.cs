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
    public class AmbulanceA : Entity
    {
        private int _size = 50;
        private int _gap = 10;
        public int ID { get; set; }
        public Nurse? Nurse { get; set; }
        public Doctor? Doctor { get; set; }
        public Patient? AssignedPatient { get; set; }
        public AnimItem AnimItem { get; set; }
        private AnimTextItem _label;
        public PointF CenterPosition { get; private set; }

        public AmbulanceA(OSPABA.Simulation sim)
        {
            ID = IDs.GetAmbulanceAID();
            _sim = (MySimulation)sim;
            InitializeEntity();
        }

        public override void InitializeEntity()
        {
            AnimItem = new AnimShapeItem(AnimShape.SQUARE, Colors.LightSkyBlue, _size);
            AnimItem.SetPosition(400 + ((ID - 1) * (_size + _gap)), 100);
            AnimItem.Visible = true;
            AnimItem.ZIndex = 1;

            _label = new AnimTextItem($"A{ID}");
            _label.SetPosition(400 + ((ID - 1) * (_size + _gap)) + (_size / 4), 100 - (_size / 4));

            if (_sim.AnimatorExists)
            {
                _sim.Animator.Register(AnimItem);
                _sim.Animator.Register(_label);
            }

            CenterPosition = new PointF(400 + ((ID - 1) * (_size + _gap)) + (_size / 2), 100 + (_size / 2));
        }
    }
}
