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

namespace GUI.Outputs.current {

	/// <summary>
	/// Interaction logic for LunchOutput.xaml
	/// </summary>
	public partial class LunchOutput : UserControl, OutputStat {

		public LunchOutput() {
			InitializeComponent();
		}

		public void Refresh(VacCenterSimulation simulation) {
			LunchAgent lunch = simulation.LunchAgent;
			MovingTo.Text = lunch.ServicesMovingToLunch.ToString();
			Eating.Text = lunch.ServicesEating.ToString();
			MovingFrom.Text = lunch.ServicesMovingFromLunch.ToString();
		}
	}
}
