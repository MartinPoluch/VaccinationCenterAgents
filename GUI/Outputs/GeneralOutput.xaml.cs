using System.Globalization;
using System.Windows.Controls;
using GUI.Outputs;
using OSPABA;
using simulation;


namespace GUI {
	/// <summary>
	/// Interaction logic for GeneralOutput.xaml
	/// </summary>
	public partial class GeneralOutput : UserControl, OutputStat {
		public GeneralOutput() {
			InitializeComponent();
		}

		public void Refresh(MySimulation simulation) {
			//TODO implement stats
			//VacSystemStat systemStat = state.SystemStat;
			//PatientsArrived.Text = systemStat.ArrivedCustomers.ToString();
			//PatientsLeft.Text = systemStat.NumberOfValues.ToString(CultureInfo.InvariantCulture);
			//PatientsInSystem.Text = systemStat.CustomersInSystem.ToString();
			//PatientsMissing.Text = systemStat.MissingPatients.ToString();

			//WaitRoomStat waitRoom = state.WaitRoomStat;
			//PatientsInWaitRoom.Text = waitRoom.WaitingPatients.ToString();
			//AvgPatientsInWaitRoom.Text = waitRoom.GetAverageWaitingPatients().ToString(CultureInfo.InvariantCulture);
		}
	}
}
