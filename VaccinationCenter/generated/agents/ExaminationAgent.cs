using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="5"
	public class ExaminationAgent : Agent
	{
		public ExaminationAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
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
			new ExaminationProcess(SimId.ExaminationProcess, MySim, this);
			new DoctorLunchScheduler(SimId.DoctorLunchScheduler, MySim, this);
			AddOwnMessage(Mc.DoctorLunchBreak);
			AddOwnMessage(Mc.DoctorEndBreak);
			AddOwnMessage(Mc.Examination);
			AddOwnMessage(Mc.ExaminationProcessEnd);
		}
		//meta! tag="end"
	}
}