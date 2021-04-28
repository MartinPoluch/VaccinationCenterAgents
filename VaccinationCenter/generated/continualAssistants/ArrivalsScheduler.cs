using System;
using OSPABA;
using simulation;
using agents;
using OSPRNG;
using VaccinationCenter.entities;

namespace continualAssistants {
	//meta! id="30"
	public class ArrivalsScheduler : Scheduler {

		public ArrivalsScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			Patient.ResetPatientId();
		}

		//meta! sender="SurroundingsAgent", id="31", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.NewArrival;
			NewArrivalProcess(message);
		}

		private void NewArrivalProcess(MessageForm message) {
			MyMessage newArrival = (MyMessage)message;
			Patient patient = new Patient(MySim); // incrementation of patient Id
			if (MyAgent.PatientIsMissing()) {
				MyAgent.PatientsMissing++;
			}
			else {
				newArrival.Patient = patient;
				Notice(newArrival.CreateCopy());
			}

			if (patient.PatientId < MyAgent.PatientsPerDay - 1) {
				Hold(MyAgent.GetArrivalsFrequency(), newArrival);
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.NewArrival: {
					NewArrivalProcess(message);
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
		public new SurroundingsAgent MyAgent {
			get {
				return (SurroundingsAgent)base.MyAgent;
			}
		}
	}
}