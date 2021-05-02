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
						myMessage.Code = Mc.ExaminationStart;
						Notice(myMessage);
						break;
					}
				case ServiceType.Doctor: {
						myMessage.Addressee = MySim.FindAgent(SimId.VaccinationAgent);
						myMessage.Code = Mc.VaccinationStart;
						Notice(myMessage);
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

		private void SendServiceToLunch(MessageForm message) {
			message.Addressee = MySim.FindAgent(SimId.LunchAgent);
			message.Code = Mc.LunchBreak;
			Request(message);
		}

		//meta! sender="ExaminationAgent", id="26", type="Notice"
		public void ProcessDoctorStartBreak(MessageForm message) {
			SendServiceToLunch(message);
		}

		//meta! sender="RegistrationAgent", id="25", type="Notice"
		public void ProcessAdminStartBreak(MessageForm message) {
			SendServiceToLunch(message);
		}

		//meta! sender="VaccinationAgent", id="27", type="Notice"
		public void ProcessNurseStartBreak(MessageForm message) {
			SendServiceToLunch(message);
		}

		//meta! sender="LunchAgent", id="28", type="Response"
		public void ProcessLunchBreak(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			ServiceEntity service = myMessage.Service;
			switch (service.ServiceType) {
				case ServiceType.AdminWorker: {
						myMessage.Addressee = MySim.FindAgent(SimId.RegistrationAgent);
						myMessage.Code = Mc.AdminEndBreak;
						Notice(myMessage);
						break;
					}
				case ServiceType.Doctor: {
						myMessage.Addressee = MySim.FindAgent(SimId.ExaminationAgent);
						myMessage.Code = Mc.DoctorEndBreak;
						Notice(myMessage);
						break;
					}
				case ServiceType.Nurse: {
						myMessage.Addressee = MySim.FindAgent(SimId.VaccinationAgent);
						myMessage.Code = Mc.NurseEndBreak;
						Notice(myMessage);
						break;
					}
			}
		}

		//meta! sender="ModelAgent", id="19", type="Notice"
		public void ProcessPatientEnterCenter(MessageForm message) {
			MyAgent.PatientsInTheSystem++;
			message.Addressee = MySim.FindAgent(SimId.RegistrationAgent);
			message.Code = Mc.RegistrationStart;
			Notice(message);
		}

		//meta! userInfo="Removed from model"
		public void ProcessExamination(MessageForm message) {
			MovePatientToAnotherRoom(message);
		}

		//meta! sender="WaitingAgent", id="23", type="Response"
		public void ProcessWaiting(MessageForm message) {
			MyAgent.PatientsInTheSystem--;
			message.Addressee = MySim.FindAgent(SimId.ModelAgent);
			message.Code = Mc.PatientLeftCenter;
			Notice(message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! sender="ExaminationAgent", id="165", type="Notice"
		public void ProcessExaminationEnd(MessageForm message) {
			MovePatientToAnotherRoom(message);
		}

		//meta! sender="VaccinationAgent", id="169", type="Notice"
		public void ProcessVaccinationEnd(MessageForm message) {
			MovePatientToAnotherRoom(message);
		}

		//meta! sender="RegistrationAgent", id="164", type="Notice"
		public void ProcessRegistrationEnd(MessageForm message) {
			MovePatientToAnotherRoom(message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init() {
		}

		public override void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				case Mc.RegistrationEnd:
					ProcessRegistrationEnd(message);
					break;

				case Mc.AdminStartBreak:
					ProcessAdminStartBreak(message);
					break;

				case Mc.ExaminationEnd:
					ProcessExaminationEnd(message);
					break;

				case Mc.PatientEnterCenter:
					ProcessPatientEnterCenter(message);
					break;

				case Mc.LunchBreak:
					ProcessLunchBreak(message);
					break;

				case Mc.Waiting:
					ProcessWaiting(message);
					break;

				case Mc.VaccinationEnd:
					ProcessVaccinationEnd(message);
					break;

				case Mc.MoveToAnotherRoom:
					ProcessMoveToAnotherRoom(message);
					break;

				case Mc.NurseStartBreak:
					ProcessNurseStartBreak(message);
					break;

				case Mc.DoctorStartBreak:
					ProcessDoctorStartBreak(message);
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