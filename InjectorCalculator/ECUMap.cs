using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectorCalculator {
	class ECUMap {
		float[] XAxis;
		float[] YAxis;
		float[] Values;


		public ECUMap(string Src) {
			Parse(Src);
		}

		float Get(int X, int Y) {
			return Values[Y * XAxis.Length + X];
		}

		public float Index(float X, float Y) {
			int W = XAxis.Length;
			int H = YAxis.Length;

			int XIdx1 = 0;
			int XIdx2 = 0;
			float XLerp = 0;

			int YIdx1 = 0;
			int YIdx2 = 0;
			float YLerp = 0;

			FindIndex(XAxis, X, out XIdx1, out XIdx2, out XLerp);
			FindIndex(YAxis, Y, out YIdx1, out YIdx2, out YLerp);

			//   A ---- B
			//   |      |
			//   |      |
			//   D ---- C

			float A = Get(XIdx1, YIdx1);
			float B = Get(XIdx2, YIdx1);
			float C = Get(XIdx2, YIdx2);
			float D = Get(XIdx1, YIdx2);

			float Value = Utils.Bilinear(A, B, C, D, XLerp, YLerp);

			return Value;
		}

		void FindIndex(float[] Axis, float Value, out int Idx1, out int Idx2, out float LerpVal) {
			Idx1 = 0;
			Idx2 = 0;
			LerpVal = 0;

			if (Value < Axis[0]) {
				Idx1 = 0;
				Idx2 = 0;
				LerpVal = 0;
			}

			if (Value > Axis[Axis.Length - 1]) {
				Idx1 = Axis.Length - 1;
				Idx2 = Axis.Length - 1;
				LerpVal = 1;
			}

			for (int i = 1; i < Axis.Length; i++) {
				float Prev = Axis[i - 1];
				float Cur = Axis[i];

				if (Value >= Prev && Value <= Cur) {
					Idx1 = i - 1;
					Idx2 = i;

					LerpVal = (Value - Prev) / (Cur - Prev);
					return;
				}
			}
		}

		public void Parse(string Src) {
			Src = Src.Trim().Replace('\t', ' ').Replace("\r", "");
			string[] Lines = Src.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

			if (Lines[Lines.Length - 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
				Lines = Lines.SkipLast(1).ToArray();


			string[] XAxisStr = Lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).SkipLast(1).ToArray();

			if (!Utils.TryParseFloat(XAxisStr[0], out float Flt)) 
				XAxisStr = XAxisStr.Skip(1).ToArray();

			XAxis = XAxisStr.Select(Utils.ParseFloat).ToArray();

			List<float> ValuesList = new List<float>();

			YAxis = new float[Lines.Length - 1];
			for (int i = 1; i < Lines.Length; i++) {
				string[] RowSplit = Lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (Utils.TryParseFloat(RowSplit[0], out float F)) 
					YAxis[i - 1] = F;
				 else
					YAxis[i - 1] = i - 1;

				ValuesList.AddRange(RowSplit.Skip(1).Select(Utils.ParseFloat));
			}

			Values = ValuesList.ToArray();
		}
	}
}
