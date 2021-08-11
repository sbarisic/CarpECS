using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;

namespace MapEdit {
	public static class Utils {
		public static double Lerp(double A, double B, double X) {
			if (X <= 0)
				return A;

			if (X >= 1)
				return B;

			return A + ((B - A) * X);
		}

		public static double Lerp(double A, double B, double X1, double X2, double X) {
			double DX = (X - X1) / (X2 - X1);
			return Lerp(A, B, DX);
		}

		public static byte Lerp(byte A, byte B, float X) {
			if (X <= 0)
				return A;

			if (X >= 1)
				return B;

			return (byte)(A + ((double)(B - A) * X));
		}

		public static SolidColor Lerp(SolidColor A, SolidColor B, float X) {
			return new SolidColor(Lerp(A.R, B.R, X), Lerp(A.G, B.G, X), Lerp(A.B, B.B, X));
		}

		public static SolidColor Lerp(SolidColor A, SolidColor B, SolidColor C, float X) {
			if (X < 1)
				return Lerp(A, B, X);

			return Lerp(B, C, X - 1);
		}

		public static SolidColor Lerp(SolidColor A, SolidColor B, SolidColor C, float MinVal, float MaxVal, float X, float Center = float.PositiveInfinity) {
			float Middle = MinVal + ((MaxVal - MinVal) / 2);

			if (!float.IsInfinity(Center))
				Middle = Center;

			if (X < Middle)
				return Lerp(A, B, (X - MinVal) / (Middle - MinVal));

			return Lerp(B, C, (X - Middle) / (MaxVal - Middle));
		}

		public static double Clamp(double Val, double Min, double Max) {
			if (Val < Min)
				return Min;

			if (Val > Max)
				return Max;

			return Val;
		}

		public static double Bilinear(double A, double B, double C, double D, double DX, double DY) {
			return Lerp(Lerp(A, B, DX), Lerp(C, D, DX), DY);
		}

		public static double Bilinear(double A, double B, double C, double D, double X1, double X2, double Y1, double Y2, double X, double Y) {
			double DX = (X - X1) / (X2 - X1);
			double DY = (Y - Y1) / (Y2 - Y1);
			return Bilinear(A, B, C, D, DX, DY);
		}
	}

	public enum EditMode {
		Grid,
		Property
	}

	[DesignerCategory("Misc")]
	public class EditableData {
		public EditMode EditMode;

		public string XName;
		public string YName;
		public string ValueName;
		public object DefaultValue;
		public Worksheet Worksheet;

		[Browsable(false)]
		public virtual bool DataEnabled {
			get {
				return true;
			}
		}

		public EditableData(EditMode EditMode) {
			this.EditMode = EditMode;
			XName = "X Axis";
			YName = "Y Axis";
			ValueName = "Value";
			Worksheet = null;
		}

		protected void GenerateXAxis(Worksheet Sheet, int Count, Func<int, string> GenName) {
			Sheet.ColumnCount = Count;
			Sheet.SetColumnsWidth(0, Count, 45);

			for (int i = 0; i < Count; i++)
				Sheet.ColumnHeaders[i].Text = GenName(i);
		}

		protected void GenerateYAxis(Worksheet Sheet, int Count, Func<int, string> GenName) {
			Sheet.RowCount = Count;

			for (int i = 0; i < Count; i++)
				Sheet.RowHeaders[i].Text = GenName(i);
		}

		public virtual void PopulateSheet(Worksheet Sheet) {
		}

		public virtual void ColorCell(int X, int Y, object Value, ref Cell C) {
		}
	}

	[DesignerCategory("Engine"), DisplayName("Engine data")]
	public class EngineData : EditableData {
		public EngineData() : base(EditMode.Property) {
		}

		[Description("Number of cylinders"), Category("Basic")]
		public int Cylinders { get; set; } = 1;

		[Display(Order = 1)]
		[Description("Engine displacement [cm^3]"), Category("Basic")]
		public int Displacement { get; set; } = 200;

		// Rev limiter

		[Description("Enable limiter"), Category("Rev limiter")]
		public bool EnableRevLimit { get; set; } = true;

		[Description("RPM above which the rev limiter activates [RPM]"), Category("Rev limiter")]
		public int RevLimit { get; set; } = 11000;

		[Description("RPM below which the rev limiter deactivates [RPM]"), Category("Rev limiter")]
		public int RevLimitStop { get; set; } = 10950;

		// Lambda

		[Description("Enable lambda sensor. If disabled, open loop fuel injection map is used."), Category("Lambda")]
		public bool LambdaEnabled { get; set; } = false;

