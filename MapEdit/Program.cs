using MapEdit.RealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

			//EngineData EData = new EngineData();
			//AxisParameters.Init(EData, ECUMonitor);

			string[] TableFiles = Directory.GetFiles("data/tables", "*.txt");
			List<EditableData> DataList = new List<EditableData>();

			for (int i = 0; i < TableFiles.Length; i++) {
				string TableName = Path.GetFileNameWithoutExtension(TableFiles[i]);
				string RawText = File.ReadAllText(TableFiles[i].Trim());

				EditableData EData = Utils.ParseTableFromText(RawText, out LookupTable2D LookupTable);
				EData.Name = TableName;

				EData.DataProperties = new LookupTableSettings(LookupTable);

				DataList.Add(EData);
			}

			BehaviorEditor Behaviors = new BehaviorEditor();
			DataList.Add(Behaviors);

			Datas = DataList.ToArray();

			Thread.Sleep(100);
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
