using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPStat;

namespace GUI {
	public static class Utils {

		public static string ParseMean(Stat stat) {
			return stat.Mean().ToString(CultureInfo.InvariantCulture);
		}

		public static string ParseConfidenceInterval(Stat stat) {
			double[] ci = stat.ConfidenceInterval95;
			return $"({ci[0]}, {ci[1]})";
		}
	}
}
