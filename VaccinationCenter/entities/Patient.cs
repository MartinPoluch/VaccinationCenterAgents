using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;

namespace VaccinationCenter.entities {
	public class Patient : Entity {

		public Patient(Simulation mySim) : base(mySim) {
			StartOfWaiting = new Dictionary<ServiceType, double>() {
				[ServiceType.AdminWorker] = 0.0,
				[ServiceType.Doctor] = 0.0,
				[ServiceType.Nurse] = 0.0,
			};
		}

		public Dictionary<ServiceType, double> StartOfWaiting { get; set; }

	}
}
