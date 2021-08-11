using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit.RealTime {
	public interface IMonitor {
		bool MonitorAvailable();

		double GetRPM();

		double GetEngineLoad();

		double GetCurrentGear();

	}

	public class EngineMonitor : IMonitor {
		public bool MonitorAvailable() {
			return true;
		}

		public double GetRPM() {
			return 2000;
		}

		public double GetEngineLoad() {
			return 10;
		}

		public double GetCurrentGear() {
			return 0;
		}
	}
}
