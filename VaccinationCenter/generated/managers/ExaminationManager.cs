using System;
using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace managers {
	//meta! id="5"
	public class ExaminationManager : ServiceManager {
		public ExaminationManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		protected override void SendServiceToLunch(Message myMessage) {
			myMessage.Code = Mc.DoctorStartBreak;
			myMessage.Addressee = MyAgent.FindAssistant(SimId.VacCenterAgent);
			Notice(myMessage);
		}

		//meta! sender="DoctorLunchScheduler", id="89", type="Notice"
		public void ProcessDoctorLunchBreak(MessageForm message) {
			StartOfLunchBreak((Message)message);
		}

		//meta! sender="ExaminationProcess", id="83", type="Finish"
		public void ProcessFinishExaminationProcess(MessageForm message) {

		}

		//meta! sender="DoctorLunchScheduler", id="87", type="Finish"
		public void ProcessFinishDoctorLunchScheduler(MessageForm message) {
		}

		//meta! sender="VacCenterAgent", id="57", type="Notice"
		public void ProcessDoctorEndBreak(MessageForm message) {
			Message myMessage = (Message)message;
			EndServiceLunchBreakAndReference(myMessage);
			ServiceNextPatientOrGoToLunch(myMessage);
		}

		//meta! sender="ExaminationProcess", id="88", type="Notice"
		public void ProcessExaminationProcessEnd(MessageForm message) {
			Message myMessage = (Message)message;
			FreeServiceAndReference(myMessage); // does not resend message, no copy needed
			ServiceNextPatientOrGoToLunch((Message)myMessage.CreateCopy());

			Debug.Assert(myMessage.Service == null, "Service should be null.");
			Message endOfExamination = myMessage;
			endOfExamination.Addressee = MySim.FindAgent(SimId.VacCenterAgent);
			endOfExamination.Code = Mc.ExaminationEnd;
			Notice(endOfExamination);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! sender="VacCenterAgent", id="166", type="Notice"
		public void ProcessExaminationStart(MessageForm message) {
			GoToServiceOrQueue((Message)message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.ExaminationStart:
				ProcessExaminationStart(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.DoctorLunchScheduler:
					ProcessFinishDoctorLunchScheduler(message);
				break;

				case SimId.ExaminationProcess:
					ProcessFinishExaminationProcess(message);
				break;
				}
			break;

			case Mc.DoctorLunchBreak:
				ProcessDoctorLunchBreak(message);
			break;

			case Mc.DoctorEndBreak:
				ProcessDoctorEndBreak(message);
			break;

			case Mc.ExaminationProcessEnd:
				ProcessExaminationProcessEnd(message);
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