using OSPABA;
using simulation;
using agents;
namespace continualAssistants {
	//meta! id="40"
	public class LunchProcess : Process {
		public LunchProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="LunchAgent", id="41", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.EndOfLunch;
			double lunchDuration = ((VacCenterSimulation)MySim).SimParameter.ValidationMode
				? 0
				: MyAgent.LunchDurationGenerator.Sample();
			Hold(lunchDuration, message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.EndOfLunch: {
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