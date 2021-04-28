using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			StartOfWaiting = 0;
		}

		public override void Reset() {
			base.Reset();
			Init();
		}

		public int Doses { get; set; }

		public double StartOfWaiting { get; private set; }

		public bool HasFullDoses() {
			return Doses == MaxDoses;
		}

		public void StartMoveToRefill() {
			Debug.Assert(ServiceStatus == ServiceStatus.Free);
			ServiceStatus = ServiceStatus.MoveToRefill;
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
