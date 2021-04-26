using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using GUI.Inputs;
using OSPABA;
using simulation;

namespace GUI {
	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
			InitVacCenter();
			Title = "Vaccination center";
			DataContext = this;
		}

		private void InitVacCenter() {
			VacCenterSim = new MySimulation();
			VacCenterSim.OnSimulationWillStart(SimulationWillStart);
			VacCenterSim.OnReplicationWillStart(ReplicationWillStart);
			VacCenterSim.OnRefreshUI(RefreshUI);
			VacCenterSim.OnReplicationDidFinish(ReplicationDidFinish);
			VacCenterSim.OnSimulationDidFinish(SimulationDidFinish);
		}

		public Simulation VacCenterSim { get; set; }

		private void SimulationWillStart(Simulation simulation) {
			MySimulation vacSimulation = (MySimulation)simulation;
			vacSimulation.SimParameter = SimInputs.CreateSimParameter(); // initialization of all simulation inputs
		}

		/**
		 * We will slow down each replication, if maximum speed is not set
		 */
		private void ReplicationWillStart(Simulation simulation) {
			Dispatcher.Invoke(() => {
				if (SimInputs.MaximumSpeed) {
					VacCenterSim.SetMaxSimSpeed();
				}
				else {
					VacCenterSim.SetSimSpeed(1, 1);
				}
			});
			
		}

		/**
		 * Refresh GUI during slow mode, refresh current stats
		 */
		private void RefreshUI(Simulation simulation) {
			Dispatcher.Invoke(() => {
				MySimulation vacSimulation = (MySimulation)simulation;
				CurrentStateOutput.Refresh(vacSimulation);
				string timeFormat = "HH:mm:ss";
				SimulationTimeOut.Text = SimInputs.StartDateTime().AddSeconds(vacSimulation.CurrentTime).ToString(timeFormat);
			});
		}

		/**
		 * Refresh replication stats
		 */
		private void ReplicationDidFinish(Simulation simulation) {
			Dispatcher.Invoke(() => {
				MySimulation vacSimulation = (MySimulation)simulation;
				// refresh after first replication, because CIs cannot be calculated from only one value
				if (vacSimulation.CurrentReplication > 1) { 
					ReplicationsOut.Refresh(vacSimulation);
					CurrentReplicationOut.Text = vacSimulation.CurrentReplication.ToString();
				}
			});
		}

		private void SimulationDidFinish(Simulation simulation) {
			Dispatcher.Invoke(() => {
				StartAndStopBtn.IsChecked = false;
				ActivateReadyState();
			});
		
		}

		private void StartClick(object sender, RoutedEventArgs e) {
			if (SimInputs.ValidInputs()) {
				ResetAllOutputs();
				ActivateRunningState();
				if (OtherInputs.SelectedMode() == Mode.Classic) {
					//InitVacCenter();
					Console.WriteLine("reps: " + SimInputs.Replications);
					VacCenterSim.SimulateAsync(SimInputs.Replications, double.MaxValue);
				}
				
			}
			else {
				MessageBox.Show("Cannot start simulation", "Wrong inputs", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ActivateReadyState() {
			ConsoleOut.Text += "\nSimulation is ready to start";
			StartAndStopBtn.Content = "Start";
			StartAndStopBtn.IsEnabled = true;
			PauseAndContinueBtn.Content = "Pause";

			PauseAndContinueBtn.IsEnabled = false;
			SimInputs.IsEnabled = true;
			OtherInputs.EnableAllInputs();
		}

		private void ActivateRunningState() {
			ConsoleOut.Text = "Simulation is running...";
			StartAndStopBtn.Content = "Stop";
			StartAndStopBtn.IsEnabled = true;
			PauseAndContinueBtn.Content = "Pause";

			PauseAndContinueBtn.IsEnabled = true;
			SimInputs.IsEnabled = false;
			OtherInputs.DisableAllInputs();
		}


		private void ActivatePausedState() {
			ConsoleOut.Text = "Simulation is paused";
			StartAndStopBtn.Content = "Stop";
			StartAndStopBtn.IsEnabled = false;
			PauseAndContinueBtn.Content = "Continue";

			PauseAndContinueBtn.IsEnabled = true;
			SimInputs.IsEnabled = false;
			OtherInputs.DisableAllInputs();
		}

		private void ResetAllOutputs() {
			//TODO reset outputs here
			ConsoleOut.Text = "";
		}

		private void StopClick(object sender, RoutedEventArgs e) {
			VacCenterSim.StopSimulation();
			ActivateReadyState();
		}

		private void PauseClick(object sender, RoutedEventArgs e) {
			VacCenterSim.PauseSimulation();
			ActivatePausedState();
		}

		private void ContinueClick(object sender, RoutedEventArgs e) {
			VacCenterSim.ResumeSimulation();
			ActivateRunningState();
		}
	}
}
