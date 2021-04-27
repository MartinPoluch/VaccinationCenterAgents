using System.Collections.Generic;
using OSPABA;
using simulation;
using managers;
using continualAssistants;
using OSPRNG;
using VaccinationCenter.entities;

namespace agents {
	//meta! id="74"
	public class MovementAgent : Agent {

		public MovementAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			MoveDurationGenerators = new Dictionary<ServiceType, RNG<double>>() {
				[ServiceType.AdminWorker] = new UniformContinuousRNG(40, 90),
				[ServiceType.Doctor] = new UniformContinuousRNG(20, 45),
				[ServiceType.Nurse] = new UniformContinuousRNG(45, 110),
			};
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			MovingPatients = new Dictionary<ServiceType, int>() {
				[ServiceType.AdminWorker] = 0,
				[ServiceType.Doctor] = 0,
				[ServiceType.Nurse] = 0,
			};
		}

		public Dictionary<ServiceType, RNG<double>> MoveDurationGenerators { get; set; }

		public Dictionary<ServiceType, int> MovingPatients { get; set; }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init() {
			new MovementManager(SimId.MovementManager, MySim, this);
			new MovementProcess(SimId.MovementProcess, MySim, this);
			AddOwnMessage(Mc.MoveToAnotherRoom);
			AddOwnMessage(Mc.EndOfMove);
		}
		//meta! tag="end"
	}
}