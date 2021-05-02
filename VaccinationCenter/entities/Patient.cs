using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;

namespace VaccinationCenter.entities {
	public class Patient : Entity {

		private static int _lastPatientId = 0;

		public Patient(Simulation mySim) : base(mySim) {
			StartOfWaiting = new Dictionary<ServiceType, double>() {
				[ServiceType.AdminWorker] = 0.0,
				[ServiceType.Doctor] = 0.0,
				[ServiceType.Nurse] = 0.0,
			};
			PatientId = _lastPatientId;
			_lastPatientId++;
		}

		public static void ResetPatientId() {
			_lastPatientId = 0;
		}

		public int PatientId { get; }

		public Dictionary<ServiceType, double> StartOfWaiting { get; set; }

		public ServiceType LastVisitedService { get; set; }

		public double ArrivalTime { get; set; }
	}
}
