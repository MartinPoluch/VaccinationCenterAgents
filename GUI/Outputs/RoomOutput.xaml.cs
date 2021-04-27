﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
using OSPABA;
using simulation;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for RoomOutput.xaml
	/// </summary>
	public partial class RoomOutput : UserControl, OutputStat {

		public RoomOutput() {
			ServiceEntities = new List<ServiceEntity>();
			InitializeComponent();
			DataContext = this;
		}

		public string RoomName { get; set; }

		public ServiceType ServiceType { get; set; }

		private ServiceAgent GetServiceAgent(MySimulation simulation) {
			switch (ServiceType) {
				case ServiceType.AdminWorker: {
					return simulation.RegistrationAgent;
				}
				case ServiceType.Doctor: {
					return simulation.ExaminationAgent;
				}
				case ServiceType.Nurse: {
					return simulation.VaccinationAgent;
				}
				default: {
					throw new Exception($"Service type not initialized, ServiceType={ServiceType}");
				}
			}
		}

		public List<ServiceEntity> ServiceEntities { get; set; }

		public void Refresh(MySimulation simulation) {
			ServiceAgent service = GetServiceAgent(simulation);
			AvgQueueLength.Text = Utils.ParseMean(service.QueueLengthStat);
			AvgWaitTime.Text = Utils.ParseMean(service.WaitingTimesStat);
			CurrentQueueLength.Text = service.Queue.Count.ToString();
			AvgServiceOccupancy.Text = service.GetAverageServiceOccupancy(simulation.CurrentTime).ToString(CultureInfo.InvariantCulture);
			ServiceEntities = service.ServiceEntities;
			Services.ItemsSource = null;
			Services.ItemsSource = ServiceEntities;
		}
	}
}
