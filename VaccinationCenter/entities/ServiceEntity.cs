using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPRNG;
using simulation;
using VaccinationCenter.stats;

namespace VaccinationCenter.entities {

	public abstract class ServiceEntity : Entity, Resettable {

		protected static readonly double InvalidValue = Double.MinValue; // for debugging purposes, used for time values

		protected ServiceEntity(Simulation mySim) : base(mySim) {
			Init();
			ServiceStat = new ServiceStat((MySimulation)mySim);
		}

		private void Init() {
			ServiceStatus = ServiceStatus.Free;
			LunchStatus = LunchStatus.TooEarly;
			StartOfServiceTime = InvalidValue;
		}

		public virtual void Reset() {
			Init();
			ServiceStat.Reset();
		}

		protected RNG<double> ServiceDurationGen { get; set; }

		public double GenerateDuration() {
			return ServiceDurationGen.Sample();
		}

		public ServiceType ServiceType { get; protected set; }

		public ServiceStatus ServiceStatus { get; protected set; }

		public LunchStatus LunchStatus { get; set; }

		public ServiceStat ServiceStat { get; }

		private double StartOfServiceTime { get; set; } // time when service start working

		public virtual void Free() {
			Debug.Assert((ServiceStatus == ServiceStatus.Occupied), "Cannot free service that is not occupied");
			ServiceStatus = ServiceStatus.Free;
			Debug.Assert(StartOfServiceTime != InvalidValue, "Start of service time is invalid!");
			double duration = MySim.CurrentTime - StartOfServiceTime;
			ServiceStat.AddServiceOccupancy(duration);
			StartOfServiceTime = InvalidValue; // value was already used, now it is invalid
		}

		public void Occupy() {
			Debug.Assert(ServiceStatus == ServiceStatus.Free, "Cannot occupied service that is not free");
			ServiceStatus = ServiceStatus.Occupied;
			StartOfServiceTime = MySim.CurrentTime;
		}
	}
}
