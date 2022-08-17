using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit {
	public interface ILookupTable {

	}

	public class LookupTableAxis {
		public string AxisName;

		public int AxisLength {
			get {
				return Data.Length;
			}
		}

		public double[] Data;

		public LookupTableAxis(string AxisName, double[] Data) {
			this.AxisName = AxisName;
			this.Data = Data.ToArray();
		}

		public void GetAxisIndex(double Value, out int Index1, out int Index2, out double Interp) {
			Interp = 0;

			if (Value < Data[0]) {
				Index1 = 0;
				Index2 = 0;
				return;
			} else if (Value > Data[AxisLength - 1]) {
				Index1 = AxisLength - 1;
				Index2 = AxisLength - 1;
				return;
			} else {
				double Prev = 0;
				double Cur = 0;

				for (int i = 1; i < AxisLength; i++) {
					Prev = Data[i - 1];
					Cur = Data[i];

					if (Value >= Prev && Value <= Cur) {
						double MinVal = Value - Prev;
						double Range = Cur - Prev;

						Interp = MinVal / Range;
						Index1 = i - 1;
						Index2 = i;
						return;
					}
				}
			}

			throw new Exception("Invalid data");
		}
	}

	public class LookupTable : ILookupTable {
		public string Name;
		public double[] Data;
	}

	public class LookupTable2D : LookupTable {
		public LookupTableAxis Axis_X;
		public LookupTableAxis Axis_Y;

		public LookupTable2D(LookupTableAxis AxisX, LookupTableAxis AxisY, double[] Data) {
			Axis_X = AxisX;
			Axis_Y = AxisY;
			this.Data = Data;
		}

		public double GetDataRaw(int X, int Y) {
			if (X < 0)
				X = 0;

			if (X >= Axis_X.AxisLength)
				X = Axis_X.AxisLength;

			if (Y < 0)
				Y = 0;

			if (Y >= Axis_Y.AxisLength)
				Y = Axis_Y.AxisLength;

			int Idx = X + Axis_X.AxisLength * Y;
			return Data[Idx];
		}

		public void SetData(int X, int Y, double Val) {
			Data[X + Axis_X.AxisLength * Y] = Val;
		}

		public double IndexData(double Axis_X_Val, double Axis_Y_Val, out int ClosestX, out int ClosestY, out float OffsetX, out float OffsetY) {
			Axis_X.GetAxisIndex(Axis_X_Val, out int Idx_X_1, out int Idx_X_2, out double Interp_X);
			Axis_Y.GetAxisIndex(Axis_Y_Val, out int Idx_Y_1, out int Idx_Y_2, out double Interp_Y);

			/*ClosestX = Interp_X < 0.5 ? Idx_X_1 : Idx_X_2;
			ClosestY = Interp_Y < 0.5 ? Idx_Y_1 : Idx_Y_2;

			OffsetX = (float)((Interp_X * 2.0) - 1.0);
			OffsetY = (float)((Interp_Y * 2.0) - 1.0);*/

			ClosestX = Idx_X_1;
			ClosestY = Idx_Y_1;

			OffsetX = (float)Interp_X;
			OffsetY = (float)Interp_Y;

			double TopLeft = GetDataRaw(Idx_X_1, Idx_Y_1);
			double TopRight = GetDataRaw(Idx_X_2, Idx_Y_1);
			double BottomLeft = GetDataRaw(Idx_X_1, Idx_Y_2);
			double BottomRight = GetDataRaw(Idx_X_2, Idx_Y_2);

			return (float)Utils.Bilinear(TopLeft, TopRight, BottomLeft, BottomRight, Interp_X, Interp_Y);
		}
	}
}
