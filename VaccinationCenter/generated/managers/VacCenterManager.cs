using System;
using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.entities;

namespace managers {
	//meta! id="3"
	public class VacCenterManager : Manager {
		public VacCenterManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		private void MovePatientToAnotherRoom(MessageForm message) {
			message.Addressee = MySim.FindAgent(SimId.MovementAgent);
			message.Code = Mc.MoveToAnotherRoom;
			Request(message);
		}

		//meta! sender="MovementAgent", id="105", type="Response"
		public void ProcessMoveToAnotherRoom(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			Patient patient = myMessage.Patient;
			switch (patient.LastVisitedService) {
				case ServiceType.AdminWorker: {
					myMessage.Addressee = MySim.FindAgent(SimId.ExaminationAgent);
					myMessage.Code = Mc.Examination;
					Request(myMessage);
					break;
				}
				case ServiceType.Doctor: {
					myMessage.Addressee = MySim.FindAgent(SimId.VaccinationAgent);
					myMessage.Code = Mc.Vaccination;
					Request(myMessage);
					break;
				}
				case ServiceType.Nurse: {
					myMessage.Addressee = MySim.FindAgent(SimId.WaitingAgent);
					myMessage.Code = Mc.Waiting;
					Request(myMessage);
					break;
				}
				default: {
					Debug.Fail("Patient did not visit any known service and he tries to move.");
					break;
				}
			}
		}


		//meta! sender="ExaminationAgent", id="26", type="Notice"
		public void ProcessDoctorStartBreak(MessageForm message) {
		}

		//meta! sender="RegistrationAgent", id="25", type="Notice"
		public void ProcessAdminStartBreak(MessageForm message) {
		}

		//meta! sender="VaccinationAgent", id="22", type="Response"
		public void ProcessVaccination(MessageForm message) {
			MovePatientToAnotherRoom(message);
		}

		//meta! sender="VaccinationAgent", id="27", type="Notice"
		public void ProcessNurseStartBreak(MessageForm message) {
		}

		//meta! sender="RegistrationAgent", id="20", type="Response"
		public void ProcessRegistration(MessageForm message) {
			MovePatientToAnotherRoom(message);
		}

		//meta! sender="LunchAgent", id="28", type="Response"
		public void ProcessLunchBreak(MessageForm message) {
		}

		//meta! sender="ModelAgent", id="19", type="Notice"
		public void ProcessPatientEnterCenter(MessageForm message) {
			message.Addressee = MySim.FindAgent(SimId.RegistrationAgent);
			message.Code = Mc.Registration;
			Request(message);
		}

		//meta! sender="ExaminationAgent", id="21", type="Response"
		public void ProcessExamination(MessageForm message) {
			MovePatientToAnotherRoom(message);
		}

		//meta! sender="WaitingAgent", id="23", type="Response"
		public void ProcessWaiting(MessageForm message) {
			message.Addressee = MySim.FindAgent(SimId.ModelAgent);
			message.Code = Mc.PatientLeftCenter;
			Notice(message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		public override void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Examination:
				ProcessExamination(message);
			break;

			case Mc.Registration:
				ProcessRegistration(message);
			break;

			case Mc.LunchBreak:
				ProcessLunchBreak(message);
			break;

			case Mc.Vaccination:
				ProcessVaccination(message);
			break;

			case Mc.Waiting:
				ProcessWaiting(message);
			break;

			case Mc.AdminStartBreak:
				ProcessAdminStartBreak(message);
			break;

			case Mc.DoctorStartBreak:
				ProcessDoctorStartBreak(message);
			break;

			case Mc.NurseStartBreak:
				ProcessNurseStartBreak(message);
			break;

			case Mc.PatientEnterCenter:
				ProcessPatientEnterCenter(message);
			break;

			case Mc.MoveToAnotherRoom:
				ProcessMoveToAnotherRoom(message);
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