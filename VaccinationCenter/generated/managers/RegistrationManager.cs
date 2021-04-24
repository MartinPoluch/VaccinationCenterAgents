using System;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.common;

namespace managers {
	//meta! id="6"
	public class RegistrationManager : ServiceManager {
		public RegistrationManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		override public void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null) {
				PetriNet.Clear();
			}
		}

		//meta! sender="VacCenterAgent", id="20", type="Request"
		public void ProcessRegistration(MessageForm message) {
			GoToServiceOrQueue((MyMessage)message);
		}

		//meta! sender="AdminLunchScheduler", id="73", type="Notice"
		public void ProcessLunchBreakScheduled(MessageForm message) {
		}

		//meta! sender="VacCenterAgent", id="56", type="Notice"
		public void ProcessAdminEndBreak(MessageForm message) {
		}

		//meta! sender="AdminLunchScheduler", id="71", type="Finish"
		public void ProcessFinishAdminLunchScheduler(MessageForm message) {
		}

		//meta! sender="RegistrationProcess", id="69", type="Finish"
		public void ProcessFinishRegistrationProcess(MessageForm message) {
		}

		//meta! sender="RegistrationProcess", id="72", type="Notice"
		public void ProcessServiceProcessDone(MessageForm message) {
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init() {
		}

		public override void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				case Mc.Finish:
					switch (message.Sender.Id) {
						case SimId.AdminLunchScheduler:
							ProcessFinishAdminLunchScheduler(message);
							break;

						case SimId.RegistrationProcess:
							ProcessFinishRegistrationProcess(message);
							break;
					}
					break;

				case Mc.Registration:
					ProcessRegistration(message);
					break;

				case Mc.ServiceProcessDone:
					ProcessServiceProcessDone(message);
					break;

				case Mc.AdminEndBreak:
					ProcessAdminEndBreak(message);
					break;

				case Mc.LunchBreakScheduled:
					ProcessLunchBreakScheduled(message);
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