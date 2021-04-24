using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="4"
	public class VaccinationAgent : Agent
	{
		public VaccinationAgent(int id, Simulation mySim, Agent parent) :
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
			new VaccinationManager(SimId.VaccinationManager, MySim, this);
			new NursesLunchScheduler(SimId.NursesLunchScheduler, MySim, this);
			new VaccinationProcess(SimId.VaccinationProcess, MySim, this);
			AddOwnMessage(Mc.Vaccination);
			AddOwnMessage(Mc.LunchBreakScheduled);
			AddOwnMessage(Mc.ServiceProcessDone);
			AddOwnMessage(Mc.NurseEndBreak);
		}
		//meta! tag="end"
	}
}