using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPRNG;

namespace VaccinationCenter.entities {

	public abstract class ServiceEntity : Entity {

		protected ServiceEntity(Simulation mySim) : base(mySim) {
			ServiceStatus = ServiceStatus.Free;
			StartOfServiceTime = 0;
			ServiceStat = new ServiceStat(mySim);
		}

		public RNG<double> ServiceDurationGen { get; protected set; }

		public ServiceStatus ServiceStatus { get; private set; }

		public ServiceStat ServiceStat { get; }

		private double StartOfServiceTime { get; set; } // time when service start working

		public void Free() {
			Debug.Assert(ServiceStatus == ServiceStatus.Occupied, "Cannot free service that is not occupied");
			ServiceStatus = ServiceStatus.Free;
			double duration = MySim.CurrentTime - StartOfServiceTime;
			ServiceStat.AddServiceOccupancy(duration);
		}

		public void Occupy() {
			Debug.Assert(ServiceStatus == ServiceStatus.Free, "Cannot occupied service that is not free");
			ServiceStatus = ServiceStatus.Occupied;
			StartOfServiceTime = MySim.CurrentTime;
		}
	}
}
