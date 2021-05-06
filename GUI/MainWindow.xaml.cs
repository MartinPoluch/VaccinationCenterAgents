using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GUI.Inputs;
using OSPABA;
using OSPRNG;
using simulation;
using VaccinationCenter;

namespace GUI {
	public partial class MainWindow : Window {

		public MainWindow() {
			//Random seeder = new Random(5000);
			//RNG<double>.SetSeedGen(seeder);
			//RNG<int>.SetSeedGen(seeder);
			InitializeComponent();
			InitVacCenter();
			Title = "Vaccination center";
			DataContext = this;
		}

		private void InitVacCenter() {
			VacCenterSim = new VacCenterSimulation();
			VacCenterSim.OnSimulationWillStart(SimulationWillStart);
			VacCenterSim.OnReplicationWillStart(ReplicationWillStart);
			VacCenterSim.OnRefreshUI(RefreshUI);
			VacCenterSim.OnReplicationDidFinish(ReplicationDidFinish);
			VacCenterSim.OnSimulationDidFinish(SimulationDidFinish);
		}

		public Simulation VacCenterSim { get; set; }

		public SimulationWrapper SimulationWrapper { get; set; }

		private void SimulationWillStart(Simulation simulation) {
			VacCenterSimulation vacSimulation = (VacCenterSimulation)simulation;
			vacSimulation.SimParameter = SimInputs.CreateSimParameter(); // initialization of all simulation inputs
		}

		/**
		 * We will slow down each replication, if maximum speed is not set
		 */
		private void ReplicationWillStart(Simulation simulation) {
			Dispatcher.Invoke(() => {
				CurrentReplicationOut.Text = (simulation.CurrentReplication + 1).ToString();
				if (SimInputs.MaximumSpeed) {
					VacCenterSim.SetMaxSimSpeed();
				}
				else {
					ChangeSimulationSpeed();
				}
			});
			
		}

		/**
		 * Refresh GUI during slow mode, refresh current stats
		 */
		private void RefreshUI(Simulation simulation) {
			Dispatcher.Invoke(() => {
				VacCenterSimulation vacSimulation = (VacCenterSimulation)simulation;
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
				VacCenterSimulation vacSimulation = (VacCenterSimulation)simulation;
				// refresh after first replication, because CIs cannot be calculated from only one value
				if (vacSimulation.CurrentReplication > 1) { 
					ReplicationsOut.Refresh(vacSimulation);
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
					InitVacCenter();
					VacCenterSim.SimulateAsync(SimInputs.Replications);
				}
				else if (OtherInputs.SelectedMode() == Mode.DependencyChart) {
					CreateDependencyChart();
				}
				else if (OtherInputs.SelectedMode() == Mode.VariantsComparison) {
					CreateVariantsTable();
				}
				else {
					Console.WriteLine($"Unknown mode: {OtherInputs.SelectedMode()}");
				}
			}
			else {
				MessageBox.Show("Cannot start simulation", "Wrong inputs", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CreateDependencyChart() {
			Charts.ResetOutput();
			ConsoleOut.Text = "Calculating points for dependency chart ...";
			SimulationWrapper = new SimulationWrapper();
			BackgroundWorker worker = new BackgroundWorker() {
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			worker.DoWork += delegate (object sender, DoWorkEventArgs args) {
				SimulationWrapper.SimulateDoctorsDependency(
					worker,
					SimInputs.Replications,
					SimInputs.SourceIntensity,
					SimInputs.NumOfWorkers,
					OtherInputs.MinDoctors,
					OtherInputs.MaxDoctors,
					SimInputs.NumOfNurses,
					SimInputs.Validation,
					SimInputs.EarlyArrivals);
			};
			worker.ProgressChanged += RefreshDependencyChart;
			worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs args) {
				RefreshAfterSimulation(sender, args);
				ConsoleOut.Text += "Dependency chart was successfully created.";
			};
			worker.RunWorkerAsync();
		}

		private void RefreshDependencyChart(object sender, ProgressChangedEventArgs e) {
			VacCenterSimulation simulation = (VacCenterSimulation)e.UserState;
			Charts.Refresh(simulation);
		}

		private void CreateVariantsTable() {
			VariantsTable.ResetOutput();
			ConsoleOut.Text = "Calculating different simulation variants ...";
			SimulationWrapper = new SimulationWrapper();
			BackgroundWorker worker = new BackgroundWorker() {
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			worker.DoWork += delegate (object sender, DoWorkEventArgs args) {
				SimulationWrapper.SimulateVariants(worker, SimInputs.Replications, SimInputs.CreateSimParameter());
			};
			worker.ProgressChanged += RefreshVariantsTable;
			worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs args) {
				RefreshAfterSimulation(sender, args);
				ConsoleOut.Text += "Results table was successfully created.";
			};
			worker.RunWorkerAsync();
		}

		private void RefreshVariantsTable(object sender, ProgressChangedEventArgs e) {
			VacCenterSimulation simulation = (VacCenterSimulation)e.UserState;
			VariantsTable.Refresh(simulation);
		}

		private void RefreshAfterSimulation(object sender, RunWorkerCompletedEventArgs e) {
			var error = e.Error;
			if (error != null) {
				Console.WriteLine($"Error occured: {error}");
				MessageBox.Show(error.StackTrace, error.Message, MessageBoxButton.OK, MessageBoxImage.Error);
			}
			StartAndStopBtn.IsChecked = false;
			ActivateReadyState();
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
			ConsoleOut.Text = "";
		}

		private void StopClick(object sender, RoutedEventArgs e) {
			if (OtherInputs.SelectedMode() == Mode.Classic) {
				VacCenterSim.StopSimulation();
			}
			else {
				SimulationWrapper.Stop();
			}
			ActivateReadyState();
		}

		private void PauseClick(object sender, RoutedEventArgs e) {
			if (OtherInputs.SelectedMode() == Mode.Classic) {
				VacCenterSim.PauseSimulation();
			}
			else {
				SimulationWrapper.Pause();
			}
			ActivatePausedState();
		}

		private void ContinueClick(object sender, RoutedEventArgs e) {
			if (OtherInputs.SelectedMode() == Mode.Classic) {
				VacCenterSim.ResumeSimulation();
			}
			else {
				SimulationWrapper.Continue();
			}
			ActivateRunningState();
		}

		private void SliderValueChanged(object sender, DragCompletedEventArgs e) {
			ChangeSimulationSpeed();
		}

		private void ChangeSimulationSpeed() {
			VacCenterSim.SetSimSpeed(FrequencySlider.Value, DurationSlider.Value);
		}

	}
}
