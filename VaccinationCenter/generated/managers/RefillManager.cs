using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
namespace managers
{
	//meta! id="144"
	public class RefillManager : Manager
	{
		public RefillManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="NurseMoveProcess", id="153", type="Notice"
		public void ProcessEndOfNurseMove(MessageForm message)
		{
		}

		//meta! sender="VaccinationAgent", id="147", type="Response"
		public void ProcessRefill(MessageForm message)
		{
		}

		//meta! sender="NurseMoveProcess", id="150", type="Finish"
		public void ProcessFinishNurseMoveProcess(MessageForm message)
		{
		}

		//meta! sender="RefillProcess", id="155", type="Finish"
		public void ProcessFinishRefillProcess(MessageForm message)
		{
		}

		//meta! sender="RefillProcess", id="156", type="Notice"
		public void ProcessEndOfRefill(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.EndOfRefill:
				ProcessEndOfRefill(message);
			break;

			case Mc.Refill:
				ProcessRefill(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.NurseMoveProcess:
					ProcessFinishNurseMoveProcess(message);
				break;

				case SimId.RefillProcess:
					ProcessFinishRefillProcess(message);
				break;
				}
			break;

			case Mc.EndOfNurseMove:
				ProcessEndOfNurseMove(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new RefillAgent MyAgent
		{
			get
			{
				return (RefillAgent)base.MyAgent;
			}
		}
	}
}
