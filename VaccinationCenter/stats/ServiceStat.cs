using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public ServiceStat(VacCenterSimulation simulation) {
			Init();
			Simulation = simulation;
		}

		private void Init() {
			_durationOfOccupiedService = 0;
			_currentSimulationTime = 0;
			DurationOfLunchBreak = 0;
		}

		private VacCenterSimulation Simulation { get; }

		private double DurationOfLunchBreak { get; set; }

		public void SetDurationOfLunchBreak(double duration) {
			Debug.Assert(DurationOfLunchBreak == 0, "DurationOfLunchBreak should be equals to zero");
			DurationOfLunchBreak = duration;
		}

		public void Reset() {
			Init();
		}

		public void AddServiceOccupancy(double duration) {
			_durationOfOccupiedService += duration;
		}

		/**
		 * Used in slow mode, for updating detailed service occupancy (in the table)
		 */
		public double GetServiceOccupancy() {
			if (Simulation.CurrentTime != VacCenterSimulation.InfinityTime) { // if simulation does not end
				_currentSimulationTime = Simulation.CurrentTime; // if simulation end then I will uses last time value
			}

			return GetServiceOccupancy(_currentSimulationTime);
		}
		
		/**
		 * Used for fast mode at end of each replication
		 */
		public double GetServiceOccupancy(double currentTime) {
			double durationOfSimulation = currentTime;
			if (durationOfSimulation == 0) {
				return 0.0;
			}
			return ((_durationOfOccupiedService / (durationOfSimulation - DurationOfLunchBreak)) * 100);
		}

		/**
		 * For each entity in the detailed table
		 */
		public override string ToString() {
			return GetServiceOccupancy().ToString(CultureInfo.InvariantCulture); //TODO, remove parameter
		}
	}
}
