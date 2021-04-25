using OSPABA;
using simulation;
using agents;
namespace continualAssistants {
	//meta! id="35"
	public class WaitingProcess : Process {
		public WaitingProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		override public void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="WaitingAgent", id="36", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.EndOfWaiting;
			Hold(MyAgent.GenerateWaitingTime(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.EndOfWaiting: {
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
		public new WaitingAgent MyAgent {
			get {
				return (WaitingAgent)base.MyAgent;
			}
		}
	}
}