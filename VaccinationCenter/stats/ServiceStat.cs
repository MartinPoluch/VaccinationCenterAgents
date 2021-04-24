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

		public ServiceStat(Simulation simulation) {
			Simulation = simulation;
			_durationOfOccupiedService = 0;
		}

		public void Reset() {
			_durationOfOccupiedService = 0;
		}

		public Simulation Simulation { get; }

		public void AddServiceOccupancy(double duration) {
			_durationOfOccupiedService += duration;
		}

		public double GetServiceOccupancy() {
			double durationOfSimulation = Simulation.CurrentTime;
			if (durationOfSimulation == 0) {
				return 0.0;
			}
			return (_durationOfOccupiedService / durationOfSimulation) * 100;
		}

		public override string ToString() {
			return GetServiceOccupancy().ToString();
		}
	}
}
