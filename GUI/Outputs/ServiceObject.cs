using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Outputs {
	public class ServiceObject {

		public int Id { get; set; }

		public double Occupancy { get; set; }

		public string State { get; set; }

		public ServiceObject(int id, double occupancy, string state) {
			Id = id;
			Occupancy = occupancy;
			State = state;
		}
	}
}
