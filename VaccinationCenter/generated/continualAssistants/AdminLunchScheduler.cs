using System;
using OSPABA;
using simulation;
using agents;
namespace continualAssistants {
	//meta! id="70"
	public class AdminLunchScheduler : Scheduler {
		public AdminLunchScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			MessageForm startScheduler = new MyMessage(MySim);
			startScheduler.Code = Mc.AdminLunchBreak;
			Hold(MyAgent.GetStartTimeOfLunch(), startScheduler);
		}

		//meta! sender="RegistrationAgent", id="71", type="Start"
		public void ProcessStart(MessageForm message) {

		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.AdminLunchBreak: {
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
		public new RegistrationAgent MyAgent {
			get {
				return (RegistrationAgent)base.MyAgent;
			}
		}
	}
}