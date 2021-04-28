using System;
using System.Collections.Generic;
using System.Globalization;
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
using GUI.Outputs;
using OSPABA;
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
			Movement.Refresh(simulation);
		}
	}
}
