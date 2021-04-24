using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccinationCenter.models;

namespace VaccinationCenter.common {
	interface Initializable {

		void Initialize(SimParameter parameter);
	}
}
