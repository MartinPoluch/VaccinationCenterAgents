using OSPABA;
using simulation;
using agents;
using VaccinationCenter.entities;

namespace continualAssistants {
	//meta! id="68"
	public class RegistrationProcess : Process {
		public RegistrationProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="RegistrationAgent", id="69", type="Start"
		public void ProcessStart(MessageForm message) {
			ServiceEntity service = ((MyMessage)message).Service;
			message.Code = Mc.RegistrationProcessEnd;
			Hold(service.GenerateDuration(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.RegistrationProcessEnd: {
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
		public new RegistrationAgent MyAgent {
			get {
				return (RegistrationAgent)base.MyAgent;
			}
		}
	}
}