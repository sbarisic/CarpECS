using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;
using static Raylib_cs.Raylib;

namespace TurboCalculator {
	class Program {
		const int W = 800;
		const int H = 800;
		static Vector2 Origin;

		static void Main(string[] args) {
			InitWindow(W, H, "Turbo Calculator");
			SetTargetFPS(60);

			Origin = new Vector2(50, 50);

			while (!WindowShouldClose())    // Detect window close button or ESC key
			{
				BeginDrawing();
				ClearBackground(Color.RAYWHITE);
				DrawGrid();
				EndDrawing();
			}

			CloseWindow();
		}

		static float ScaleMassFlowAxis(float Pt) {
			return Pt * 4.5f;
		}

		static float ScalePressureRatioAxis(float Pt) {
			return (Pt - 1) * 300;
		}

		static Vector2 TransGraphPt(Vector2 Pt) {
			return new Vector2(ScaleMassFlowAxis(Pt.X), ScalePressureRatioAxis(Pt.Y)) + Origin;
		}

		static void Line(Vector2 A, Vector2 B, Color Clr, float Thick) {
			DrawLineEx(new Vector2(A.X, H - A.Y), new Vector2(B.X, H - B.Y), Thick, Clr);
		}

		static void TransLines(int Thick, params Vector2[] Points) {
			for (int i = 1; i < Points.Length; i++) {
				Vector2 Prev = TransGraphPt(Points[i - 1]);
				Vector2 Cur = TransGraphPt(Points[i]);

				Line(Prev, Cur, Color.BLACK, Thick);
			}
		}

		static void AxisLabel(bool XAxis, float Val, string Text) {
			Vector2 Offset = Origin;
			int TxtFont = 20;

			if (XAxis) {
				float XPos = ScaleMassFlowAxis(Val);
				Line(new Vector2(XPos, 5) + Offset, new Vector2(XPos, -5) + Offset, Color.BLACK, 1);

				int TextSize = MeasureText(Text, TxtFont);
				DrawText(Text, (int)(Offset.X + XPos - TextSize / 2), H - (int)(Offset.Y - 10), TxtFont, Color.BLACK);
			} else {
				float YPos = ScalePressureRatioAxis(Val);
				Line(new Vector2(-5, YPos) + Offset, new Vector2(5, YPos) + Offset, Color.BLACK, 1);

				int TextSize = MeasureText(Text, TxtFont);
				DrawText(Text, (int)(Offset.X - TextSize - 15), H - (int)(Offset.Y + YPos + TxtFont / 2), TxtFont, Color.BLACK);
			}
		}

		static void DrawGrid() {
			int X = (int)Origin.X;
			int Y = (int)Origin.Y;

			int Size = 700;

			Line(Origin, Origin + new Vector2(1, 0) * Size, Color.BLACK, 1);
			Line(Origin, Origin + new Vector2(0, 1) * Size, Color.BLACK, 1);


			for (int i = 0; i < 11; i++) {
				float Val = i * 15;
				AxisLabel(true, Val, Val.ToString());
			}

			for (int i = 2; i < 10; i++) {
				float Val = i * 0.5f;
				AxisLabel(false, Val, Val.ToString());
			}


			// Surge lines
			TransLines(2, new Vector2(16, 1.12f), new Vector2(18, 1.23f), new Vector2(36, 1.6f), new Vector2(41, 1.85f), new Vector2(77, 2.46f), new Vector2(103, 3.2f));
			DrawEfficiencyLines();
			DrawSpeedLines();
		}

