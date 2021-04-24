using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="8"
	public class WaitingAgent : Agent
	{
		public WaitingAgent(int id, Simulation mySim, Agent parent) :
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
			new WaitingManager(SimId.WaitingManager, MySim, this);
			new WaitingProcess(SimId.WaitingProcess, MySim, this);
			AddOwnMessage(Mc.EndOfWaiting);
			AddOwnMessage(Mc.Waiting);
		}
		//meta! tag="end"
	}
}