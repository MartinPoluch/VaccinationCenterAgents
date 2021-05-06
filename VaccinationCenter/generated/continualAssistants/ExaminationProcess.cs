using OSPABA;
using simulation;
using agents;
using VaccinationCenter.entities;

namespace continualAssistants {
	//meta! id="82"
	public class ExaminationProcess : Process {
		public ExaminationProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		override public void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="ExaminationAgent", id="83", type="Start"
		public void ProcessStart(MessageForm message) {
			ServiceEntity service = ((Message)message).Service;
			message.Code = Mc.ExaminationProcessEnd;
			Hold(service.GenerateDuration(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.ExaminationProcessEnd: {
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