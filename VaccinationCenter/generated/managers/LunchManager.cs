using System;
using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.entities;

namespace managers {
	//meta! id="7"
	public class LunchManager : Manager {
		public LunchManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="LunchProcess", id="46", type="Notice"
		public void ProcessEndOfLunch(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			ServiceEntity service = myMessage.Service;
			MyAgent.ServicesEating--;
			MyAgent.ServicesMovingFromLunch++;
			service.StartMoveFromLunch();
			//Console.WriteLine($"Service [{service.Id}] start moving FROM lunch");
			myMessage.Addressee = MyAgent.FindAssistant(SimId.TravelProcess);
			StartContinualAssistant(myMessage);
		}

		//meta! sender="VacCenterAgent", id="28", type="Request"
		public void ProcessLunchBreak(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			MyAgent.ServicesMovingToLunch++;
			ServiceEntity service = myMessage.Service;
			service.StartMoveToLunch();
			//Console.WriteLine($"Service [{service.Id}] start moving TO lunch");
			myMessage.Addressee = MyAgent.FindAssistant(SimId.TravelProcess);
			StartContinualAssistant(myMessage);
		}

		//meta! sender="TravelProcess", id="39", type="Finish"
		public void ProcessFinishTravelProcess(MessageForm message) {
		}

		//meta! sender="LunchProcess", id="41", type="Finish"
		public void ProcessFinishLunchProcess(MessageForm message) {
		}

		//meta! sender="TravelProcess", id="43", type="Notice"
		public void ProcessEndOfTravel(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			ServiceEntity service = myMessage.Service;
			if (service.LunchStatus == LunchStatus.MoveTo) {
				MyAgent.ServicesMovingToLunch--;
				MyAgent.ServicesEating++;
				service.StartEating();
				myMessage.Addressee = MyAgent.FindAssistant(SimId.LunchProcess);
				StartContinualAssistant(myMessage);
			}
			else if (service.LunchStatus == LunchStatus.MoveFrom) {
				MyAgent.ServicesMovingFromLunch--;
				//Console.WriteLine($"Service [{service.Id}] start END move FROM lunch");
				myMessage.Code = Mc.LunchBreak;
				Response(myMessage);
			}
			else {
				Debug.Fail($"Wrong lunch status: {service.LunchStatus}");
			}
		}


		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
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
			case Mc.LunchBreak:
				ProcessLunchBreak(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.LunchProcess:
					ProcessFinishLunchProcess(message);
				break;

				case SimId.TravelProcess:
					ProcessFinishTravelProcess(message);
				break;
				}
			break;

			case Mc.EndOfTravel:
				ProcessEndOfTravel(message);
			break;

			case Mc.EndOfLunch:
				ProcessEndOfLunch(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new LunchAgent MyAgent {
			get {
				return (LunchAgent)base.MyAgent;
			}
		}
	}
}