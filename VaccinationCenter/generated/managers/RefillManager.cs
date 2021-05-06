using System;
using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
using VaccinationCenter.entities;

namespace managers {
	//meta! id="144"
	public class RefillManager : Manager {

		public RefillManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="NurseMoveProcess", id="153", type="Notice"
		public void ProcessEndOfNurseMove(MessageForm message) {
			Message myMessage = (Message)message;
			Nurse nurse = myMessage.GetNurse();
			if (nurse.Doses == 0) { // move from vaccination room to refill room
				EndOfMoveToRefillRoom(myMessage);
			}
			else if (nurse.HasFullDoses()) { // move from refill room to vaccination room
				EndOfMoveFromRefillRoom(myMessage);
			}
			else {
				Debug.Fail($"Nurse has {nurse.Doses} doses, should have either 0 or 20.");
			}
		}

		private void StartRefillService(Message myMessage) {
			Nurse nurse = myMessage.GetNurse();
			MyAgent.NursesRefilling++;
			double waitingTime = MySim.CurrentTime - nurse.StartOfWaiting;
			MyAgent.WaitingTimeStat.AddSample(waitingTime);
			nurse.StartRefilling();
			myMessage.Addressee = MyAgent.FindAssistant(SimId.RefillProcess);
			myMessage.Code = Mc.Refill;
			StartContinualAssistant(myMessage);
			//Console.WriteLine($"Nurse {nurse.Id} done start refill");
		}

		private void EndOfMoveToRefillRoom(Message myMessage) {
			Nurse nurse = myMessage.GetNurse();
			Debug.Assert(nurse.Doses == 0, "If nurse goes to refill room, then she should have 0 doses.");
			MyAgent.NursesMovingToRefill--;
			nurse.StartWaitingForRefill(); // if there is enough capacity, then the nurse will wait 0 seconds
			if (MyAgent.NursesRefilling < RefillAgent.MaxNursesCapacity) { // nurse can do refill
				StartRefillService(myMessage);
			}
			else { // not enough capacity, nurse must wait for her turn
				//Console.WriteLine($"Nurse {nurse.Id} went to queue");
				MyAgent.Queue.Enqueue(myMessage.GetNurse());
			}
		}

		private void EndOfMoveFromRefillRoom(Message myMessage) {
			MyAgent.NursesMovingFromRefill--;
			myMessage.Code = Mc.Refill;
			Response(myMessage); // back to vaccination room
		}

		//meta! sender="VaccinationAgent", id="147", type="Response"
		public void ProcessRefill(MessageForm message) { // first interaction in this manager
			MyAgent.NursesMovingToRefill++;
			message.Addressee = MyAgent.FindAssistant(SimId.NurseMoveProcess);
			StartContinualAssistant(message); // move from vaccination room to refill room
		}

		//meta! sender="NurseMoveProcess", id="150", type="Finish"
		public void ProcessFinishNurseMoveProcess(MessageForm message) {
		}

		//meta! sender="RefillProcess", id="155", type="Finish"
		public void ProcessFinishRefillProcess(MessageForm message) {
		}

		//meta! sender="RefillProcess", id="156", type="Notice"
		public void ProcessEndOfRefill(MessageForm message) {
			Message myMessage = (Message)message;
			Nurse nurse = myMessage.GetNurse();
			nurse.StartMoveFromRefill();
			myMessage.Addressee = MyAgent.FindAssistant(SimId.NurseMoveProcess);
			StartContinualAssistant((Message)myMessage.CreateCopy()); // move from refill room back to vaccination room

			MyAgent.NursesMovingFromRefill++;
			MyAgent.NursesRefilling--;
			if (! MyAgent.Queue.IsEmpty()) {
				Nurse nextNurse = MyAgent.Queue.Dequeue();
				myMessage.Service = nextNurse; // this is copy of message
				Debug.Assert(MyAgent.NursesRefilling < RefillAgent.MaxNursesCapacity);
				StartRefillService(myMessage);
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

			case Mc.Refill:
				ProcessRefill(message);
			break;

			case Mc.EndOfNurseMove:
				ProcessEndOfNurseMove(message);
			break;

			case Mc.EndOfRefill:
				ProcessEndOfRefill(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new RefillAgent MyAgent {
			get {
				return (RefillAgent)base.MyAgent;
			}
		}
	}
}