using System;
using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace managers {
	//meta! id="6"
	public class RegistrationManager : ServiceManager {

		public RegistrationManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication

		}

		protected override void SendServiceToLunch(MyMessage myMessage) {
			myMessage.Code = Mc.AdminStartBreak;
			myMessage.Addressee = MyAgent.FindAssistant(SimId.VacCenterAgent);
			Notice(myMessage);
		}

		//meta! sender="VacCenterAgent", id="20", type="Request"
		public void ProcessRegistration(MessageForm message) {
			GoToServiceOrQueue((MyMessage)message);
		}

		//meta! sender="AdminLunchScheduler", id="73", type="Notice"
		public void ProcessAdminLunchBreak(MessageForm message) {
			StartOfLunchBreak((MyMessage)message);
		}

		//meta! sender="VacCenterAgent", id="56", type="Notice"
		public void ProcessAdminEndBreak(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			ServiceEntity service = myMessage.Service;
			service.EndLunchBreak();
			ServiceNextPatient(myMessage);
		}

		//meta! sender="AdminLunchScheduler", id="71", type="Finish"
		public void ProcessFinishAdminLunchScheduler(MessageForm message) {
		}

		//meta! sender="RegistrationProcess", id="69", type="Finish"
		public void ProcessFinishRegistrationProcess(MessageForm message) {
		}

		//meta! sender="RegistrationProcess", id="72", type="Notice"
		public void ProcessRegistrationProcessEnd(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			FreeServiceAndReference(myMessage); // does not resend message, no copy needed
			ServiceNextPatient((MyMessage)myMessage.CreateCopy());

			Debug.Assert(myMessage.Service == null, "Service should be null.");
			MyMessage endOfRegistration = myMessage;
			endOfRegistration.Code = Mc.Registration;
			Response(endOfRegistration);
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
						case SimId.RegistrationProcess:
							ProcessFinishRegistrationProcess(message);
							break;

						case SimId.AdminLunchScheduler:
							ProcessFinishAdminLunchScheduler(message);
							break;
					}
					break;

				case Mc.Registration:
					ProcessRegistration(message);
					break;

				case Mc.AdminEndBreak:
					ProcessAdminEndBreak(message);
					break;

				case Mc.RegistrationProcessEnd:
					ProcessRegistrationProcessEnd(message);
					break;

				case Mc.AdminLunchBreak:
					ProcessAdminLunchBreak(message);
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