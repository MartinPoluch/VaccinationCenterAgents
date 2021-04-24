using System;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.common;

namespace managers {
	//meta! id="5"
	public class ExaminationManager : ServiceManager {
		public ExaminationManager(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="DoctorLunchScheduler", id="89", type="Notice"
		public void ProcessDoctorLunchBreak(MessageForm message) {
		}

		//meta! sender="ExaminationProcess", id="83", type="Finish"
		public void ProcessFinishExaminationProcess(MessageForm message) {
		
		}

		//meta! sender="DoctorLunchScheduler", id="87", type="Finish"
		public void ProcessFinishDoctorLunchScheduler(MessageForm message) {
		}

		//meta! sender="VacCenterAgent", id="57", type="Notice"
		public void ProcessDoctorEndBreak(MessageForm message) {
		}

		//meta! sender="ExaminationProcess", id="88", type="Notice"
		public void ProcessExaminationProcessEnd(MessageForm message) {
			EndOfService((MyMessage)message);
			MyMessage endOfExamination = (MyMessage)message.CreateCopy();
			endOfExamination.Service = null;
			endOfExamination.Code = Mc.Examination;
			Response(endOfExamination);
		}

		//meta! sender="VacCenterAgent", id="21", type="Request"
		public void ProcessExamination(MessageForm message) {
			GoToServiceOrQueue((MyMessage)message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init() {
		}

		override public void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				case Mc.Finish:
					switch (message.Sender.Id) {
						case SimId.ExaminationProcess:
							ProcessFinishExaminationProcess(message);
							break;

						case SimId.DoctorLunchScheduler:
							ProcessFinishDoctorLunchScheduler(message);
							break;
					}
					break;

				case Mc.Examination:
					ProcessExamination(message);
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