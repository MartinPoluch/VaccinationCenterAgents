using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers
{
	//meta! id="74"
	public class MovementManager : Manager
	{
		public MovementManager(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="VacCenterAgent", id="105", type="Request"
		public void ProcessMoveToAnotherRoom(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFromVaccination(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFromRegistration(MessageForm message)
		{
		}

		//meta! sender="MovementProcess", id="100", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="MovementProcess", id="137", type="Notice"
		public void ProcessEndOfMove(MessageForm message)
		{
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.MoveToAnotherRoom:
				ProcessMoveToAnotherRoom(message);
			break;

			case Mc.EndOfMove:
				ProcessEndOfMove(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new MovementAgent MyAgent
		{
			get
			{
				return (MovementAgent)base.MyAgent;
			}
		}
	}
}