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
using OSPABA;
using simulation;
using VaccinationCenter.entities;


namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for DependencyChart.xaml
	/// </summary>
	public partial class ChartsOutput : UserControl, ResettableOutput {

		public ChartsOutput() {
			InitializeComponent();
		}

		public void Refresh(VacCenterSimulation simulation) {
			int numOfDoctors = simulation.SimParameter.NumOfDoctors;
			var serviceAgentStat = simulation.ServiceAgentStats[ServiceType.Doctor];
			double avgQueueLength = serviceAgentStat.QueueLengths.Mean();
			DoctorQueueLengthChart.AddChartValue(numOfDoctors, avgQueueLength);

			double avgWaitingTime = serviceAgentStat.WaitingTimes.Mean();
			DoctorWaitingChart.AddChartValue(numOfDoctors, avgWaitingTime);

			Console.WriteLine($"Added point => doctors: {numOfDoctors}  " +
			                  $"avg. queue length: {avgQueueLength} wait time: {avgWaitingTime}");
		}

		public void ResetOutput() {
			DoctorQueueLengthChart.Clear();
			DoctorWaitingChart.Clear();
		}
	}
}
