using OSPABA;
using simulation;
using managers;
using continualAssistants;
using OSPRNG;

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
			ExponentialRng = new ExponentialRNG(5.0);  //TODO only for validation, remove
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


		public ExponentialRNG ExponentialRng { get; set; } //TODO only for validation, remove

		public double GetArrivalsFrequency() {
			//return WorkDayTime / (double)PatientsPerDay;
			return ExponentialRng.Sample();
		}
	}
}