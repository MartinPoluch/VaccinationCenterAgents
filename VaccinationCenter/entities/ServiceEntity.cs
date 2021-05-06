using System;
using System.Diagnostics;
using OSPABA;
using OSPRNG;
using simulation;
using VaccinationCenter.stats;

namespace VaccinationCenter.entities {

	public abstract class ServiceEntity : Entity, Resettable {

		protected static readonly double InvalidValue = Double.MinValue; // for debugging purposes, used for time values

		protected ServiceEntity(Simulation mySim) : base(mySim) {
			Init();
			ServiceStat = new ServiceStat((VacCenterSimulation)mySim);
		}

		private void Init() {
			ServiceStatus = ServiceStatus.Free;
			LunchStatus = LunchStatus.TooEarly;
			StartOfServiceTime = InvalidValue;
			StartOfLunchBreak = InvalidValue;
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

		public LunchStatus LunchStatus { get; protected set; }

		public ServiceStat ServiceStat { get; }

		private double StartOfServiceTime { get; set; } // time when service start working

		public double StartOfLunchBreak { get; set; } // time when service went to lunch

		public virtual void Free() {
			Debug.Assert((ServiceStatus == ServiceStatus.Occupied), "Cannot free service that is not occupied");
			ServiceStatus = ServiceStatus.Free;
			Debug.Assert(StartOfServiceTime != InvalidValue, "Start of service time is invalid!");
			double duration = MySim.CurrentTime - StartOfServiceTime;
			ServiceStat.AddServiceOccupancy(duration);
			StartOfServiceTime = InvalidValue; // value was alreadt
		}

		public void Occupy() {
			Debug.Assert(ServiceStatus == ServiceStatus.Free, "Cannot occupied service that is not free");
			ServiceStatus = ServiceStatus.Occupied;
			StartOfServiceTime = MySim.CurrentTime;
		}

		public virtual void StartLunchBreak() {
			Debug.Assert(ServiceStatus == ServiceStatus.Free, "Only free entity can start lunch break");
			ServiceStatus = ServiceStatus.Lunch;
			Debug.Assert(StartOfLunchBreak == InvalidValue, "StartOfLunchBreak should not be initialized yet");
			StartOfLunchBreak = MySim.CurrentTime;
		}

		public void StartBeingHungry() {
			Debug.Assert(LunchStatus == LunchStatus.TooEarly, "Cannot start being hungry");
			LunchStatus = LunchStatus.Hungry;
		}

		public void StartMoveToLunch() {
			Debug.Assert((ServiceStatus == ServiceStatus.Lunch) && (LunchStatus == LunchStatus.Hungry),
				"Cannot start move to lunch");
			LunchStatus = LunchStatus.MoveTo;
		}

		public void StartEating() {
			Debug.Assert((ServiceStatus == ServiceStatus.Lunch) && (LunchStatus == LunchStatus.MoveTo),
				"Cannot start eating");
			LunchStatus = LunchStatus.Eating;
		}

		public void StartMoveFromLunch() {
			Debug.Assert((ServiceStatus == ServiceStatus.Lunch) && (LunchStatus == LunchStatus.Eating),
				"Cannot start moving from lunch");
			LunchStatus = LunchStatus.MoveFrom;
		}

		public void EndLunchBreak() {
			Debug.Assert((ServiceStatus == ServiceStatus.Lunch) && (LunchStatus == LunchStatus.MoveFrom),
				"Cannot end lunch break");
			LunchStatus = LunchStatus.Full;
			ServiceStatus = ServiceStatus.Free;
			Debug.Assert(StartOfLunchBreak != InvalidValue, "StartOfLunchBreak has invalid value");
			double durationOfLunch =  MySim.CurrentTime - StartOfLunchBreak;
			ServiceStat.SetDurationOfLunchBreak(durationOfLunch);
			StartOfLunchBreak = InvalidValue;
		}
	}
}
