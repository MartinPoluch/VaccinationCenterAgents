using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents {
	//meta! id="3"
	public class VacCenterAgent : Agent {
		public VacCenterAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			PatientsInTheSystem = 0;
		}

		public int PatientsInTheSystem { get; set; }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init() {
			new VacCenterManager(SimId.VacCenterManager, MySim, this);
			AddOwnMessage(Mc.ExaminationEnd);
			AddOwnMessage(Mc.MoveToAnotherRoom);
			AddOwnMessage(Mc.AdminStartBreak);
			AddOwnMessage(Mc.DoctorStartBreak);
			AddOwnMessage(Mc.VaccinationEnd);
			AddOwnMessage(Mc.NurseStartBreak);
			AddOwnMessage(Mc.RegistrationEnd);
			AddOwnMessage(Mc.LunchBreak);
			AddOwnMessage(Mc.PatientEnterCenter);
			AddOwnMessage(Mc.Waiting);
		}
		//meta! tag="end"
	}
}