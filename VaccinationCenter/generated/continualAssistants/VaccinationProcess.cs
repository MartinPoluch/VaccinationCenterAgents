using OSPABA;
using simulation;
using agents;
using VaccinationCenter.entities;

namespace continualAssistants {
	//meta! id="91"
	public class VaccinationProcess : Process {
		public VaccinationProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		override public void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="VaccinationAgent", id="92", type="Start"
		public void ProcessStart(MessageForm message) {
			ServiceEntity service = ((MyMessage)message).Service;
			message.Code = Mc.VaccinationProcessEnd;
			Hold(service.ServiceDurationGen.Sample(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.VaccinationProcessEnd: {
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