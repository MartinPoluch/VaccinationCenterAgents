using OSPABA;
using simulation;
using agents;
using OSPRNG;
using VaccinationCenter.entities;

namespace continualAssistants {
	//meta! id="99"
	public class MovementProcess : Process {
		public MovementProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="MovementAgent", id="100", type="Start"
		public void ProcessStart(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			Patient patient = myMessage.Patient;
			myMessage.Code = Mc.EndOfMove;
			RNG<double> generator = MyAgent.MoveDurationGenerators[patient.LastVisitedService];
			double moveDuration = ((MySimulation)MySim).SimParameter.ValidationMode 
				? 0 
				: generator.Sample();
			Hold(moveDuration, myMessage);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.EndOfMove: {
					Notice(message);
					break;
				}
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public override void ProcessMessage(MessageForm message) {
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
		public new MovementAgent MyAgent {
			get {
				return (MovementAgent)base.MyAgent;
			}
		}
	}
}