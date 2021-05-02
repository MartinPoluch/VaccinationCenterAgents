using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="3"
	public class VacCenterAgent : Agent
	{
		public VacCenterAgent(int id, Simulation mySim, Agent parent) :
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