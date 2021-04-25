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

		public void Refresh(MySimulation simulation) {
			//TODO implement stats
			//RoomStat roomStat = state.ReplicationStats[RoomType];
			//AvgWaitTime.Text = roomStat.WaitingTime.Average().ToString(CultureInfo.InvariantCulture);
			//CiWaitTime.Text = roomStat.WaitingTime.ConfidenceInterval().ToString();

			//AvgQueueLength.Text = roomStat.QueueLength.Average().ToString(CultureInfo.InvariantCulture);
			//CiQueueLength.Text = roomStat.QueueLength.ConfidenceInterval().ToString();

			//AvgServiceOccupancy.Text = roomStat.ServiceOccupancy.Average().ToString(CultureInfo.InvariantCulture);
			//CiServiceOccupancy.Text = roomStat.ServiceOccupancy.ConfidenceInterval().ToString();

			//AvgQueueLengthAtEnd.Text = state.QueueLengthAtEndOfDayRs[RoomType].Average().ToString(CultureInfo.InvariantCulture);
		}
	}
}
