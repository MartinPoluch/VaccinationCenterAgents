using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.entities;

namespace managers {
	//meta! id="74"
	public class MovementManager : Manager {
		public MovementManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
		}

		//meta! sender="VacCenterAgent", id="105", type="Request"
		public void ProcessMoveToAnotherRoom(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			Patient patient = myMessage.Patient;
			MyAgent.MovingPatients[patient.LastVisitedService]++;
			myMessage.Addressee = MyAgent.FindAssistant(SimId.MovementProcess);
			StartContinualAssistant(myMessage);
		}

		//meta! sender="MovementProcess", id="100", type="Finish"
		public void ProcessFinish(MessageForm message) {
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! sender="MovementProcess", id="137", type="Notice"
		public void ProcessEndOfMove(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			Patient patient = myMessage.Patient;
			MyAgent.MovingPatients[patient.LastVisitedService]--;
			myMessage.Code = Mc.MoveToAnotherRoom;
			Response(myMessage);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.EndOfMove:
				ProcessEndOfMove(message);
			break;

			case Mc.MoveToAnotherRoom:
				ProcessMoveToAnotherRoom(message);
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
		public new MovementAgent MyAgent {
			get {
				return (MovementAgent)base.MyAgent;
			}
		}
	}
}