using System;
using System.Collections.Generic;
using OSPABA;
using simulation;
using agents;
using OSPRNG;
using Priority_Queue;
using VaccinationCenter.entities;
using VaccinationCenter.models;

namespace continualAssistants {
	//meta! id="30"
	public class ArrivalsScheduler : Scheduler {

		public ArrivalsScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		private List<Patient> PatientBeforeOpening { get; set; }

		private SimplePriorityQueue<Patient, double> PatientArrivals { get; set; }

		private SimParameter GetSimParameter() {
			return ((MySimulation)MySim).SimParameter;
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			Patient.ResetPatientId();
			if (GetSimParameter().EarlyArrivals) {
				PatientBeforeOpening = new List<Patient>();
				PatientArrivals = new SimplePriorityQueue<Patient, double>();
				PrepareEarlyPatientArrivals();
			}
		}

		private void PrepareEarlyPatientArrivals() {
			double timeBetweenArrivals = MyAgent.GetArrivalsFrequency();
			double simTime = 0;
			for (int i = 0; i < GetSimParameter().NumOfPatients; i++) {
				Patient patient = new Patient(MySim);
				if (MyAgent.PatientWilComeEarly()) {
					double realArrivalTime = simTime - MyAgent.EarlyArrivalGen.Sample();
					if (realArrivalTime <= 0) {
						PatientBeforeOpening.Add(patient);
					}
					else {
						PatientArrivals.Enqueue(patient, realArrivalTime);
					}
				}
				else {
					if (simTime == 0) { // only for first patient, if he will come at 8:00
						PatientBeforeOpening.Add(patient);
					}
					else {
						PatientArrivals.Enqueue(patient, simTime);
					}
				}
				simTime += timeBetweenArrivals;
			}
		}


		//meta! sender="SurroundingsAgent", id="31", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.NewArrival;
			if (GetSimParameter().EarlyArrivals) {
				ArrivalOfPatientBeforeOpening((MyMessage)message);
			}
			else {
				NewRegularArrivalProcess((MyMessage)message);
			}
		}

		private void NewRegularArrivalProcess(MyMessage newArrival) {
			Patient patient = new Patient(MySim); // incrementation of patient Id
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

		private void ArrivalOfPatientBeforeOpening(MyMessage message) {
			foreach (Patient patient in PatientBeforeOpening) {
				//TODO, implement
			}
		}

		private void NewEarlyArrivalProcess(MyMessage newArrival) {
			//TODO, implement
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.NewArrival: {
					MyMessage myMessage = (MyMessage)message;
					if (GetSimParameter().EarlyArrivals) {
						NewEarlyArrivalProcess(myMessage);
					}
					else {
						NewRegularArrivalProcess(myMessage);
					}
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