﻿using MapEdit.RealTime;
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

			//EngineData EData = new EngineData();
			//AxisParameters.Init(EData, ECUMonitor);


			LookupTableAxis TestX = new LookupTableAxis("RPM", new float[] { 1000, 2000, 3000, 4000 });
			LookupTableAxis TestY = new LookupTableAxis("MAF", new float[] { 10, 14, 20 });
			LookupTable2D Test = new LookupTable2D(TestX, TestY, new float[] { 0, 1, 2, 3, 16, 17, 18, 7, 8, 9, 10, 11 });

			EditableData TestData = new EditableData(EditMode.Grid, Test, "UNIT");
			TestData.DataProperties = new LookupTableSettings(Test);


			Datas = new EditableData[] {
				//EData,
				//new LoadLimiter(EData),

				TestData
			};

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
