using OSPABA;
using simulation;
using managers;
using continualAssistants;
using OSPRNG;

namespace agents {

	//meta! id="7"
	public class LunchAgent : Agent {
		public LunchAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			MoveDurationGenerator = new UniformContinuousRNG(70, 200);
			LunchDurationGenerator = new TriangularRNG(300, 900, 1800);
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			ServicesMovingToLunch = 0;
			ServicesEating = 0;
			ServicesMovingFromLunch = 0;
		}

		public RNG<double> MoveDurationGenerator { get; }

		public RNG<double> LunchDurationGenerator { get; }

		public int ServicesMovingToLunch { get; set; }

		public int ServicesEating { get; set; }

		public int ServicesMovingFromLunch { get; set; }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new LunchManager(SimId.LunchManager, MySim, this);
			new LunchProcess(SimId.LunchProcess, MySim, this);
			new TravelProcess(SimId.TravelProcess, MySim, this);
			AddOwnMessage(Mc.EndOfLunch);
			AddOwnMessage(Mc.LunchBreak);
			AddOwnMessage(Mc.EndOfTravel);
		}
		//meta! tag="end"
	}
}