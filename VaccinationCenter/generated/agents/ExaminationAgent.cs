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
			ExaminationProcess = (ContinualAssistant)FindAssistant(SimId.ExaminationProcess);
		}

		private ContinualAssistant ExaminationProcess { get; }

		protected override ServiceEntity CreateEntity() {
			return new Doctor(MySim);
		}

		public override ServiceType GetServiceType() {
			return ServiceType.Doctor;
		}

		public override ContinualAssistant GetServiceProcess() {
			return ExaminationProcess;
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
			AddOwnMessage(Mc.DoctorLunchBreak);
			AddOwnMessage(Mc.DoctorEndBreak);
			AddOwnMessage(Mc.Examination);
			AddOwnMessage(Mc.ExaminationProcessEnd);
		}
		//meta! tag="end"
	}
}