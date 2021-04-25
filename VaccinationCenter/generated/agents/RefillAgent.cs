using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
namespace agents
{
	//meta! id="144"
	public class RefillAgent : Agent
	{
		public RefillAgent(int id, Simulation mySim, Agent parent) :
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
