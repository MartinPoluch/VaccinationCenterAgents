using OSPABA;
using simulation;
using agents;
namespace continualAssistants {
	//meta! id="93"
	public class NursesLunchScheduler : Scheduler {
		public NursesLunchScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			MessageForm startScheduler = new Message(MySim);
			startScheduler.Code = Mc.NurseLunchBreak;
			Hold(MyAgent.GetStartTimeOfLunch(), startScheduler);
		}

		//meta! sender="VaccinationAgent", id="94", type="Start"
		public void ProcessStart(MessageForm message) {
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.NurseLunchBreak: {
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
		public new VaccinationAgent MyAgent {
			get {
				return (VaccinationAgent)base.MyAgent;
			}
		}
	}
}