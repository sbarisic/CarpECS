using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;

namespace InjectorCalculator {
	class Utils {
		public static float Lerp(float A, float B, float Amt) {
			return A * (1 - Amt) + B * Amt;
		}


		//   A ---- B
		//   |      |
		//   |      |
		//   D ---- C

		public static float Bilinear(float A, float B, float C, float D, float XAmt, float YAmt) {
			return Lerp(Lerp(A, B, XAmt), Lerp(D, C, XAmt), YAmt);
		}

		public static Vector2 Lerp(Vector2 A, Vector2 B, float Amt) {
			return new Vector2(Lerp(A.X, B.X, Amt), Lerp(A.Y, B.Y, Amt));
		}

		public static Vector3 Lerp(Vector3 A, Vector3 B, float Amt) {
			return new Vector3(Lerp(A.X, B.X, Amt), Lerp(A.Y, B.Y, Amt), Lerp(A.Z, B.Z, Amt));
		}

		public static float Normalize(float Num, float Min, float Max) {
			return (Num - Min) / (Max - Min);
		}

		public static double ParseDouble(string Txt) {
			Txt = Txt.Replace(",", ".");
			return double.Parse(Txt, System.Globalization.CultureInfo.InvariantCulture);
		}

		public static bool TryParseDouble(string Txt, out double D) {
			Txt = Txt.Replace(",", ".");
			return double.TryParse(Txt, NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out D);
		}

		public static float ParseFloat(string Txt) {
			return (float)ParseDouble(Txt);
		}

		public static bool TryParseFloat(string Txt, out float F) {
			bool Ret = TryParseDouble(Txt, out double D);

			F = (float)D;
			return Ret;
		}
	}
}
