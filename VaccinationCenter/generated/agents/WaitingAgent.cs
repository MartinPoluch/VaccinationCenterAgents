using OSPABA;
using simulation;
using managers;
using continualAssistants;
using OSPDataStruct;
using OSPRNG;
using OSPStat;

namespace agents {

	//meta! id="8"
	public class WaitingAgent : Agent {

		public WaitingAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			WaitingDecisionGen = new UniformContinuousRNG(0, 1);
			WaitingRoomStat = new WStat(MySim);
			WaitingRoom = new SimQueue<int>(WaitingRoomStat);
		}

		public WStat WaitingRoomStat { get; }

		private SimQueue<int> WaitingRoom { get; }

		private RNG<double> WaitingDecisionGen { get; }

		public int GetWaitingPatients() {
			return WaitingRoom.Count;
		}

		public void AddPatientToWaitingRoom() {
			WaitingRoom.Enqueue(1); //just dummy value, I am only interested in stats, dont care about content of queue
		}

		public void RemovePatientFromWaitingRoom() {
			WaitingRoom.Dequeue();
		}

		public double GenerateWaitingTime() {
			double minute = 60;
			return (WaitingDecisionGen.Sample() < 0.05) ? (30 * minute) : (15 * minute);
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			WaitingRoom.Clear();
			WaitingRoomStat.Clear();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new WaitingManager(SimId.WaitingManager, MySim, this);
			new WaitingProcess(SimId.WaitingProcess, MySim, this);
			AddOwnMessage(Mc.EndOfWaiting);
			AddOwnMessage(Mc.Waiting);
		}
		//meta! tag="end"
	}
}