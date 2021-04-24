using System;
using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers {
	//meta! id="3"
	public class VacCenterManager : Manager {
		public VacCenterManager(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="MovementAgent", id="105", type="Response"
		public void ProcessFromExamination(MessageForm message) {
		}

		//meta! sender="MovementAgent", id="106", type="Response"
		public void ProcessFromVaccination(MessageForm message) {
		}

		//meta! sender="ExaminationAgent", id="26", type="Notice"
		public void ProcessDoctorStartBreak(MessageForm message) {
		}

		//meta! sender="RegistrationAgent", id="25", type="Notice"
		public void ProcessAdminStartBreak(MessageForm message) {
		}

		//meta! sender="VaccinationAgent", id="22", type="Response"
		public void ProcessVaccination(MessageForm message) {
		}

		//meta! sender="VaccinationAgent", id="27", type="Notice"
		public void ProcessNurseStartBreak(MessageForm message) {
		}

		//meta! sender="RegistrationAgent", id="20", type="Response"
		public void ProcessRegistration(MessageForm message) {
		}

		//meta! sender="LunchAgent", id="28", type="Response"
		public void ProcessLunchBreak(MessageForm message) {
		}

		//meta! sender="MovementAgent", id="104", type="Response"
		public void ProcessFromRegistration(MessageForm message) {
		}

		//meta! sender="ModelAgent", id="19", type="Notice"
		public void ProcessPatientEnterCenter(MessageForm message) {
			message.Addressee = MySim.FindAgent(SimId.RegistrationAgent);
			message.Code = Mc.Registration;
			Request(message);
		}

		//meta! sender="ExaminationAgent", id="21", type="Response"
		public void ProcessExamination(MessageForm message) {
		}

		//meta! sender="WaitingAgent", id="23", type="Response"
		public void ProcessWaiting(MessageForm message) {
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
				case Mc.PatientEnterCenter:
					ProcessPatientEnterCenter(message);
					break;

				case Mc.FromVaccination:
					ProcessFromVaccination(message);
					break;

				case Mc.Vaccination:
					ProcessVaccination(message);
					break;

				case Mc.NurseStartBreak:
					ProcessNurseStartBreak(message);
					break;

				case Mc.Registration:
					ProcessRegistration(message);
					break;

				case Mc.FromExamination:
					ProcessFromExamination(message);
					break;

				case Mc.Examination:
					ProcessExamination(message);
					break;

				case Mc.LunchBreak:
					ProcessLunchBreak(message);
					break;

				case Mc.FromRegistration:
					ProcessFromRegistration(message);
					break;

				case Mc.Waiting:
					ProcessWaiting(message);
					break;

				case Mc.DoctorStartBreak:
					ProcessDoctorStartBreak(message);
					break;

				case Mc.AdminStartBreak:
					ProcessAdminStartBreak(message);
					break;

				default:
					ProcessDefault(message);
					break;
			}
		}
		//meta! tag="end"
		public new VacCenterAgent MyAgent {
			get {
				return (VacCenterAgent)base.MyAgent;
			}
		}
	}
}