		[Description("Sensor min voltage"), Category("Lambda")]
		public float LambdaStart { get; set; } = 0.1f;

		[Description("Sensor max voltage"), Category("Lambda")]
		public float LambdaEnd { get; set; } = 0.9f;

		[Description("Sensor min A/F ratio"), Category("Lambda")]
		public float LambdaRatioBottom { get; set; } = 6.0f;

		[Description("Sensor max A/F ratio"), Category("Lambda")]
		public float LambdaRatioTop { get; set; } = 17.0f;

		// Fuel injector

		[Description("Minimum allowed AFR [A/F]"), Category("Fuel injector")]
		public float MinAFR { get; set; } = 11.0f;

		[Description("Maximum allowed AFR [A/F]"), Category("Fuel injector")]
		public float MaxAFR { get; set; } = 16.0f;

		/*[Description("Calculated injector pulse range [ms]"), Category("Fuel injector")]
		public float PulseRange {
			get {
				return MaxPulseWidth - MinPulseWidth;
			}
		}*/

		[Description("Shut off fuel when coasting on 0% throttle"), Category("Fuel injector")]
		public bool FuelShutOff { get; set; } = false;
	}

	[DesignerCategory("Maps"), DisplayName("Fuel map")]
	public class InjectionMap : EditableData {
		EngineData EngineData;

		public override bool DataEnabled => !EngineData.LambdaEnabled;

		public InjectionMap(EngineData EngineData) : base(EditMode.Grid) {
			this.EngineData = EngineData;

			XName = "Engine speed [RPM]";
			YName = "Engine load [%]";
			ValueName = "Target A/F";
			DefaultValue = 10.0;
		}

		public override void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet, 26, (i) => string.Format("{0}", i * 500));
			GenerateYAxis(Sheet, 21, (i) => string.Format("{0} %", i * 5));

			// Top left
			Sheet[0, 0] = 14.7;

			// Bottom left
			Sheet[Sheet.RowCount - 1, 0] = 14.0;

			// Top right
			Sheet[0, Sheet.ColumnCount - 1] = 13.0;

			// Bottom right
			Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = 11.0;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(new SolidColor(104, 162, 255), SolidColor.Green, new SolidColor(255, 0, 0), EngineData.MinAFR, EngineData.MaxAFR, (float)Num);
		}
	}

	[DesignerCategory("Maps"), DisplayName("Ignition advance map")]
	public class AdvanceMap : EditableData {
		EngineData EngineData;

		public AdvanceMap(EngineData EngineData) : base(EditMode.Grid) {
			this.EngineData = EngineData;

			XName = "Engine speed [RPM]";
			YName = "Engine load [%]";
			ValueName = "Spark advance [° before TDC]";
			DefaultValue = 0.0;
		}

		public override void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet, 26, (i) => string.Format("{0}", i * 500));
			GenerateYAxis(Sheet, 21, (i) => string.Format("{0} %", i * 5));

			Sheet[0, 0] = Sheet[Sheet.RowCount - 1, 0] = 2.0;
			Sheet[0, Sheet.ColumnCount - 1] = Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = -30.0;

			// Top left
			Sheet[0, 0] = 10.0;

			// Bottom left
			Sheet[Sheet.RowCount - 1, 0] = -10.0;

			// Top right
			Sheet[0, Sheet.ColumnCount - 1] = 10.0;

			// Bottom right
			Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = 10.0;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(SolidColor.Blue, SolidColor.Green, SolidColor.Red, -20, 20, (float)Num, Center: 0);
		}
	}

	[DesignerCategory("Maps"), DisplayName("Load limiter by gear map")]
	public class LoadLimiter : EditableData {
		public LoadLimiter(EngineData EngineData) : base(EditMode.Grid) {
			XName = "Engine speed [RPM]";
			YName = "Gearbox gear";
			ValueName = "Engine load limit [% of max load]";
			DefaultValue = 100.0;
		}

		public override void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet, 26, (i) => string.Format("{0}", i * 500));
			GenerateYAxis(Sheet, 6, (i) => string.Format("{0}", i + 1));

			// Top left
			Sheet[0, 0] = 100.0;

			// Bottom left
			Sheet[Sheet.RowCount - 1, 0] = 100.0;

			// Top right
			Sheet[0, Sheet.ColumnCount - 1] = 100.0;

			// Bottom right
			Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = 100.0;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(SolidColor.Red, SolidColor.Green, SolidColor.Blue, 0, 200, (float)Num, Center: 100);
		}
	}
}
