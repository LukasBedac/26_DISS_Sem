using Core.AgentOrientedSimulation.Models;
using OSPABA;

namespace Simulation
{
	public class MyMessage : OSPABA.MessageForm
	{
        public Patient Patient { get; set; }
        public Nurse Nurse { get; set; }
        public Doctor Doctor { get; set; }
        public AmbulanceA AmbulanceA { get; set; }
		public AmbulanceB AmbulanceB { get; set; }
        public bool IsFreeAmbulanceA { get; set; }
        public bool IsFreeAmbulanceB { get; set; }
        public MyMessage(OSPABA.Simulation mySim) :
			base(mySim)
		{
		}

		public MyMessage(MyMessage original) :
			base(original)
		{
			// copy() is called in superclass
		}

		override public MessageForm CreateCopy()
		{
			return new MyMessage(this);
		}

		override protected void Copy(MessageForm message)
		{
			base.Copy(message);
			MyMessage original = (MyMessage)message;
			
			// Copy attributes
			Patient = original.Patient;
			Nurse = original.Nurse;
			Doctor = original.Doctor;
			AmbulanceA = original.AmbulanceA;
			AmbulanceB = original.AmbulanceB;
        }
	}
}