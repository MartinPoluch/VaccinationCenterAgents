using System.Collections.Generic;
using System.Windows.Controls;
using simulation;
using VaccinationCenter.entities;

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for VariantsTable.xaml
	/// </summary>
	public partial class VariantsTable : UserControl, ResettableOutput {
		public VariantsTable() {
			InitializeComponent();
			SimulationResults = new List<SimulationResult>();
			DataContext = this;
		}

		private double GetOccupancy(MySimulation simulation, ServiceType serviceType) {
			return simulation.ServiceAgentStats[serviceType].Occupancy.Mean();
		}

		private double GetWaitingTime(MySimulation simulation, ServiceType serviceType) {
			return simulation.ServiceAgentStats[serviceType].WaitingTimes.Mean();
		}

		public List<SimulationResult> SimulationResults { get; set; }

		public void Refresh(MySimulation simulation) {
			SimulationResult result = new SimulationResult() {
				Workers = simulation.SimParameter.NumOfAdminWorkers,
				Doctors = simulation.SimParameter.NumOfDoctors,
				Nurses = simulation.SimParameter.NumOfNurses,
				WorkersOccupancy = GetOccupancy(simulation, ServiceType.AdminWorker),
				DoctorsOccupancy = GetOccupancy(simulation, ServiceType.Doctor),
				NursesOccupancy = GetOccupancy(simulation, ServiceType.Nurse),
				RegistrationWaitingTime = GetWaitingTime(simulation, ServiceType.AdminWorker),
				ExaminationWaitingTime = GetWaitingTime(simulation, ServiceType.Doctor),
				VaccinationWaitingTime = GetWaitingTime(simulation, ServiceType.Nurse),
			};
			SimulationResults.Add(result);
			Results.ItemsSource = null;
			Results.ItemsSource = SimulationResults;
		}

		public void ResetOutput() {
			SimulationResults.Clear();
		}

		public class SimulationResult {

			public int Workers { get; set; }

			public int Doctors { get; set; }

			public int Nurses { get; set; }

			public double WorkersOccupancy { get; set; }

			public double DoctorsOccupancy { get; set; }

			public double NursesOccupancy { get; set; }

			public double RegistrationWaitingTime { get; set; }

			public double ExaminationWaitingTime { get; set; }

			public double VaccinationWaitingTime { get; set; }

			public double SumOfWaitingTime {
				get { return RegistrationWaitingTime + ExaminationWaitingTime + VaccinationWaitingTime; }
			}
		}
	}
}
