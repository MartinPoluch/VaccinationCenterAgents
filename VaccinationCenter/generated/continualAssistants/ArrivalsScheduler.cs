using OSPABA;
using simulation;
using agents;
using VaccinationCenter.entities;

namespace continualAssistants {
	//meta! id="30"
	public class ArrivalsScheduler : Scheduler {
		public ArrivalsScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		override public void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="SurroundingsAgent", id="31", type="Start"
		public void ProcessStart(MessageForm msg) {
			MyMessage message = (MyMessage)msg;
			message.Code = Mc.NewArrival;
			message.Patient = new Patient(MySim);
			Notice(message);
			Hold(MyAgent.GetArrivalsFrequency(), message.CreateCopy());
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.NewArrival: {
					((MyMessage)message).Patient = new Patient(MySim);
					Notice(message);
					Hold(MyAgent.GetArrivalsFrequency(), message.CreateCopy());
					break;
				}
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				case Mc.Start:
					ProcessStart(message);
					break;

				default:
					ProcessDefault(message);
					break;
			}
		}
		//meta! tag="end"
		public new SurroundingsAgent MyAgent {
			get {
				return (SurroundingsAgent)base.MyAgent;
			}
		}
	}
}