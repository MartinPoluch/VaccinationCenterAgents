using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agents;
using OSPRNG;
using simulation;
using VaccinationCenter.models;

namespace VaccinationCenter {
	class Program {
		static void Main(string[] args) {
			SimParameter simParameter = new SimParameter() {
				NumOfAdminWorkers = 5,
				NumOfDoctors = 6,
				NumOfNurses = 3,
				NumOfPatients = 540,
			};
			MySimulation simulation = new MySimulation(simParameter);
			int replication = 10000;
			double endTime = 9 * 60 * 60;
			simulation.Simulate(replication, endTime);
			Console.Read();

		}
	}
}
