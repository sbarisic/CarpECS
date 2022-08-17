using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit.RealTime {
	public interface IMonitor {
		bool MonitorAvailable();

		double GetRPM();

		double GetEngineLoad();

		double GetCurrentGear();

		double GetCurrentMAP();

		double GetCurrentMAF();

		double GetSampleData(int Min, int Max, double Freq);
	}

	public class EngineMonitor : IMonitor {
		Stopwatch SWatch;

		public EngineMonitor() {
			SWatch = Stopwatch.StartNew();
		}

		public bool MonitorAvailable() {
			return true;
		}

		public double GetRPM() {
			return ((Math.Sin(SWatch.Elapsed.TotalSeconds / 2) + 1) / 2) * 2500 + 1500;

			// return 3150;
		}

		public double GetEngineLoad() {
			return ((Math.Sin(SWatch.Elapsed.TotalSeconds / 3) + 1) / 2) * 60 + 5;
		}

		public double GetCurrentGear() {
			return 0;
		}

		public double GetCurrentMAP() {
			return ((Math.Sin(SWatch.Elapsed.TotalSeconds / 1.5) + 1) / 2) * 2 + 0.3;
		}

		public double GetCurrentMAF() {
			return ((Math.Sin(SWatch.Elapsed.TotalSeconds / 1.5) + 1) / 2) * 140;
		}

		public double GetSampleData(int Min, int Max, double Freq) {
			double Time = (Math.Sin(SWatch.Elapsed.TotalSeconds * Freq) + 1) / 2;

			//return Time * (Max - Min);
			return Time * (Max - Min) + Min;
		}
	}
}