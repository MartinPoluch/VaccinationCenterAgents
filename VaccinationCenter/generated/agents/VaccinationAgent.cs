using OSPABA;
using simulation;
using managers;
using continualAssistants;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace agents {
	//meta! id="4"
	public class VaccinationAgent : ServiceAgent {
		public VaccinationAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			VaccinationProcess = (Process)FindAssistant(SimId.VaccinationProcess);
			LunchBreakScheduler = (Scheduler)FindAssistant(SimId.NursesLunchScheduler);
		}

		private Process VaccinationProcess { get; }

		private Scheduler LunchBreakScheduler { get; }

		protected override ServiceEntity CreateEntity() {
			return new Nurse(MySim);
		}

		public override ServiceType GetServiceType() {
			return ServiceType.Nurse;
		}

		public override Process GetServiceProcess() {
			return VaccinationProcess;
		}

		public override double GetStartTimeOfLunch() {
			return (60 * 60 * 5) + (60 * 30); // 13:30
		}

		public override Scheduler GetLunchBreakScheduler() {
			return LunchBreakScheduler;
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new VaccinationManager(SimId.VaccinationManager, MySim, this);
			new NursesLunchScheduler(SimId.NursesLunchScheduler, MySim, this);
			new VaccinationProcess(SimId.VaccinationProcess, MySim, this);
			AddOwnMessage(Mc.VaccinationProcessEnd);
			AddOwnMessage(Mc.NurseLunchBreak);
			AddOwnMessage(Mc.Vaccination);
			AddOwnMessage(Mc.Refill);
			AddOwnMessage(Mc.NurseEndBreak);
		}
		//meta! tag="end"
	}
}