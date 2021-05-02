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

		//meta! sender="AdminLunchScheduler", id="73", type="Notice"
		public void ProcessAdminLunchBreak(MessageForm message) {
			StartOfLunchBreak((MyMessage)message);
		}

		//meta! sender="VacCenterAgent", id="56", type="Notice"
		public void ProcessAdminEndBreak(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			EndServiceLunchBreakAndReference(myMessage);
			ServiceNextPatientOrGoToLunch(myMessage);
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
			MyMessage messageCopy = (MyMessage)myMessage.CreateCopy();
			ServiceNextPatientOrGoToLunch(messageCopy);

			Debug.Assert(myMessage.Service == null, "Service should be null.");
			MyMessage endOfRegistration = myMessage;
			endOfRegistration.Addressee = MySim.FindAgent(SimId.VacCenterAgent);
			endOfRegistration.Code = Mc.RegistrationEnd;
			Notice(endOfRegistration);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! sender="VacCenterAgent", id="163", type="Notice"
		public void ProcessRegistrationStart(MessageForm message) {
			GoToServiceOrQueue((MyMessage)message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.AdminEndBreak:
				ProcessAdminEndBreak(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.RegistrationProcess:
					ProcessFinishRegistrationProcess(message);
				break;

				case SimId.AdminLunchScheduler:
					ProcessFinishAdminLunchScheduler(message);
				break;
				}
			break;

			case Mc.RegistrationProcessEnd:
				ProcessRegistrationProcessEnd(message);
			break;

			case Mc.AdminLunchBreak:
				ProcessAdminLunchBreak(message);
			break;

			case Mc.RegistrationStart:
				ProcessRegistrationStart(message);
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