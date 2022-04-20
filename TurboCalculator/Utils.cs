using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Raylib_cs;

namespace TurboCalculator {
    static class Utils {
        public static float Lerp(float A, float B, float Amt) {
            return A * (1 - Amt) + B * Amt;
        }

        public static Vector3 Lerp(Vector3 A, Vector3 B, float Amt) {
            return new Vector3(Lerp(A.X, B.X, Amt), Lerp(A.Y, B.Y, Amt), Lerp(A.Z, B.Z, Amt));
        }

        public static Color Lerp(Color A, Color B, float Amt) {
            if (Amt <= 0)
                return A;

            if (Amt >= 1)
                return B;

            Vector3 New = Lerp(new Vector3(A.r, A.g, A.b), new Vector3(B.r, B.g, B.b), Amt);
            return new Color((byte)New.X, (byte)New.Y, (byte)New.Z, (byte)255);
        }

        public static float Normalize(float Num, float Min, float Max) {
            return (Num - Min) / (Max - Min);
        }
    }
}
