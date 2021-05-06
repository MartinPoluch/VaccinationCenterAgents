using System.Windows.Controls;
using OSPABA;
using simulation;
using VaccinationCenter.entities;

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for RoomReplicationOutput.xaml
	/// </summary>
	public partial class RoomReplicationOutput : UserControl, OutputStat {
		public RoomReplicationOutput() {
			InitializeComponent();
			DataContext = this;
		}

		public string RoomName { get; set; }

		public ServiceType ServiceType { get; set; }

		public void Refresh(VacCenterSimulation simulation) {
			var serviceStat = simulation.ServiceAgentStats[ServiceType];
			AvgWaitTime.Text = Utils.ParseMean(serviceStat.WaitingTimes);
			CiWaitTime.Text = Utils.ParseConfidenceInterval(serviceStat.WaitingTimes);

			AvgQueueLength.Text = Utils.ParseMean(serviceStat.QueueLengths);
			CiQueueLength.Text = Utils.ParseConfidenceInterval(serviceStat.QueueLengths);

			AvgServiceOccupancy.Text = Utils.ParseMean(serviceStat.Occupancy);
			CiServiceOccupancy.Text = Utils.ParseConfidenceInterval(serviceStat.Occupancy);
		}
	}
}
