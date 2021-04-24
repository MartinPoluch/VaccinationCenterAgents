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


		public ServiceManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent) {
			Init();
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
			ServiceEntity service = GetFreeService();
			service.ServiceStatus = ServiceStatus.Occupied;
			message.Service = service;
			message.Addressee = MyAgent.GetServiceProcess();
			StartContinualAssistant(message);
		}

		public void GoToServiceOrQueue(MyMessage message) {
			Patient patient = message.Patient;
			patient.StartOfWaiting[MyAgent.GetServiceType()] = MySim.CurrentTime;
			if (IsAnyServiceFree()) {
				StartService(message);
			}
			else {
				MyAgent.Queue.Enqueue(patient);
			}
		}

		public void ProcessEndOfMove(MessageForm message) {
		}


		public void ProcessFinishMoveProcess(MessageForm message) {
		}

		public void ProcessFinishServiceProcess(MessageForm message) {
		}

		public void ProcessFinishBreakScheduler(MessageForm message) {
		}

		public void ProcessEndOfService(MessageForm msg) {
			MyMessage message = (MyMessage)msg;
			Console.WriteLine("End of service: " +MySim.CurrentTime);
		}

		public void ProcessStartBreak(MessageForm message) {
		}

		public void ProcessDefault(MessageForm message) {
			switch (message.Code) {
			}
		}

		public void Init() {
		}

		public override void ProcessMessage(MessageForm message) {
			switch (message.Code) {
				//case Mc.EndOfMove:
				//	ProcessEndOfMove(message);
				//	break;
				//case SimId.MoveProcess:
				//	ProcessFinishMoveProcess(message);
				//	break;
				//case SimId.ServiceProcess:
				//	ProcessFinishServiceProcess(message);
				//	break;
				//case SimId.BreakScheduler:
				//	ProcessFinishBreakScheduler(message);
				//	break;
				//case Mc.StartBreak:
				//	ProcessStartBreak(message);
				//	break;
				//case Mc.EndOfService:
				//	ProcessEndOfService(message);
				//	break;

				//default:
				//	ProcessDefault(message);
				//	break;
			}
		}

		public new ServiceAgent MyAgent {
			get {
				return (ServiceAgent)base.MyAgent;
			}
		}
	}
}
