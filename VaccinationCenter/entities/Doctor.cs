using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPRNG;

namespace VaccinationCenter.entities {
	public class Doctor : ServiceEntity {
		public Doctor(Simulation mySim) : base(mySim) {
			ServiceDurationGen = new ExponentialRNG(260.0);
			ServiceType = ServiceType.Doctor;
		}
	}
}
