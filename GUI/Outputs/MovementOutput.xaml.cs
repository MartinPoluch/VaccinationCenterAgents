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
using VaccinationCenter.entities;

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for MovementOutput.xaml
	/// </summary>
	public partial class MovementOutput : UserControl, OutputStat {

		public MovementOutput() {
			InitializeComponent();
		}

		public void Refresh(MySimulation simulation) {
			MovementAgent agent = simulation.MovementAgent;
			FromRegistration.Text = agent.MovingPatients[ServiceType.AdminWorker].ToString();
			FromExamination.Text = agent.MovingPatients[ServiceType.Doctor].ToString();
			FromVaccination.Text = agent.MovingPatients[ServiceType.Nurse].ToString();
		}
	}
}
