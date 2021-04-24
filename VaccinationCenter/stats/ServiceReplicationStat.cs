﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPStat;
using VaccinationCenter.common;

namespace VaccinationCenter.stats {
	public class ServiceReplicationStat {

		public ServiceReplicationStat() {
			WaitingTimes = new Stat();
			QueueLengths = new Stat();
			Occupancy = new Stat();
		}

		public void UpdateStats(ServiceAgent serviceAgent) {
			WaitingTimes.AddSample(serviceAgent.WaitingTimes.Mean());
			QueueLengths.AddSample(serviceAgent.QueueLengthStat.Mean());
			Occupancy.AddSample(serviceAgent.GetAverageServiceOccupancy());
		}

		public Stat WaitingTimes { get; }

		public Stat QueueLengths { get; }

		public Stat Occupancy { get; }
	}
}
