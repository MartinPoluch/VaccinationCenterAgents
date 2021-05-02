using System;
using System.Collections.Generic;
using System.Diagnostics;
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
					patient.ArrivalTime = realArrivalTime;
					if (realArrivalTime <= 0) {
						PatientBeforeOpening.Add(patient);
					}
					else {
						PatientArrivals.Enqueue(patient, realArrivalTime);
					}
				}
				else {
					patient.ArrivalTime = simTime;
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

		private void ArrivalOfPatientBeforeOpening(MyMessage message) {
			foreach (Patient patient in PatientBeforeOpening) {
				if (MyAgent.PatientIsMissing()) {
					MyAgent.PatientsMissing++;
				}
				else {
					MyMessage newArrival = (MyMessage)message.CreateCopy();
					newArrival.Patient = patient;
					Notice(newArrival);
				}
			}
			NewEarlyArrivalProcess(message);
		}

		private void NewEarlyArrivalProcess(MyMessage newArrival) {
			if (PatientArrivals.Count > 0) {
				
				//TODO refactor needed, only for validation purposes, missing patients not included yet
				Patient patient = PatientArrivals.First;
				double arrivalTime = PatientArrivals.GetPriority(patient);
				var p = PatientArrivals.Dequeue();
				newArrival.Patient = patient;

				Debug.Assert(p == patient, "Not correct patient");
				Debug.Assert(patient.ArrivalTime == arrivalTime, "Patient arrival time is invalid.");
				Debug.Assert(arrivalTime >= MySim.CurrentTime, "This patient should already be in the system. Arrival time is smaller then current time");
				double durationFromNowToArrival = arrivalTime - MySim.CurrentTime;
				Hold(durationFromNowToArrival, newArrival);
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.NewArrival: {
					MyMessage myMessage = (MyMessage)message;
					if (GetSimParameter().EarlyArrivals) {
						Debug.Assert(myMessage.Patient.ArrivalTime == MySim.CurrentTime, "Not good time for arrival.");
						Notice(myMessage.CreateCopy());
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