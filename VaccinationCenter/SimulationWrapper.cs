using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simulation;
using VaccinationCenter.models;

namespace VaccinationCenter {
	public class SimulationWrapper {

		private bool _stop;

		public SimulationWrapper() {
			_stop = false;
		}

		public void Stop() {
			if (simulation != null) {
				_stop = true;
				simulation.StopSimulation();
			}
		}

		public void Pause() {
			simulation?.PauseSimulation();
		}

		public void Continue() {
			simulation?.ResumeSimulation();
		}

		public MySimulation simulation { get; set; }

		public void Simulate(BackgroundWorker worker, int replications, int numOfPatients, int numOfWorkers,
			int minNumOfDoctors, int maxNumOfDoctors, int numOfNurses, bool validationMode, bool earlyArrivals) {
			_stop = false;
			for (int doctors = minNumOfDoctors; (doctors < maxNumOfDoctors); doctors++) {
				if (_stop) {
					return;
				}

				simulation = new MySimulation {
					SimParameter = new SimParameter() {
						NumOfAdminWorkers = numOfWorkers,
						NumOfDoctors = doctors,
						NumOfNurses = numOfNurses,
						NumOfPatients = numOfPatients,
						ValidationMode = validationMode,
						EarlyArrivals = earlyArrivals,
					}
				};
				simulation.Simulate(replications, MySimulation.InfinityTime);
				worker.ReportProgress(1, simulation);
			}
		}
	}
}
