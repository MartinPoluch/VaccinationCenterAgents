using System;
using OSPABA;
using VaccinationCenter.entities;

namespace simulation
{
	public class MyMessage : MessageForm
	{
		public MyMessage(Simulation sim) :
			base(sim)
		{
		}

		public MyMessage(MyMessage original) :
			base(original)
		{
			// copy() is called in superclass
		}

		public String DebugText { get; set; }

		public ServiceEntity Service { get; set; }

		public Patient Patient { get; set; }

		/**
		 * Casting for vaccination manager
		 */
		public Nurse GetNurse() {
			return (Nurse)Service;
		}

		public override MessageForm CreateCopy() {
			return new MyMessage(this) {
				Service = Service,
				Patient = Patient,
			};
		}

		protected override void Copy(MessageForm message) {
			base.Copy(message);
			MyMessage original = (MyMessage)message;
			Service = original.Service;
			Patient = original.Patient;
		}
	}
}