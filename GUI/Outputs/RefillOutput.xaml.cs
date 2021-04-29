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
	/// Interaction logic for RefillOutput.xaml
	/// </summary>
	public partial class RefillOutput : UserControl, OutputStat {
		public RefillOutput() {
			InitializeComponent();
		}

		public void Refresh(MySimulation simulation) {
			RefillAgent refill = simulation.RefillAgent;
			MovingToRefill.Text = refill.NursesMovingToRefill.ToString();
			CurrentQueueLength.Text = refill.Queue.Count.ToString();
			Refilling.Text = refill.NursesRefilling.ToString();
			MovingFromRefill.Text = refill.NursesMovingFromRefill.ToString();
			AvgQueueLength.Text = Utils.ParseMean(refill.QueueLengthStat);
			AvgWaitTime.Text = Utils.ParseMean(refill.WaitingTimeStat);
		}
	}
}
