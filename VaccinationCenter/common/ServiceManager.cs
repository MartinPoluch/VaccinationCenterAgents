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
			//TODO, follow rule tha half of services must be present
			//bool serviceCanGoToLunch = (MySim.CurrentTime > MyAgent.GetStartTimeOfLunch());
			//if (MySim.CurrentTime > MyAgent.GetStartTimeOfLunch()) {
			//	ServiceEntity service = PickServiceForLunch();
			//	if (service != null) {
			//		MyMessage lunchMessage = (MyMessage)message.CreateCopy();
			//		service.StartLunchBreak();
			//		lunchMessage.Service = service;
			//		SendServiceToLunch(lunchMessage);
			//	}
			//}


			if (!MyAgent.Queue.IsEmpty()) {
				message.Patient = MyAgent.Queue.Dequeue(); // get first patient in queue
				StartService(message); // we know that at least one service is free
			}
		}

		protected void StartOfLunchBreak(MyMessage myMessage) {
			foreach (ServiceEntity service in MyAgent.ServiceEntities) {
				service.StartBeingHungry();
			}

			var freeServices = GetFreeServices(); // nobody has eaten yet, so everybody has HUNGRY state
			if (freeServices.Count > 0) {
				int serviceLimit = MyAgent.ServiceEntities.Count / 2; // at least half of services must be in the room all the time
				int numOfServiceToLunch = (freeServices.Count <= serviceLimit) ? freeServices.Count : serviceLimit; // how many services can go to lunch
				for (int i = 0; i < numOfServiceToLunch; i++) {
					MyMessage lunchMessage = (i == (numOfServiceToLunch - 1)) // last sent service
						? myMessage // don't want to copy last message
						: (MyMessage)myMessage.CreateCopy(); // I need copy messages

					ServiceEntity service = freeServices[i];
					service.StartLunchBreak();
					lunchMessage.Service = service;
					SendServiceToLunch(lunchMessage);
				}
			}
		}

		/**
		 * Pick one service for lunch.
		 * If no service is available, then it will return NULL.
		 */
		private ServiceEntity PickServiceForLunch() {
			foreach (ServiceEntity service in MyAgent.ServiceEntities) {
				if ((service.ServiceStatus == ServiceStatus.Free) && (service.LunchStatus == LunchStatus.Hungry)) {
					return service;
				}
			}

			return null; // no free service available or everybody has already eaten
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
