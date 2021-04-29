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
	/// Interaction logic for WaitRoomOutput.xaml
	/// </summary>
	public partial class WaitRoomOutput : UserControl, OutputStat {
		public WaitRoomOutput() {
			InitializeComponent();
		}

		public void Refresh(MySimulation simulation) {
			WaitingAgent waitingAgent = simulation.WaitingAgent;
			PatientsInWaitRoom.Text = waitingAgent.GetWaitingPatients().ToString();
			AvgPatientsInWaitRoom.Text = Utils.ParseMean(waitingAgent.WaitingRoomStat);
		}
	}
}
