using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPRNG;

namespace VaccinationCenter.entities {
	public class AdminWorker : ServiceEntity {

		public AdminWorker(Simulation mySim) : base(mySim) {
			//ServiceDurationGen = new UniformContinuousRNG(140, 220);
			ServiceDurationGen = new ExponentialRNG(4.0);
		}
	}
}
