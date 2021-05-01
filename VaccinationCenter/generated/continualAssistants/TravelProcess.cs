using OSPABA;
using simulation;
using agents;
namespace continualAssistants {
	//meta! id="38"
	public class TravelProcess : Process {
		public TravelProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="LunchAgent", id="39", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.EndOfTravel;
			double travelDuration = ((MySimulation)MySim).SimParameter.ValidationMode
				? 0
				: MyAgent.MoveDurationGenerator.Sample();
			Hold(travelDuration, message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.EndOfTravel: {
					Notice(message);
					break;
				}
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
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