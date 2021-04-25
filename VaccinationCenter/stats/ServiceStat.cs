using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using VaccinationCenter.stats;

namespace VaccinationCenter.entities {
	public class ServiceStat : Resettable {

		private double _durationOfOccupiedService;

		public ServiceStat() {
			_durationOfOccupiedService = 0;
		}

		public void Reset() {
			_durationOfOccupiedService = 0;
		}

		public void AddServiceOccupancy(double duration) {
			_durationOfOccupiedService += duration;
		}

		public double GetServiceOccupancy(double currentTime) {
			double durationOfSimulation = currentTime;
			if (durationOfSimulation == 0) {
				return 0.0;
			}
			return (_durationOfOccupiedService / durationOfSimulation) * 100;
		}
	}
}
