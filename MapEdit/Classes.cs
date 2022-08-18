using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;

namespace MapEdit {
	public enum EditMode {
		Grid,
		Property
	}

	public static class Utils {
		public static double Lerp(double A, double B, double X) {
			if (X <= 0)
				return A;

			if (X >= 1)
				return B;

			return A + ((B - A) * X);
		}

		public static float Lerp(float A, float B, float X) {
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

		public static string ConvertUnit(string UnitName) {
			if (UnitName == "°")
				return "deg";

			return UnitName;
		}

		public static bool TryParseDouble(string Str, out double F) {
			F = 0;

			if (Str == null)
				return false;

			if (double.TryParse(Str.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out F))
				return true;

			return false;
		}

		public static double ParseDouble(string Str) {
			return double.Parse(Str.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture);
		}

		public static double[] ParseStringArrayToDouble(string[] StrArr) {
			double[] Arr = new double[StrArr.Length];

			for (int i = 0; i < Arr.Length; i++) {
				Arr[i] = ParseDouble(StrArr[i]);
			}

			return Arr;
		}

		public static EditableData ParseTableFromText(string RawText) {
			string[] Lines = RawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
			string[][] Table = Lines.Select(L => L.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();

			List<string> RawTableData = new List<string>();

			for (int i = 1; i < Table.Length - 1; i++) {
				RawTableData.AddRange(Table[i].Skip(1));
			}

			double[] RawTableDataFloat = ParseStringArrayToDouble(RawTableData.ToArray());


			string YAxisUnit = ConvertUnit(Table[Table.Length - 1][0]);
			string XAxisUnit = ConvertUnit(Table[0][Table[0].Length - 1]);
			string Unit = ConvertUnit(Table[0][0]);

			string[] XAxisLabels = Table[0].Skip(1).Take(Table[0].Length - 2).ToArray();
			string[] YAxisLabels = new string[Table.Length - 2];

			for (int i = 1; i < Table.Length - 1; i++)
				YAxisLabels[i - 1] = Table[i][0];

			LookupTableAxis TableXAxis = new LookupTableAxis(XAxisUnit, ParseStringArrayToDouble(XAxisLabels));
			LookupTableAxis TableYAxis = new LookupTableAxis(YAxisUnit, ParseStringArrayToDouble(YAxisLabels));
			LookupTable2D LookupTable = new LookupTable2D(TableXAxis, TableYAxis, RawTableDataFloat);

			EditableData EditData = new EditableData(EditMode.Grid, LookupTable, Unit);
			EditData.DataProperties = new LookupTableSettings(LookupTable);

			return EditData;
		}

		public static EditableData ParseTableFromClipboard() {
			string RawText = Clipboard.GetText().Trim();
			return ParseTableFromText(RawText);
		}
	}
}
