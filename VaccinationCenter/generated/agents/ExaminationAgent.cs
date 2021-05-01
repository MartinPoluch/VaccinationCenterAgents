using OSPABA;
using simulation;
using managers;
using continualAssistants;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace agents
{
	//meta! id="5"
	public class ExaminationAgent : ServiceAgent
	{
		public ExaminationAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			ExaminationProcess = (Process)FindAssistant(SimId.ExaminationProcess);
			LunchBreakScheduler = (Scheduler)FindAssistant(SimId.DoctorLunchScheduler);
		}

		private Process ExaminationProcess { get; }

		private Scheduler LunchBreakScheduler { get; }

		protected override ServiceEntity CreateEntity() {
			return new Doctor(MySim);
		}

		public override ServiceType GetServiceType() {
			return ServiceType.Doctor;
		}

		public override Process GetServiceProcess() {
			return ExaminationProcess;
		}

		public override double GetStartTimeOfLunch() {
			return (60 * 60 * 3) + (60 * 45); // 11:45
		}

		public override Scheduler GetLunchBreakScheduler() {
			return LunchBreakScheduler;
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ExaminationManager(SimId.ExaminationManager, MySim, this);
			new DoctorLunchScheduler(SimId.DoctorLunchScheduler, MySim, this);
			new ExaminationProcess(SimId.ExaminationProcess, MySim, this);
			AddOwnMessage(Mc.ExaminationStart);
			AddOwnMessage(Mc.DoctorLunchBreak);
			AddOwnMessage(Mc.DoctorEndBreak);
			AddOwnMessage(Mc.ExaminationProcessEnd);
		}
		//meta! tag="end"
	}
}