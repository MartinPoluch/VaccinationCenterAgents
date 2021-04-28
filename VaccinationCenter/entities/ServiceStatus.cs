using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationCenter.entities {
	public enum ServiceStatus {
		Occupied,
		Free,
		Lunch,

		/**
		 * Only for Nurses
		 * unfortunately I cannot extend enum
		 */
		MoveToRefill,
		WaitingForRefill,
		Refilling,
		MoveFromRefill,
	}
}
