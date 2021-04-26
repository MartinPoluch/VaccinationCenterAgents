using System.Globalization;
using System.Windows.Controls;
using agents;
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
			SurroundingsAgent surroundings = simulation.SurroundingsAgent;
			PatientsArrived.Text = surroundings.PatientsArrived.ToString();
			PatientsLeft.Text = surroundings.PatientsLeft.ToString();
			PatientsMissing.Text = surroundings.PatientsMissing.ToString();
			PatientsInSystem.Text = "XXX";

			WaitingAgent waitingAgent = simulation.WaitingAgent;
			PatientsInWaitRoom.Text = waitingAgent.GetWaitingPatients().ToString();
			AvgPatientsInWaitRoom.Text = Utils.ParseMean(waitingAgent.WaitingRoomStat);
		}
	}
}
