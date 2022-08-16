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

		public float[] Data;

		public LookupTableAxis(string AxisName, float[] Data) {
			this.AxisName = AxisName;
			this.Data = Data.ToArray();
		}

		public void GetAxisIndex(float Value, out int Index1, out int Index2, out float Interp) {
			Interp = 0;

			if (Value < Data[0]) {
				Index1 = 0;
				Index2 = 0;
			} else if (Value > Data[AxisLength - 1]) {
				Index1 = AxisLength - 1;
				Index2 = AxisLength - 1;
			} else {
				float Prev = 0;
				float Cur = 0;

				for (int i = 1; i < AxisLength; i++) {
					Prev = Data[i - 1];
					Cur = Data[i];

					if (Value >= Prev && Value <= Cur) {
						float MinVal = Value - Prev;
						float Range = Cur - Prev;

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
		public float[] Data;
	}

	public class LookupTable2D : LookupTable {
		public LookupTableAxis Axis_X;
		public LookupTableAxis Axis_Y;

		public LookupTable2D(LookupTableAxis AxisX, LookupTableAxis AxisY, float[] Data) {
			Axis_X = AxisX;
			Axis_Y = AxisY;
			this.Data = Data;
		}

		public float GetDataRaw(int X, int Y) {
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

		public void SetData(int X, int Y, float Val) {
			Data[X + Axis_X.AxisLength * Y] = Val;
		}

		public float IndexData(float Axis_X_Val, float Axis_Y_Val) {
			Axis_X.GetAxisIndex(Axis_X_Val, out int Idx_X_1, out int Idx_X_2, out float Interp_X);
			Axis_Y.GetAxisIndex(Axis_Y_Val, out int Idx_Y_1, out int Idx_Y_2, out float Interp_Y);

			float TopLeft = GetDataRaw(Idx_X_1, Idx_Y_1);
			float TopRight = GetDataRaw(Idx_X_2, Idx_Y_1);
			float BottomLeft = GetDataRaw(Idx_X_1, Idx_Y_2);
			float BottomRight = GetDataRaw(Idx_X_2, Idx_Y_2);

			return (float)Utils.Bilinear(TopLeft, TopRight, BottomLeft, BottomRight, Interp_X, Interp_Y);
		}
	}
}
