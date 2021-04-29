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
using agents;
using simulation;

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for RefillReplicationOutput.xaml
	/// </summary>
	public partial class RefillReplicationOutput : UserControl, OutputStat {
		public RefillReplicationOutput() {
			InitializeComponent();
		}

		public void Refresh(MySimulation simulation) {
			AvgWaitingTime.Text = Utils.ParseMean(simulation.NurseWaitingTimeStat);
			CiWaitingTime.Text = Utils.ParseConfidenceInterval(simulation.NurseWaitingTimeStat);
			AvgQueueLength.Text = Utils.ParseMean(simulation.NursesQueueLengthStat);
			CiQueueLength.Text = Utils.ParseConfidenceInterval(simulation.NursesQueueLengthStat);
		}
	}
}
