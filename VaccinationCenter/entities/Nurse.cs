using System;
using System.Diagnostics;
using OSPABA;
using OSPRNG;

namespace VaccinationCenter.entities {
	public class Nurse : ServiceEntity {

		public static readonly int MaxDoses = 20;

		public Nurse(Simulation mySim) : base(mySim) {
			ServiceDurationGen = new TriangularRNG(20, 75, 100);
			ServiceType = ServiceType.Nurse;
			Init();
		}

		private void Init() {
			Doses = MaxDoses;
			StartOfMovingToRefill = InvalidValue;
		}

		public override void Reset() {
			base.Reset();
			Init();
		}

		public int Doses { get; set; }

		public double StartOfWaiting { get; private set; }

		public double StartOfMovingToRefill { get; private set; }

		public bool HasFullDoses() {
			return Doses == MaxDoses;
		}

		public override void Free() {
			Debug.Assert(((ServiceStatus == ServiceStatus.Occupied) || (ServiceStatus == ServiceStatus.MoveFromRefill)),
				"Cannot free nurse that is not occupied");
			if (ServiceStatus == ServiceStatus.Occupied) {
				base.Free();
			}
			else { // service moves from refill, I need to update occupancy time with entire duration of refill
				Debug.Assert(ServiceStatus == ServiceStatus.MoveFromRefill);
				ServiceStatus = ServiceStatus.Free;
				Debug.Assert(StartOfMovingToRefill != InvalidValue, "Start of moving to refill was already used, It is invalid!");
				double duration = MySim.CurrentTime - StartOfMovingToRefill;
				ServiceStat.AddServiceOccupancy(duration);
				StartOfMovingToRefill = InvalidValue;
			}
		}

		public void StartMoveToRefill() {
			Debug.Assert(ServiceStatus == ServiceStatus.Free);
			ServiceStatus = ServiceStatus.MoveToRefill;
			StartOfMovingToRefill = MySim.CurrentTime;
		}

		public void StartWaitingForRefill() {
			Debug.Assert(ServiceStatus == ServiceStatus.MoveToRefill);
			ServiceStatus = ServiceStatus.WaitingForRefill;
			StartOfWaiting = MySim.CurrentTime;
		}

		public void StartRefilling() {
			Debug.Assert(ServiceStatus == ServiceStatus.WaitingForRefill);
			ServiceStatus = ServiceStatus.Refilling;
		}

		public void StartMoveFromRefill() {
			Debug.Assert(ServiceStatus == ServiceStatus.Refilling);
			ServiceStatus = ServiceStatus.MoveFromRefill;
		}
	}
}
