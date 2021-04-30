using System.Windows.Controls;
using System.Windows.Data;
using GUI.Outputs;
using simulation;
using VaccinationCenter.entities;

namespace GUI {

	/// <summary>
	/// Interaction logic for CurrentStateOutput.xaml
	/// </summary>
	public partial class CurrentStateOutput : UserControl, OutputStat {

		public CurrentStateOutput() {
			InitializeComponent();
			Registration.ServiceType = ServiceType.AdminWorker;
			Examination.ServiceType = ServiceType.Doctor;
			Vaccination.ServiceType = ServiceType.Nurse;
			DataGridTextColumn dosesColumn = new DataGridTextColumn {
				Header = "Doses",
				Binding = new Binding("Doses")
			};
			Vaccination.AddColumn(dosesColumn);
		}

		public void Refresh(MySimulation simulation) {
			Registration.Refresh(simulation);
			Examination.Refresh(simulation);
			Vaccination.Refresh(simulation);
			GeneralOut.Refresh(simulation);
			Refill.Refresh(simulation);
			Movement.Refresh(simulation);
			WaitRoom.Refresh(simulation);
			Lunch.Refresh(simulation);
		}
	}
}
