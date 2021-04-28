using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using VaccinationCenter.entities;
using Process = OSPABA.Process;

namespace continualAssistants {
	//meta! id="149"
	public class NurseMoveProcess : Process {
		public NurseMoveProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="RefillAgent", id="150", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.EndOfNurseMove;
			Hold(MyAgent.MoveDurationGenerator.Sample(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.EndOfNurseMove: {
					Notice(message);
					break;
				}
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public override void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				case Mc.Start:
					ProcessStart(message);
					break;

				default:
					ProcessDefault(message);
					break;
			}
		}
		//meta! tag="end"
		public new RefillAgent MyAgent {
			get {
				return (RefillAgent)base.MyAgent;
			}
		}
	}
}
