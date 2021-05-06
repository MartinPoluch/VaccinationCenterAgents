using OSPABA;
using simulation;
using agents;
namespace continualAssistants {
	//meta! id="86"
	public class DoctorLunchScheduler : Scheduler {
		public DoctorLunchScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			MessageForm startScheduler = new Message(MySim);
			startScheduler.Code = Mc.DoctorLunchBreak;
			Hold(MyAgent.GetStartTimeOfLunch(), startScheduler);
		}

		//meta! sender="ExaminationAgent", id="87", type="Start"
		public void ProcessStart(MessageForm message) {
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.DoctorLunchBreak: {
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
		public new ExaminationAgent MyAgent {
			get {
				return (ExaminationAgent)base.MyAgent;
			}
		}
	}
}