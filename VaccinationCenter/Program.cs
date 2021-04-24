using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPRNG;
using simulation;
using VaccinationCenter.models;

namespace VaccinationCenter {
	class Program {
		static void Main(string[] args) {
			SimParameter simParameter = new SimParameter() {
				NumOfAdminWorkers = 5,
				NumOfDoctors = 5,
				NumOfNurses = 5,
			};
			MySimulation simulation = new MySimulation(simParameter);
			simulation.Simulate(1, 1000000);
			Console.Read();

		}
	}
}
