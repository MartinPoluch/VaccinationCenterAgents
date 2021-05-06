using System;
using OSPABA;
using VaccinationCenter.entities;

namespace simulation
{
	public class Message : MessageForm
	{
		public Message(Simulation sim) :
			base(sim)
		{
		}

		public Message(Message original) :
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
			return new Message(this) {
				Service = Service,
				Patient = Patient,
			};
		}

		protected override void Copy(MessageForm message) {
			base.Copy(message);
			Message original = (Message)message;
			Service = original.Service;
			Patient = original.Patient;
		}
	}
}