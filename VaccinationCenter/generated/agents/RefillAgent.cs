using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
using OSPDataStruct;
using OSPRNG;
using OSPStat;
using VaccinationCenter.entities;

namespace agents {
	//meta! id="144"
	public class RefillAgent : Agent {

		public static readonly int MaxNursesCapacity = 2;

		public RefillAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			CustomInit();
			QueueLengthStat = new WStat(mySim);
			Queue = new SimQueue<Nurse>(QueueLengthStat);
			WaitingTimeStat = new Stat();
			OneRefillGenerator = new TriangularRNG(6, 10, 40);
			MoveDurationGenerator = new UniformContinuousRNG(10, 18);
		}

		private void CustomInit() {
			AddOwnMessage(Mc.OneRefillDone);
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			Queue.Clear();
			QueueLengthStat.Clear();
			WaitingTimeStat.Clear();
			NursesMovingToRefill = 0;
			NursesRefilling = 0;
			NursesMovingFromRefill = 0;
		}

		public SimQueue<Nurse> Queue { get; set; }

		public WStat QueueLengthStat { get; set; }

		public Stat WaitingTimeStat { get; }

		public int NursesMovingToRefill { get; set; }

		public int NursesRefilling { get; set; }

		public int NursesMovingFromRefill { get; set; }

		public RNG<double> MoveDurationGenerator { get; }

		public RNG<double> OneRefillGenerator { get; }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init() {
			new RefillManager(SimId.RefillManager, MySim, this);
			new NurseMoveProcess(SimId.NurseMoveProcess, MySim, this);
			new RefillProcess(SimId.RefillProcess, MySim, this);
			AddOwnMessage(Mc.EndOfNurseMove);
			AddOwnMessage(Mc.Refill);
			AddOwnMessage(Mc.EndOfRefill);
		}
		//meta! tag="end"
	}
}
