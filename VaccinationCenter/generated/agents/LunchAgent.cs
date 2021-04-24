using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="7"
	public class LunchAgent : Agent
	{
		public LunchAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

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