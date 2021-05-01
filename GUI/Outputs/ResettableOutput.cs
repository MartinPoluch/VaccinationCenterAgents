using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Outputs {
	public interface ResettableOutput : OutputStat {

		void ResetOutput();
	}
}
