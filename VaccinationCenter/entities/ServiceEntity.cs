using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPRNG;

namespace VaccinationCenter.entities {

	public abstract class ServiceEntity : Entity {

		protected ServiceEntity(Simulation mySim) : base(mySim) {
			ServiceStatus = ServiceStatus.Free;
		}

		public RNG<double> ServiceDurationGen { get; protected set; }

		public ServiceStatus ServiceStatus { get; set; }
	}
}
