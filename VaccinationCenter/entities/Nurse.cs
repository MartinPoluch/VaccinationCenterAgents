using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPRNG;

namespace VaccinationCenter.entities {
	public class Nurse : ServiceEntity {

		public Nurse(Simulation mySim) : base(mySim) {
			ServiceDurationGen = new TriangularRNG(20, 75, 100);
			ServiceType = ServiceType.Nurse;
			Doses = 20;
		}

		public int Doses { get; set; }
	}
}
