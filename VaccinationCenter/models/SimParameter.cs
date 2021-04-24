using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccinationCenter.entities;

namespace VaccinationCenter.models {
	public class SimParameter {

		public int NumOfAdminWorkers { get; set; }

		public int NumOfDoctors { get; set; }

		public int NumOfNurses { get; set; }

		public int GetNumberOfServices(ServiceType type) {
			switch (type) {
				case ServiceType.AdminWorker: {
					return NumOfAdminWorkers;
				}
				case ServiceType.Doctor: {
					return NumOfDoctors;
				}
				case ServiceType.Nurse: {
					return NumOfNurses;
				}
				default: {
					Debug.Fail("Invalid type of service.");
					return -1;
				}
			}
		}
	}
}
