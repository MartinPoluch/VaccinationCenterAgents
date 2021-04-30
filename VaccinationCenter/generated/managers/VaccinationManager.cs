using System;
using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using continualAssistants;
using VaccinationCenter.common;
using VaccinationCenter.entities;

namespace managers {
	//meta! id="4"
	public class VaccinationManager : ServiceManager {
		public VaccinationManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		protected override void SendServiceToLunch(MyMessage myMessage) {
			myMessage.Code = Mc.NurseStartBreak;
			myMessage.Addressee = MyAgent.FindAssistant(SimId.VacCenterAgent);
			Notice(myMessage);
		}

		//meta! sender="VacCenterAgent", id="22", type="Request"
		public void ProcessVaccination(MessageForm message) {
			GoToServiceOrQueue((MyMessage)message);
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
			MyMessage myMessage = (MyMessage)message;
			Nurse nurse = myMessage.GetNurse(); // I need keep reference before FreeService call
			nurse.Doses--;
			FreeServiceAndReference(myMessage); // set service reference to NULL and update stats, no message copy needed
			if (nurse.Doses <= 0) {
				Debug.Assert(nurse.Doses == 0, $"nurse should have 0 doses (but has {nurse.Doses} doses)");
				MyMessage refillMessage = (MyMessage)myMessage.CreateCopy();
				refillMessage.Service = nurse; //FreeService() kill old reference, I need assign service reference again
				refillMessage.Addressee = MySim.FindAgent(SimId.RefillAgent);
				refillMessage.Code = Mc.Refill;
				nurse.StartMoveToRefill();
				Request(refillMessage);
			}
			else {
				ServiceNextPatientOrGoToLunch((MyMessage)myMessage.CreateCopy());
			}

			Debug.Assert(myMessage.Service == null, "Service should be null.");
			MyMessage endOfVaccination = myMessage;
			endOfVaccination.Code = Mc.Vaccination;
			Response(endOfVaccination);
		}

		//meta! sender="VacCenterAgent", id="59", type="Notice"
		public void ProcessNurseEndBreak(MessageForm message) {
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		//meta! sender="RefillAgent", id="147", type="Request"
		public void ProcessRefill(MessageForm message) {
			MyMessage myMessage = (MyMessage)message;
			FreeServiceAndReference(myMessage);
			ServiceNextPatientOrGoToLunch(myMessage);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init() {
		}

		public override void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				case Mc.NurseLunchBreak:
					ProcessNurseLunchBreak(message);
					break;
				case Mc.VaccinationProcessEnd:
					ProcessVaccinationProcessEnd(message);
					break;

				case Mc.Refill:
					ProcessRefill(message);
					break;

				case Mc.Vaccination:
					ProcessVaccination(message);
					break;

				case Mc.NurseEndBreak:
					ProcessNurseEndBreak(message);
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