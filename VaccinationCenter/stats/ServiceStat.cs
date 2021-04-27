using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using simulation;
using VaccinationCenter.stats;

namespace VaccinationCenter.entities {
	public class ServiceStat : Resettable {

		private double _durationOfOccupiedService;
		private double _currentSimulationTime;

		public ServiceStat(MySimulation simulation) {
			_durationOfOccupiedService = 0;
			_currentSimulationTime = 0;
			Simulation = simulation;
		}

		private MySimulation Simulation { get; }

		public void Reset() {
			_durationOfOccupiedService = 0;
			_currentSimulationTime = 0;
		}

		public void AddServiceOccupancy(double duration) {
			_durationOfOccupiedService += duration;
		}

		public double GetServiceOccupancy(double currentTime) {
			//TODO, do not include lunch duration
			if (Simulation.CurrentTime != MySimulation.InfinityTime) { // if simulation does not end
				_currentSimulationTime = Simulation.CurrentTime;
			}
			
			double durationOfSimulation = _currentSimulationTime;
			if (durationOfSimulation == 0) {
				return 0.0;
			}
			return Math.Round(((_durationOfOccupiedService / durationOfSimulation) * 100), 2);
		}

		public override string ToString() {
			return GetServiceOccupancy(0).ToString(CultureInfo.InvariantCulture); //TODO, remove parameter
		}
	}
}
