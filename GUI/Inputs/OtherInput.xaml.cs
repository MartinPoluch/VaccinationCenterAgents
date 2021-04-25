using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GUI.Inputs {

	/// <summary>
	/// Optional inputs for simulation run. Specific modes selection.
	/// </summary>
	public partial class OtherInput : UserControl {

		public OtherInput() {
			InitializeComponent();
			InitializeInputs();
			MinDoctors = 1;
			MaxDoctors = 9;
			DataContext = this;
		}

		public void InitializeInputs() {
		}

		public Mode SelectedMode() {
			if (ClassicMode.IsChecked ?? false) {
				return Mode.Classic;
			}
			if (DependencyChart.IsChecked ?? false) {
				return Mode.DependencyChart;
			}
			else {
				return Mode.Classic; // default
			}
		}

		public int MinDoctors { get; set; }

		public int MaxDoctors { get; set; }

		public void EnableAllInputs() {
			Modes.IsEnabled = true;
			DependencyChartInputs.IsEnabled = true;
		}

		public void DisableAllInputs() {
			Modes.IsEnabled = false;
			DependencyChartInputs.IsEnabled = false;
		}

		public void CheckIntegerInput(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}
