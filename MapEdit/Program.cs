using MapEdit.RealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEdit {
	class Program {
		static bool Initialized;

		static MapEdit EditForm;
		static EditableData[] Datas;

		public static IMonitor ECUMonitor;

		static void Main(string[] args) {
			Initialized = false;

			Thread FormThread = new Thread(RunForms);
			FormThread.SetApartmentState(ApartmentState.STA);
			FormThread.IsBackground = false;
			FormThread.Start();

			while (!Initialized)
				Thread.Sleep(10);

			ECUMonitor = new EngineMonitor();

			EngineData EData = new EngineData();
			AxisParameters.Init(EData, ECUMonitor);

			Datas = new EditableData[] {
				EData,
				new VolumetricEfficiencyMap(EData),
				new MassAirFlowMap(EData),
				new FuelMap(EData),
				new IgnitionAdvanceMap(EData),
				new LoadLimiter(EData),
			};

			SetEditable(Datas);
		}

		static void SetEditable(EditableData[] Data) {
			EditForm.Invoke(new Action<EditableData[]>((DataArr) => {
				EditForm.SetEditable(DataArr);
			}), new object[] { Data });
		}

		static void RunForms() {
			Application.EnableVisualStyles();

			EditForm = new MapEdit();
			Initialized = true;

			Application.Run(EditForm);
			Environment.Exit(0);
		}
	}
}
