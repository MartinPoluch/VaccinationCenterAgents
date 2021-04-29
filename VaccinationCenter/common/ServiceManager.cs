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

		protected abstract void SendServiceToLunch(MyMessage myMessage);

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
			Debug.Assert(message.Service == null, "Service should be NULL");
			double startOfWaiting = message.Patient.StartOfWaiting[MyAgent.GetServiceType()];
			double waitingTime = MySim.CurrentTime - startOfWaiting;
			MyAgent.WaitingTimeStat.AddSample(waitingTime);

			ServiceEntity service = GetFreeService();
			service.Occupy();
			message.Service = service;
			message.Addressee = MyAgent.GetServiceProcess();
			StartContinualAssistant(message);
		}

		protected void GoToServiceOrQueue(MyMessage message) {
			Patient patient = message.Patient;
			patient.LastVisitedService = MyAgent.GetServiceType();
			patient.StartOfWaiting[MyAgent.GetServiceType()] = MySim.CurrentTime;
			if (IsAnyServiceFree()) {
				StartService(message);
			}
			else {
				MyAgent.Queue.Enqueue(patient);
			}
		}

		protected void FreeServiceAndReference(MyMessage message) {
			Debug.Assert(message.Service != null, "No service available");
			ServiceEntity service = message.Service;
			message.Service = null; // delete service reference
			service.Free();
		}

		protected void ServiceNextPatient(MyMessage message) {
			//TODO handle lunch here, this method is called also after service return from lunch
			if (!MyAgent.Queue.IsEmpty()) {
				message.Patient = MyAgent.Queue.Dequeue(); // get first patient in queue
				StartService(message); // we know that at least one service is free
			}
		}

		protected void StartOfLunchBreak(MyMessage myMessage) {
			foreach (ServiceEntity service in MyAgent.ServiceEntities) {
				service.StartBeingHungry();
			}

			var freeServices = GetFreeServices();
			if (freeServices.Count > 0) {
				//TODO, implement, do not forget copy of message
				ServiceEntity service = freeServices[0];
				myMessage.Service = service;
				service.StartLunchBreak();
				SendServiceToLunch(myMessage); //TODO, only for testing
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
