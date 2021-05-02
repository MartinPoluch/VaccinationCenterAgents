using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using simulation;
using VaccinationCenter.common;
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
			return simulation.ServiceAgentStats[ServiceType.AdminWorker].Occupancy.Mean();
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
		}
	}
}
