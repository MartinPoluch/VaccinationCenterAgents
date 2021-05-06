using System;
using OSPABA;
using simulation;
using agents;
using VaccinationCenter.entities;

namespace continualAssistants {
	//meta! id="30"
	public class ArrivalsScheduler : Scheduler {

		public ArrivalsScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			if (!((VacCenterSimulation)MySim).SimParameter.EarlyArrivals) {
				Patient.ResetPatientId();
			}
		}

		//meta! sender="SurroundingsAgent", id="31", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.NewArrival;
			Hold(0, message); // simulation starts from zero time
		}

		private void NewArrivalProcess(Message newArrival) {
			Patient patient = new Patient(MySim); // incrementation of patient Id
			patient.ArrivalTime = MySim.CurrentTime;
			if (MyAgent.PatientIsMissing()) {
				MyAgent.PatientsMissing++;
			}
			else {
				newArrival.Patient = patient;
				Notice(newArrival.CreateCopy());
			}

			if (patient.PatientId < MyAgent.PatientsPerDay - 1) { // stop arrivals of customer after last customer came
				Hold(MyAgent.GetArrivalsFrequency(), newArrival);
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.NewArrival: {
						Message myMessage = (Message)message;
						NewArrivalProcess(myMessage);
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
		public new SurroundingsAgent MyAgent {
			get {
				return (SurroundingsAgent)base.MyAgent;
			}
		}
	}
}