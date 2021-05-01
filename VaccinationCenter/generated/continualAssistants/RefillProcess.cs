using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using VaccinationCenter.entities;
using Process = OSPABA.Process;

namespace continualAssistants {
	//meta! id="154"
	public class RefillProcess : Process {
		public RefillProcess(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		private double GenerateOneRefillDuration() {
			return ((MySimulation)MySim).SimParameter.ValidationMode
				? 0
				: MyAgent.OneRefillGenerator.Sample();
		}

		//meta! sender="RefillAgent", id="155", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.OneRefillDone;
			Hold(GenerateOneRefillDuration(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			switch (message.Code) {
				case Mc.OneRefillDone: {
					Nurse nurse = myMessage.GetNurse();
					nurse.Doses++;
					if (nurse.Doses >= Nurse.MaxDoses) { // already has 20 doses
						Debug.Assert(nurse.Doses == Nurse.MaxDoses);
						myMessage.Code = Mc.EndOfRefill; 
						Notice(myMessage);
					}
					else {
						Hold(GenerateOneRefillDuration(), message); // another dose refill
					}
					break;
				}
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
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