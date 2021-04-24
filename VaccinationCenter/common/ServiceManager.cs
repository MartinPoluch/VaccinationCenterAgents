using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPRNG;
using simulation;
using VaccinationCenter.entities;

namespace VaccinationCenter.common {

	public abstract class ServiceManager : Manager {

		protected ServiceManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
		}

		public override void PrepareReplication() {
			base.PrepareReplication();
			// Setup component for the next replication
		}

		private List<ServiceEntity> GetFreeServices() {
			List<ServiceEntity> freeServices = new List<ServiceEntity>();
			foreach (ServiceEntity service in MyAgent.ServiceEntities) {
				if (service.ServiceStatus == ServiceStatus.Free) {
					freeServices.Add(service);
				}
			}

			return freeServices;
		}

		public ServiceEntity GetFreeService() {
			var freeServices = GetFreeServices();
			Debug.Assert(freeServices.Count > 0, "Patient try to select service, but all services are occupied!");
			if (freeServices.Count == 1) {
				return freeServices[0]; // nie je o com rozhodovat
			}

			UniformDiscreteRNG decisionGen = MyAgent.ServiceDecisions[freeServices.Count];
			int decision = decisionGen.Sample();
			return freeServices[decision];
		}

		protected bool IsAnyServiceFree() {
			foreach (ServiceEntity service in MyAgent.ServiceEntities) {
				if (service.ServiceStatus == ServiceStatus.Free) {
					return true;
				}
			}

			return false;
		}

		private void StartService(MyMessage message) {
			double startOfWaiting = message.Patient.StartOfWaiting[MyAgent.GetServiceType()];
			double waitingTime = MySim.CurrentTime - startOfWaiting;
			MyAgent.WaitingTimes.AddSample(waitingTime);

			ServiceEntity service = GetFreeService();
			service.Occupy();
			message.Service = service;
			message.Addressee = MyAgent.GetServiceProcess();
			StartContinualAssistant(message);
		}

		protected void GoToServiceOrQueue(MyMessage message) {
			Patient patient = message.Patient;
			patient.StartOfWaiting[MyAgent.GetServiceType()] = MySim.CurrentTime;
			if (IsAnyServiceFree()) {
				StartService(message);
			}
			else {
				MyAgent.Queue.Enqueue(patient);
			}
		}

		private void FreeService(MyMessage message) {
			Debug.Assert(message.Service != null, "No service available");
			ServiceEntity service = message.Service;
			message.Service = null; // delete service reference
			service.Free();
		}

		protected void EndOfService(MyMessage message) {
			FreeService(message);
			if (! MyAgent.Queue.IsEmpty()) {
				message.Patient = MyAgent.Queue.Dequeue(); // get first patient in queue
				StartService(message); // we know that at least one service is free
			}
		}

		public override void ProcessMessage(MessageForm message) {
			switch (message.Code) {
			}
		}

		public new ServiceAgent MyAgent {
			get {
				return (ServiceAgent)base.MyAgent;
			}
		}
	}
}
