using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="74"
	public class MovementAgent : Agent
	{
		public MovementAgent(int id, Simulation mySim, Agent parent) :
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
			new MovementManager(SimId.MovementManager, MySim, this);
			new MovementProcess(SimId.MovementProcess, MySim, this);
			AddOwnMessage(Mc.EndOfMove);
			AddOwnMessage(Mc.FromExamination);
			AddOwnMessage(Mc.FromVaccination);
			AddOwnMessage(Mc.FromRegistration);
		}
		//meta! tag="end"
	}
}