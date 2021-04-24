using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers {
	//meta! id="4"
	public class VaccinationManager : Manager {
		public VaccinationManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		override public void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null) {
				PetriNet.Clear();
			}
		}

		//meta! sender="VacCenterAgent", id="22", type="Request"
		public void ProcessVaccination(MessageForm message) {
		}

		//meta! sender="NursesLunchScheduler", id="95", type="Notice"
		public void ProcessNurseLunchBreak(MessageForm message) {
		}

		//meta! sender="NursesLunchScheduler", id="94", type="Finish"
		public void ProcessFinishNursesLunchScheduler(MessageForm message) {
		}

		//meta! sender="VaccinationProcess", id="92", type="Finish"
		public void ProcessFinishVaccinationProcess(MessageForm message) {
		}

		//meta! sender="VaccinationProcess", id="97", type="Notice"
		public void ProcessVaccinationProcessEnd(MessageForm message) {
		}

		//meta! sender="VacCenterAgent", id="59", type="Notice"
		public void ProcessNurseEndBreak(MessageForm message) {
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

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.NurseEndBreak:
				ProcessNurseEndBreak(message);
			break;

			case Mc.NurseLunchBreak:
				ProcessNurseLunchBreak(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.NursesLunchScheduler:
					ProcessFinishNursesLunchScheduler(message);
				break;

				case SimId.VaccinationProcess:
					ProcessFinishVaccinationProcess(message);
				break;
				}
			break;

			case Mc.VaccinationProcessEnd:
				ProcessVaccinationProcessEnd(message);
			break;

			case Mc.Vaccination:
				ProcessVaccination(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new VaccinationAgent MyAgent {
			get {
				return (VaccinationAgent)base.MyAgent;
			}
		}
	}
}