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

		//meta! sender="MovementProcess", id="101", type="Notice"
		public void ProcessEndOfMove(MessageForm message)
		{
		}

		//meta! sender="VacCenterAgent", id="105", type="Request"
		public void ProcessFromExamination(MessageForm message)
		{
		}

		//meta! sender="VacCenterAgent", id="106", type="Request"
		public void ProcessFromVaccination(MessageForm message)
		{
		}

		//meta! sender="VacCenterAgent", id="104", type="Request"
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

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.FromExamination:
				ProcessFromExamination(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			case Mc.FromVaccination:
				ProcessFromVaccination(message);
			break;

			case Mc.FromRegistration:
				ProcessFromRegistration(message);
			break;

			case Mc.EndOfMove:
				ProcessEndOfMove(message);
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