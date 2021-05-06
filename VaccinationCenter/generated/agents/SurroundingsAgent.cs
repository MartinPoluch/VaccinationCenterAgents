using System;
using OSPABA;
using simulation;
using managers;
using continualAssistants;
using OSPRNG;
using VaccinationCenter.common;
using VaccinationCenter.entities;
using VaccinationCenter.models;

namespace agents {
	//meta! id="2"
	public class SurroundingsAgent : Agent, Initializable {

		private static readonly double Minute = 60;

		public readonly double WorkDayDuration = 9 * 60 * Minute;

		public SurroundingsAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			CustomInit();
			MissingDecisionGen = new UniformContinuousRNG(0, 1);
			EarlyArrivalDecisionGen = new UniformContinuousRNG(0, 1);
			EarlyArrivalGen = new EmpiricRNG<double>(
				new EmpiricPair<double>(new UniformContinuousRNG(Minute, 20 * Minute), 0.3),
				new EmpiricPair<double>(new UniformContinuousRNG(20 * Minute, 60 * Minute), 0.4),
				new EmpiricPair<double>(new UniformContinuousRNG(60 * Minute, 80 * Minute), 0.2),
				new EmpiricPair<double>(new UniformContinuousRNG(80 * Minute, 240 * Minute), 0.1)
			);
		}

		private void CustomInit() {
			AddOwnMessage(Mc.GroupArrival);
		}

		public int PatientsPerDay { get; private set; } // planned number of patients per day

		public int PatientsArrived { get; set; }

		public int PatientsLeft { get; set; }

		public int PatientsMissing { get; set; }

		public double LastPatientExitTime { get; set; }

		private UniformDiscreteRNG MissingPatientsGen { get; set; }

		private RNG<double> MissingDecisionGen { get; set; }

		private double ProbabilityOfMissing { get; set; }

		private RNG<double> EarlyArrivalDecisionGen { get; set; }

		public RNG<double> EarlyArrivalGen { get; set; }

		public void Initialize(SimParameter parameter) {
			PatientsPerDay = parameter.NumOfPatients;
			MissingPatientsGen = new UniformDiscreteRNG(
				parameter.GetMinMissingPatients(),
				parameter.GetMaxMissingPatients());
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			PatientsArrived = 0;
			PatientsLeft = 0;
			PatientsMissing = 0;
			LastPatientExitTime = 0;

			double missingPatients = MissingPatientsGen.Sample();
			ProbabilityOfMissing = missingPatients / (double)PatientsPerDay;

			Message initialMsg = new Message(MySim) {
				Addressee = this,
				Code = Mc.Initialization,
			};
			MyManager.Call(initialMsg);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new SurroundingsManager(SimId.SurroundingsManager, MySim, this);
			new ArrivalsScheduler(SimId.ArrivalsScheduler, MySim, this);
			new EarlyArrivalsScheduler(SimId.EarlyArrivalsScheduler, MySim, this);
			AddOwnMessage(Mc.PatientExit);
			AddOwnMessage(Mc.NewEarlyArrival);
			AddOwnMessage(Mc.NewArrival);
		}
		//meta! tag="end"

		public double GetArrivalsFrequency() {
			return WorkDayDuration / (double)PatientsPerDay;
		}

		public bool PatientIsMissing() {
			return (MissingDecisionGen.Sample() < ProbabilityOfMissing);
		}

		public bool PatientWilComeEarly() {
			return (EarlyArrivalDecisionGen.Sample() < 0.9);
		}
	}
}