using System;
using System.Collections.Generic;
using OSPABA;
using agents;
using OSPStat;
using VaccinationCenter.common;
using VaccinationCenter.entities;
using VaccinationCenter.models;
using VaccinationCenter.stats;

namespace simulation {

	public class MySimulation : Simulation {

		public static readonly double InfinityTime = double.MaxValue;

		public MySimulation() {
			Init();
		}

		public SimParameter SimParameter { get; set; }

		private List<ServiceAgent> ServiceAgents { get; set; }

		public Dictionary<ServiceType, ServiceReplicationStat> ServiceAgentStats { get; set; }

		public Stat WaitingRoomStat { get; set; }

		public Stat PatientsLeftStat { get; set; }

		public Stat PatientsMissingStat { get; set; }

		public Stat CoolingDurationStat { get; set; }

		protected override void PrepareSimulation() {
			base.PrepareSimulation();
			//initialize stats
			ServiceAgents = new List<ServiceAgent>() {RegistrationAgent, ExaminationAgent, VaccinationAgent};
			ServiceAgentStats = new Dictionary<ServiceType, ServiceReplicationStat>() {
				[ServiceType.AdminWorker] = new ServiceReplicationStat(),
				[ServiceType.Doctor] = new ServiceReplicationStat(),
				[ServiceType.Nurse] = new ServiceReplicationStat(),
			};
			WaitingRoomStat = new Stat();
			CoolingDurationStat = new Stat();
			PatientsLeftStat = new Stat();
			PatientsMissingStat = new Stat();

			Initializable[] initAgents = { // agents that needs Simulation Parameter for their initialization
				SurroundingsAgent, RegistrationAgent, ExaminationAgent, VaccinationAgent
			};
			foreach (Initializable agent in initAgents) {
				agent.Initialize(SimParameter);
			}
		}

		protected override void PrepareReplication() {
			base.PrepareReplication();
			if (CurrentReplication % 100 == 0) {
				Console.WriteLine($"Replication: {CurrentReplication}");
			}
		}

		protected override void ReplicationFinished() {
			base.ReplicationFinished();
			foreach (ServiceAgent serviceAgent in ServiceAgents) {
				ServiceReplicationStat replicationStat = ServiceAgentStats[serviceAgent.GetServiceType()];
				replicationStat.UpdateStats(serviceAgent, SurroundingsAgent.LastPatientExitTime);
			}
			WaitingRoomStat.AddSample(WaitingAgent.WaitingRoomStat.Mean());
			CoolingDurationStat.AddSample(SurroundingsAgent.LastPatientExitTime - SurroundingsAgent.WorkDayDuration);
			PatientsMissingStat.AddSample(SurroundingsAgent.PatientsMissing);
			PatientsLeftStat.AddSample(SurroundingsAgent.PatientsLeft);
		}

		protected override void SimulationFinished() {
			base.SimulationFinished();
			PrintResults();
		}

		private void PrintResults() {
			Console.WriteLine("\n-----------------------------------------------");
			foreach (var pair in ServiceAgentStats) {
				Console.WriteLine($"Service type: {pair.Key}");
				var replicationStat = pair.Value;
				Console.WriteLine($"Avg queue length: {replicationStat.QueueLengths.Mean()}");
				Console.WriteLine($"Avg wait times: {replicationStat.WaitingTimes.Mean()}");
				Console.WriteLine($"Avg service occupancy: {replicationStat.Occupancy.Mean()}");
				Console.WriteLine("\n-----------------------------------------------");
			}

			Console.WriteLine($"Avg. waiting patients: {WaitingRoomStat.Mean()}");
			Console.WriteLine($"Avg. cooling: {CoolingDurationStat.Mean()/(3600)}");
			Console.WriteLine($"Avg. patients left: {PatientsLeftStat.Mean()}");
			Console.WriteLine($"Avg. patients missing: {PatientsMissingStat.Mean()}");


		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			ModelAgent = new ModelAgent(SimId.ModelAgent, this, null);
			SurroundingsAgent = new SurroundingsAgent(SimId.SurroundingsAgent, this, ModelAgent);
			VacCenterAgent = new VacCenterAgent(SimId.VacCenterAgent, this, ModelAgent);
			VaccinationAgent = new VaccinationAgent(SimId.VaccinationAgent, this, VacCenterAgent);
			ExaminationAgent = new ExaminationAgent(SimId.ExaminationAgent, this, VacCenterAgent);
			RegistrationAgent = new RegistrationAgent(SimId.RegistrationAgent, this, VacCenterAgent);
			LunchAgent = new LunchAgent(SimId.LunchAgent, this, VacCenterAgent);
			WaitingAgent = new WaitingAgent(SimId.WaitingAgent, this, VacCenterAgent);
			MovementAgent = new MovementAgent(SimId.MovementAgent, this, VacCenterAgent);
			RefillAgent = new RefillAgent(SimId.RefillAgent, this, VaccinationAgent);
		}
		public ModelAgent ModelAgent
		{ get; set; }
		public SurroundingsAgent SurroundingsAgent
		{ get; set; }
		public VacCenterAgent VacCenterAgent
		{ get; set; }
		public VaccinationAgent VaccinationAgent
		{ get; set; }
		public ExaminationAgent ExaminationAgent
		{ get; set; }
		public RegistrationAgent RegistrationAgent
		{ get; set; }
		public LunchAgent LunchAgent
		{ get; set; }
		public WaitingAgent WaitingAgent
		{ get; set; }
		public MovementAgent MovementAgent
		{ get; set; }
		public RefillAgent RefillAgent
		{ get; set; }
		//meta! tag="end"
	}
}