		static void DrawEfficiencyLines() {
			double[,] Arr = new double[,]{ { 0.550000011f, 0.474999994f, 0.404009997f, 0.419999986f, 0.446979999f, 0.386759996f, 0.426539987f, 0.431010007f },
				{ 0.519439995f, 0.627759993f, 0.554009974f, 0.651019990f, 0.523479998f, 0.471689999f, 0.486620008f, 0.479479998f },
				{ 0.455850005f, 0.671140015f, 0.690100014f, 0.676419973f, 0.599969983f, 0.550689995f, 0.545029997f, 0.527939975f },
				{ 0.489659994f, 0.654280006f, 0.723510026f, 0.708920001f, 0.684889972f, 0.629760026f, 0.603429973f, 0.576409995f },
				{ 0.517679989f, 0.578840017f, 0.711310029f, 0.728219985f, 0.719780027f, 0.689270019f, 0.661840021f, 0.624880015f },
				{ 0.539420008f, 0.573629975f, 0.659510016f, 0.713779985f, 0.724969983f, 0.716840028f, 0.698329985f, 0.667550027f },
				{ 0.564369976f, 0.573010027f, 0.598420023f, 0.670809984f, 0.699509978f, 0.705519974f, 0.698729991f, 0.688189983f },
				{ 0.581430017f, 0.567669987f, 0.586929976f, 0.607150018f, 0.625989973f, 0.638599991f, 0.642210006f, 0.644159972f },
				{ 0.523000001f, 0.510999977f, 0.527999997f, 0.546000003f, 0.563000023f, 0.574999988f, 0.578000009f, 0.579999983f }
				};


			double[] YArr = new double[] { 1.0, 1.3, 1.6, 2.0, 2.3, 2.6, 2.9, 3.3 };
			double[] XArr = new double[] { 0, 20, 40, 60, 80, 100, 120, 140, 160 };

			List<double> ZPoints = new List<double>();
			for (int i = 0; i < 25; i++) {
				ZPoints.Add((30 + i * 2) / 100.0);
			}

			double[] ZArr = ZPoints.ToArray();

			Conrec.Contour(Arr, XArr, YArr, ZArr, new RendererDelegate((X1, Y1, X2, Y2, Z) => {
				Vector2 A = new Vector2((float)X1, (float)Y1);
				Vector2 B = new Vector2((float)X2, (float)Y2);

				A = TransGraphPt(A);
				B = TransGraphPt(B);


				Line(A, B, Color.BLACK, 1);

			}));
		}

		static void DrawSpeedLines() {
			double[,] Arr = new double[,] { { 50000,    92527,  128442, 153878, 176256, 193700, 209686, 225238 },
				{ 50000,    93261,  128956, 154181, 176051, 193601, 209706, 225238 },
				{ 69477,    98678,  129814, 154485, 175845, 193589, 209733, 225238 },
				{ 94499,    108744, 134011, 156118, 175573, 193589, 209760, 225238 },
				{ 116717,   121867, 142909, 160408, 177514, 194153, 209787, 225238 },
				{ 139741,   153547, 154383, 168817, 182773, 196046, 210636, 226045 },
				{ 162799,   186210, 167864, 180211, 191834, 203833, 216283, 229394 },
				{ 194649,   220572, 209206, 200738, 209842, 221330, 232564, 243735 },
				{ 214114,   242629, 230127, 220812, 230826, 243463, 255820, 268109 }
				};


			double[] YArr = new double[] { 1.0, 1.3, 1.6, 2.0, 2.3, 2.6, 2.9, 3.3 };
			double[] XArr = new double[] { 0, 20, 40, 60, 80, 100, 120, 140, 160 };

			List<double> ZPoints = new List<double>();
			for (int i = 0; i < 40; i++) {
				ZPoints.Add(50000 + i * 25000);
			}

			Conrec.Contour(Arr, XArr, YArr, ZPoints.ToArray(), new RendererDelegate((X1, Y1, X2, Y2, Z) => {
				Vector2 A = new Vector2((float)X1, (float)Y1);
				Vector2 B = new Vector2((float)X2, (float)Y2);

				A = TransGraphPt(A);
				B = TransGraphPt(B);


				Line(A, B, Color.BLACK, 1);

			}));
		}
	}
}