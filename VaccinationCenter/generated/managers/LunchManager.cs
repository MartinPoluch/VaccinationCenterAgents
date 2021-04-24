using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers
{
	//meta! id="7"
	public class LunchManager : Manager
	{
		public LunchManager(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="LunchProcess", id="46", type="Notice"
		public void ProcessEndOfLunch(MessageForm message)
		{
		}

		//meta! sender="VacCenterAgent", id="28", type="Request"
		public void ProcessLunchBreak(MessageForm message)
		{
		}

		//meta! sender="TravelProcess", id="39", type="Finish"
		public void ProcessFinishTravelProcess(MessageForm message)
		{
		}

		//meta! sender="LunchProcess", id="41", type="Finish"
		public void ProcessFinishLunchProcess(MessageForm message)
		{
		}

		//meta! sender="TravelProcess", id="43", type="Notice"
		public void ProcessEndOfTravel(MessageForm message)
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
			case Mc.EndOfLunch:
				ProcessEndOfLunch(message);
			break;

			case Mc.EndOfTravel:
				ProcessEndOfTravel(message);
			break;

			case Mc.LunchBreak:
				ProcessLunchBreak(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.TravelProcess:
					ProcessFinishTravelProcess(message);
				break;

				case SimId.LunchProcess:
					ProcessFinishLunchProcess(message);
				break;
				}
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new LunchAgent MyAgent
		{
			get
			{
				return (LunchAgent)base.MyAgent;
			}
		}
	}
}