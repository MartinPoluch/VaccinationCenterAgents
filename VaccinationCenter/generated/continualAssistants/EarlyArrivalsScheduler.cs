using System;
using System.Collections.Generic;
using System.Diagnostics;
using OSPABA;
using simulation;
using agents;
using Priority_Queue;
using VaccinationCenter.entities;
using VaccinationCenter.models;

namespace continualAssistants {
	//meta! id="185"
	public class EarlyArrivalsScheduler : Scheduler {

		public EarlyArrivalsScheduler(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent) {
		}

		private List<Patient> PatientBeforeOpening { get; set; }

		private SimplePriorityQueue<Patient, double> PatientArrivals { get; set; }

		private SimParameter GetSimParameter() {
			return ((MySimulation)MySim).SimParameter;
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			if (GetSimParameter().EarlyArrivals) {
				Patient.ResetPatientId();
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
				Patient patient = PatientArrivals.Dequeue();
				Debug.Assert(patient.ArrivalTime >= MySim.CurrentTime, "This patient should already be in the system. Arrival time is smaller then current time");
				newArrival.Patient = patient;
				double durationFromNowToArrival = patient.ArrivalTime - MySim.CurrentTime;
				Hold(durationFromNowToArrival, newArrival);
			}
		}

		//meta! sender="SurroundingsAgent", id="186", type="Start"
		public void ProcessStart(MessageForm message) {
			message.Code = Mc.NewEarlyArrival;
			ArrivalOfPatientBeforeOpening((MyMessage)message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
				case Mc.NewEarlyArrival: {
					MyMessage myMessage = (MyMessage)message;
					if (MyAgent.PatientIsMissing()) {
						MyAgent.PatientsMissing++;
					}
					else {
						Notice(myMessage.CreateCopy());
					}
					NewEarlyArrivalProcess(myMessage);
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
