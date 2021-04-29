using OSPABA;
using simulation;
using managers;
using continualAssistants;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace agents {
	//meta! id="6"
	public class RegistrationAgent : ServiceAgent {

		public RegistrationAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			RegistrationProcess = (Process)FindAssistant(SimId.RegistrationProcess);
			LunchBreakScheduler = (Scheduler)FindAssistant(SimId.AdminLunchScheduler);
		}

		private Process RegistrationProcess { get; }

		private Scheduler LunchBreakScheduler { get; }

		protected override ServiceEntity CreateEntity() {
			return new AdminWorker(MySim);
		}

		public override ServiceType GetServiceType() {
			return ServiceType.AdminWorker;
		}

		public override Process GetServiceProcess() {
			return RegistrationProcess;
		}

		public override double GetStartTimeOfLunch() {
			return 60 * 60 * 3; // 11:00
		}

		public override Scheduler GetLunchBreakScheduler() {
			return (Scheduler)LunchBreakScheduler;
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new RegistrationManager(SimId.RegistrationManager, MySim, this);
			new AdminLunchScheduler(SimId.AdminLunchScheduler, MySim, this);
			new RegistrationProcess(SimId.RegistrationProcess, MySim, this);
			AddOwnMessage(Mc.Registration);
			AddOwnMessage(Mc.AdminLunchBreak);
			AddOwnMessage(Mc.AdminEndBreak);
			AddOwnMessage(Mc.RegistrationProcessEnd);
		}
		//meta! tag="end"
	}
}