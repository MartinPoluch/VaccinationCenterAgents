using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using OSPDataStruct;
using OSPRNG;
using OSPStat;
using simulation;
using VaccinationCenter.entities;
using VaccinationCenter.models;


namespace VaccinationCenter.common {

	public abstract class ServiceAgent : Agent {

		protected ServiceAgent(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent) {
			Init();
			QueueLengthStat = new WStat(MySim);
			Queue = new SimQueue<Patient>(QueueLengthStat);
		}

		public SimQueue<Patient> Queue { get; set; }

		public WStat QueueLengthStat { get; set; }

		public List<ServiceEntity> ServiceEntities { get; set; }

		public Dictionary<int, UniformDiscreteRNG> ServiceDecisions { get; set; }

		/**
		 * Template method design patter.
		 */
		public virtual void Initialization(SimParameter simParameter) {
			ServiceEntities = new List<ServiceEntity>();
			int numOfServices = simParameter.GetNumberOfServices(GetServiceType());
			for (int i = 0; i < numOfServices; i++) {
				ServiceEntities.Add(CreateEntity());
			}

			ServiceDecisions = new Dictionary<int, UniformDiscreteRNG>();
			for (int i = 2; i <= numOfServices; i++) {
				ServiceDecisions[i] = new UniformDiscreteRNG(0, i - 1);
			}
		}

		protected abstract ServiceEntity CreateEntity();

		public abstract ServiceType GetServiceType();

		public abstract ContinualAssistant GetServiceProcess();

		public override void PrepareReplication() {
			base.PrepareReplication();
			QueueLengthStat.Clear();
			Queue.Clear();
		}

		private void Init() {
			
		}
	}
}
