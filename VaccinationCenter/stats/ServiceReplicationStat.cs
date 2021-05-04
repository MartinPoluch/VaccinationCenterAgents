using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPStat;
using simulation;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace VaccinationCenter.stats {
	public class ServiceReplicationStat {

		public ServiceReplicationStat() {
			WaitingTimes = new Stat();
			QueueLengths = new Stat();
			Occupancy = new Stat();
		}

		public void UpdateStats(ServiceAgent serviceAgent, double endTime, Patient fakePatient) {
			serviceAgent.Queue.Enqueue(fakePatient);
			WaitingTimes.AddSample(serviceAgent.WaitingTimeStat.Mean());
			QueueLengths.AddSample(serviceAgent.QueueLengthStat.Mean());
			Occupancy.AddSample(serviceAgent.GetAverageServiceOccupancy(endTime));
		}

		public Stat WaitingTimes { get; }

		public Stat QueueLengths { get; }

		public Stat Occupancy { get; }
	}
}
