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
			//SimParameter simParameter = new SimParameter() {
			//	NumOfAdminWorkers = 23,
			//	NumOfDoctors = 20,
			//	NumOfNurses = 14,
			//	NumOfPatients = 2500,
			//};
			//MySimulation simulation = new MySimulation();
			//simulation.SimParameter = simParameter;
			//int replication = 10000;
			//double endTime = double.MaxValue;
			//simulation.Simulate(replication, endTime);
			Random seeder = new Random(1);
			RNG<double>.SetSeedGen(seeder);
			RNG<double> generator = new UniformContinuousRNG(10, 20);
			for (int i = 0; i < 100; i++) {
				Console.WriteLine(generator.Sample());
			}

			Console.Read();

		}
	}
}
