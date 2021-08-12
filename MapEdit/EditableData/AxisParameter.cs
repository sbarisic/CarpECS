using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapEdit.RealTime;

namespace MapEdit {
	public delegate double GetMonitorValueFunc();

	public class AxisParameter {
		double MinVal;
		double MaxVal;
		double Step;

		public string AxisName {
			get; private set;
		}

		public int Count {
			get; private set;
		}

		public string[] ColumnName {
			get; private set;
		}

		public GetMonitorValueFunc GetMonitorValue {
			get;
			private set;
		}

		public int GetMonitorColumnIndex() {
			double Val = GetMonitorValue();
			return (int)Math.Round(Val / Step);
		}

		public AxisParameter(string Name) {
			AxisName = Name;
		}

		public void Init(double MinVal, double MaxVal, double Step, GetMonitorValueFunc GetMonitorValue) {
			this.MinVal = MinVal;
			this.MaxVal = MaxVal;
			this.Step = Step;

			Count = (int)((MaxVal - MinVal) / Step + 1);
			ColumnName = new string[Count];

			for (int i = 0; i < Count; i++) {
				ColumnName[i] = (MinVal + Step * i).ToString();
			}

			this.GetMonitorValue = GetMonitorValue;
		}
	}

	public static class AxisParameters {
		public static AxisParameter RPM = new AxisParameter("RPM");
		public static AxisParameter EngineLoad = new AxisParameter("Engine Load [%]");
		public static AxisParameter GearboxGear = new AxisParameter("Gearbox Gear");
		public static AxisParameter MAP = new AxisParameter("Manifold Absolute Pressure [bar]");
		public static AxisParameter MAF = new AxisParameter("Mass Air Flow [g/sec]");

		public static void Init(EngineData EData, IMonitor Mon) {
			RPM.Init(0, EData.RevLimit, 250, Mon.GetRPM);
			EngineLoad.Init(0, 100, 5, Mon.GetEngineLoad);
			GearboxGear.Init(0, 7, 1, Mon.GetCurrentGear);
			MAP.Init(0, 3, 0.1, Mon.GetCurrentMAP);
			MAF.Init(0, 140, 5, Mon.GetCurrentMAF);
		}
	}
}
