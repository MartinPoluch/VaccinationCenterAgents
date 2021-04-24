using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="2"
	public class SurroundingsAgent : Agent
	{

		public readonly int PatientsPerDay = 540;
		public readonly double WorkDayTime = 9 * 60 * 60;

		public SurroundingsAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			base.PrepareReplication();
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
			return WorkDayTime / (double)PatientsPerDay;
		}
	}
}