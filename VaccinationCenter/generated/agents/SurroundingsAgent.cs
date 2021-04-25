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

		public readonly double WorkDayDuration = 9 * 60 * 60;

		public SurroundingsAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			MissingDecisionGen = new UniformContinuousRNG(0, 1);
		}

		public int PatientsPerDay { get; private set; } // planned number of patients per day

		public int PatientsArrived { get; set; }

		public int PatientsLeft { get; set; }

		public int PatientsMissing { get; set; }

		public double LastPatientExitTime { get; set; }

		private UniformDiscreteRNG MissingPatientsGen { get; set; }

		private RNG<double> MissingDecisionGen { get; set; }

		private double ProbabilityOfMissing { get; set; }

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

			MyMessage initialMsg = new MyMessage(MySim) {
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
			AddOwnMessage(Mc.PatientExit);
			AddOwnMessage(Mc.NewArrival);
		}
		//meta! tag="end"

		public double GetArrivalsFrequency() {
			return WorkDayDuration / (double)PatientsPerDay;
		}

		public bool PatientIsMissing() {
			return (MissingDecisionGen.Sample() < ProbabilityOfMissing);
		}
	}
}