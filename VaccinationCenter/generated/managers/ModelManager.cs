using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers {
	//meta! id="1"
	public class ModelManager : Manager {
		public ModelManager(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="VacCenterAgent", id="48", type="Notice"
		public void ProcessPatientLeftCenter(MessageForm message) {
		}

		//meta! sender="SurroundingsAgent", id="18", type="Notice"
		public void ProcessPatientArrival(MessageForm message) {
			message.Addressee = MySim.FindAgent(SimId.VacCenterAgent);
			message.Code = Mc.PatientEnterCenter;
			Notice(message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init() {
		}

		override public void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				case Mc.PatientLeftCenter:
					ProcessPatientLeftCenter(message);
					break;

				case Mc.PatientArrival:
					ProcessPatientArrival(message);
					break;

				default:
					ProcessDefault(message);
					break;
			}
		}
		//meta! tag="end"
		public new ModelAgent MyAgent {
			get {
				return (ModelAgent)base.MyAgent;
			}
		}
	}
}