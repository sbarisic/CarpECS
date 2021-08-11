using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapEdit.RealTime;

namespace MapEdit {
	public delegate double GetMonitorValueFunc();

	public class AxisParameter {
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

		public AxisParameter(string Name) {
			AxisName = Name;
		}

		public void Init(int MinVal, int MaxVal, int Step, GetMonitorValueFunc GetMonitorValue) {
			Count = (MaxVal - MinVal) / Step + 1;
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

		public static void Init(EngineData EData, IMonitor Mon) {
			RPM.Init(0, EData.RevLimit, 250, Mon.GetRPM);
			EngineLoad.Init(0, 100, 5, Mon.GetEngineLoad);
			GearboxGear.Init(0, 7, 1, Mon.GetCurrentGear);
		}
	}
}
