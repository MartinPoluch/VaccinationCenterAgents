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
			return MyAgent.ServiceEntities
				.Where(x => x.ServiceStatus == ServiceStatus.Free)
				.ToList();
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
			return MyAgent.ServiceEntities.Any(x => x.ServiceStatus == ServiceStatus.Free);
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

		private int GetNumberOfCurrentAvailableServices() {
			return MyAgent.ServiceEntities.Count(x => x.ServiceStatus != ServiceStatus.Lunch);
		}

		private bool CanGoToLunch() {
			int minimalNumOfServices = (int)Math.Ceiling(((double)MyAgent.ServiceEntities.Count) / 2);
			bool isAnybodyFreeAndHungry = MyAgent.ServiceEntities
				.Any(x => x.LunchStatus == LunchStatus.Hungry && x.ServiceStatus == ServiceStatus.Free);
			return (MySim.CurrentTime >= MyAgent.GetStartTimeOfLunch())
			       && (GetNumberOfCurrentAvailableServices() > minimalNumOfServices)
			       && (isAnybodyFreeAndHungry);
		}

		/**
		 * This method is called when service finish its process (end of registration, end of vaccination, end of lunch break, etc.)
		 */
		protected void ServiceNextPatientOrGoToLunch(MyMessage message) {
			Debug.Assert(IsAnyServiceFree(), "At least one service should be free");
			if (CanGoToLunch()) { // if there is someone who want and also can go to lunch
				ServiceEntity service = PickServiceForLunch(); // pick first and hungry service
				if (service != null) {
					service.StartLunchBreak();
					message.Service = service;
					SendServiceToLunch(message); // abstract method, real message call is implemented in the child class
				}
				else { // No one is free, but should be ...
					   //if this method was called some service must be free, because this method is called only when some service end it process
					Debug.Fail("No service cannot go to lunch because no service is free, but someone should be free");
				}
			}
			else if (!MyAgent.Queue.IsEmpty()) { // if someone is waiting in queue for service
				message.Patient = MyAgent.Queue.Dequeue(); // get first patient in queue
				Console.WriteLine($"Service entity from lunch has just started service: {MySim.CurrentTime}");
				StartService(message); // we know that at least one service is free
			}
			else {
				// Service cannot go to lunch and nobody is waiting in the queue. So it is free time :)
			}
		}

		protected void StartOfLunchBreak(MyMessage myMessage) {
			Debug.Assert(Math.Abs(MySim.CurrentTime - MyAgent.GetStartTimeOfLunch()) < Double.Epsilon, "It is not lunch time!");
			Debug.Assert(GetNumberOfCurrentAvailableServices() == MyAgent.ServiceEntities.Count, "All services should be available at this time.");
			foreach (ServiceEntity service in MyAgent.ServiceEntities) {
				service.StartBeingHungry();
			}

			var freeServices = GetFreeServices(); // nobody has eaten yet, so everybody has HUNGRY state
			if (freeServices.Count > 0) {
				int serviceLimit = MyAgent.ServiceEntities.Count / 2; // at least half of services must be in the room all the time
				int numOfServiceToLunch = (freeServices.Count <= serviceLimit) ? freeServices.Count : serviceLimit; // how many services can go to lunch
				for (int i = 0; i < numOfServiceToLunch; i++) {
					MyMessage lunchMessage = (i == (numOfServiceToLunch - 1)) // last sent service
						? myMessage // don't need to copy last message
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
