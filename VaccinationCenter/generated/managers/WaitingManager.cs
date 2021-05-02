using System;
using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers {
	//meta! id="8"
	public class WaitingManager : Manager {
		public WaitingManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication

		}

		//meta! sender="WaitingProcess", id="36", type="Finish"
		public void ProcessFinish(MessageForm message) {
		}

		//meta! sender="VacCenterAgent", id="23", type="Request"
		public void ProcessWaiting(MessageForm message) {
			MyAgent.AddPatientToWaitingRoom();
			message.Addressee = MyAgent.FindAssistant(SimId.WaitingProcess);
			StartContinualAssistant(message);
		}

		//meta! sender="WaitingProcess", id="109", type="Notice"
		public void ProcessEndOfWaiting(MessageForm message) {
			MyAgent.RemovePatientFromWaitingRoom();
			message.Code = Mc.Waiting;
			Response(message);
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
			case Mc.Waiting:
				ProcessWaiting(message);
			break;

			case Mc.EndOfWaiting:
				ProcessEndOfWaiting(message);
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
		public new WaitingAgent MyAgent {
			get {
				return (WaitingAgent)base.MyAgent;
			}
		}
	}
}