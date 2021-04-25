using System.Globalization;
using System.Windows.Controls;
using GUI.Outputs;
using OSPABA;
using simulation;
using VaccinationCenter.entities;

namespace GUI {
	/// <summary>
	/// Interaction logic for ReplicationOutput.xaml
	/// </summary>
	public partial class ReplicationOutput : UserControl, OutputStat {
		public ReplicationOutput() {
			InitializeComponent();
			Registration.ServiceType = ServiceType.AdminWorker;
			Examination.ServiceType = ServiceType.Doctor;
			Vaccination.ServiceType = ServiceType.Nurse;
		}

		public void Refresh(MySimulation simulation) {
			Registration.Refresh(simulation);
			Examination.Refresh(simulation);
			Vaccination.Refresh(simulation);
			
			var waitAgentStat = simulation.WaitingAgent.WaitingRoomStat;
			AvgPeopleInWaitRoom.Text = Utils.ParseMean(waitAgentStat);
			CiPeopleInWaitRoom.Text = Utils.ParseConfidenceInterval(waitAgentStat);

			AvgMissingPatients.Text = Utils.ParseMean(simulation.PatientsMissingStat);
			AvgLeftPatients.Text = Utils.ParseMean(simulation.PatientsLeftStat);
			AvgCoolingDuration.Text = (simulation.CoolingDurationStat.Mean() / 3600).ToString(CultureInfo.InvariantCulture);
		}
	}
}
