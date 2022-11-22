using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static Raylib_cs.Raylib;
using System.Diagnostics;

namespace InjectorCalculator {
	static class Program {
		const int W = 1600;
		const int H = 1000;

		static Vector2 Origin;
		static Font TxtFnt;

		static RaylibGrid Grid;

		static bool DoPrintData;

		static void Main(string[] args) {
			SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
			SetConfigFlags(ConfigFlags.FLAG_WINDOW_HIGHDPI);
			InitWindow(W, H, "Injector Calculator");
			SetTargetFPS(30);

			Origin = new Vector2(50, 50);

			TxtFnt = LoadFont("consola.ttf");
			TxtFnt.baseSize = 36;
			SetTextureFilter(TxtFnt.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);

			Grid = new RaylibGrid(new Vector2(60, 50), new Vector2(W - 100, H - 100), W, H);
			Grid.SetAxis("PWidth [ms]", "Flow", new Vector2(5, 0.03f), new Vector2(0.25f, 0.001f));
			Grid.SetFont(TxtFnt);

			LoadData();

			float MS = CalcPW(0.076f, 14.7f, out float MSIdeal);
			Console.WriteLine("{0} ms; {1} ideal ms", MS, MSIdeal);

			CSVPoint[] Points = HPTCSV.ParseEntriesHPT("test1.csv");

			while (!WindowShouldClose())    // Detect window close button or ESC key
			{
				BeginDrawing();
				ClearBackground(Color.BLACK);
				Grid.Draw();
				Draw();


				/*for (int i = 0; i < Points.Length; i++) {
					CSVPoint Pt = Points[i];

					float Stoich = 14.1298828125f;
					float AFRFromEQ = (Points[i].EQ / 14.7f) * Stoich;
					float Trim = ((Points[i].LTFT + Points[i].STFT) / 100);

					AFRFromEQ = AFRFromEQ - (AFRFromEQ * Trim);



					Grid.PlotPixel(new Vector2(Points[i].InjMS, Points[i].CylAirmass / AFRFromEQ), Color.BLUE);
				}*/


				EndDrawing();

				Vector2 MousePos = GetMousePosition();
				MousePos = new Vector2(MousePos.X, H - MousePos.Y);


				//bool Left = IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT);
				//bool Right = IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT);

				if (Raylib.IsKeyPressed(KeyboardKey.KEY_F1))
					DoPrintData = true;
			}

			CloseWindow();
		}

		static void Draw() {
			int MassCount = 400;
			float MassStep = Grid.AxisSize.Y / MassCount;

			Grid.BeginLine(false);

			for (int i = 0; i < MassCount; i++) {
				float FuelMass = i * MassStep;

				float MS = CalcPWFord(FuelMass, out float MSIdeal);

				Grid.PlotPixel(new Vector2(MSIdeal, FuelMass), Color.GRAY);
				Grid.PlotLinePoint(new Vector2(MS, FuelMass));
			}

			Grid.EndLine(1, Color.BLUE);

			//---


			Grid.BeginLine(false);

			for (int i = 0; i < MassCount; i++) {
				float FuelMass = i * MassStep;

				float MS2 = CalcPW(FuelMass, out float MSIdeal2);

				Grid.PlotPixel(new Vector2(MSIdeal2, FuelMass), Color.RED);
				Grid.PlotLinePoint(new Vector2(MS2, FuelMass));
			}

			Grid.EndLine(1, Color.ORANGE);
		}

		static ECUMap Map_OffsetPressIGNV;
		static ECUMap Map_FlowRateMultVsIAT;
		static ECUMap Map_ShortPulseAdder;

		static ECUMap Map_FlowRateLowMultVsPressure;
		static ECUMap Map_FlowRateHighMultVsPressure;
		static ECUMap Map_BreakpointMult;
		static ECUMap Map_Offset;
		static ECUMap Map_OffsetMult;

		static float CalcPW(float AirMass, float AFR, out float PWIdeal) {
			return CalcPW(AirMass / AFR, out PWIdeal);
		}

		const float IAT = 22.0f;
		const float BatteryVoltage = 14;

		static float CalcPWFord(float FuelMass, out float PWIdeal) {
			//float PressureDropRef = 270.0f; // kPa
			float RailFuelPressure = 300.0f; // kPa

			float FlowRateLow = 7.218009496f; // g/s
			float FlowRateHigh = 6.122131224f; // g/s
			float Breakpoint = 0.006549868f; // g

			float BreakpointMult = Map_BreakpointMult.Index(0, RailFuelPressure);
			float LowMult = Map_FlowRateLowMultVsPressure.Index(0, RailFuelPressure);
			float HighMult = Map_FlowRateHighMultVsPressure.Index(0, RailFuelPressure);


			float Offset = Map_Offset.Index(0, BatteryVoltage);
			float OffsetMult = Map_OffsetMult.Index(0, RailFuelPressure);
			float IGNV = Offset * OffsetMult;

			float CalcBreakpoint = Breakpoint * BreakpointMult;

			// Fuel mass breakpoint for low and high flow curves
			float HiOffset1 = (CalcBreakpoint / (FlowRateHigh * HighMult)) * 1000;
			float LoOffset1 = (CalcBreakpoint / (FlowRateLow * LowMult)) * 1000;
			float Diff = HiOffset1 - LoOffset1;
			
			float LowCurveOffset = 0;
			float CurrentFlowRate = 0;

			if (FuelMass > CalcBreakpoint)
				CurrentFlowRate = FlowRateHigh * HighMult;
			else {
				CurrentFlowRate = FlowRateLow * LowMult;

				// Offset the low curve by breakpoint difference (moves low fuel mass curve to the right)
				LowCurveOffset = Diff;
			}


			// Ideal pulse width
			PWIdeal = FuelMass / (FlowRateHigh * HighMult) * 1000;

			// Current pulse width = ideal + injector lag + ideal curve from low and high compensation
			float PWCur = FuelMass / CurrentFlowRate * 1000 + IGNV + LowCurveOffset;


			/*if (PWIdeal >= 0 && PWIdeal <= 4f) {
				Console.WriteLine("{0} - {1}", ShortPulseAdder, PWIdeal);
			} else if (PWIdeal > 4)
				Console.WriteLine("Done!");*/

			return PWCur;
		}

		static float CalcPW(float FuelMass, out float PWIdeal) {
			float ShortPulseLimit = 3;
			float StaticFlowRate = 6.4580078125f; // 6.2998046875f; // g/s


			float OffsetPressIGNV = Map_OffsetPressIGNV.Index(0, BatteryVoltage);
			//float OffsetPressIGNV = 0;


			float FlowRateMultVsIAT = Map_FlowRateMultVsIAT.Index(IAT, 0);

			float FlowRate = StaticFlowRate * FlowRateMultVsIAT;

			float MS = FuelMass / FlowRate * 1000;
			PWIdeal = FuelMass / StaticFlowRate * 1000;

			float ShortPulseAdder = 0;

			if (MS < ShortPulseLimit)
				ShortPulseAdder = Map_ShortPulseAdder.Index(MS, 0);

			return MS + ShortPulseAdder + OffsetPressIGNV;

		}
		static void LoadData() {
			Map_OffsetPressIGNV = new ECUMap(@"
ms	128	148	168	188	208	228	248	268	288	308	328	348	368	388	408	428	448	468	488	508	528	548	568	588	608	628	648	668	688	708	728	748	768	kPa
4	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875	13.96875
5	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375	12.7109375
6	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625	5.625
7	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375	3.5859375
8	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375	2.359375
9	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625	1.9140625
10	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875	1.5546875
11	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125	1.3125
12	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125	1.125
13	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375	0.984375
14	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625	0.8515625
15	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125	0.7578125
16	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125	0.6328125
17	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625	0.515625
18	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875	0.421875
19	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125	0.328125
20	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625	0.2265625
V
");

			Map_FlowRateMultVsIAT = new ECUMap(@"
	-40	-30	-20	-10	0	10	20	30	40	50	60	70	80	90	100	110	120	°C
Multiplier	1	1	1	1	1	1	1	1	1	1	1	1	1	1	1	1	1
");

			Map_ShortPulseAdder = new ECUMap(@"
ms	0	0.125	0.25	0.375	0.5	0.625	0.75	0.875	1	1.125	1.25	1.375	1.5	1.625	1.75	1.875	2	2.125	2.25	2.375	2.5	2.625	2.75	2.875	3	3.125	3.25	3.375	3.5	3.625	3.75	3.875	4	
PulseWidthAdder	0	0.140625	0.109375	0.09375	0.078125	0.0625	0.03125	0.015625	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0
");



			//-- Ford

			Map_FlowRateLowMultVsPressure = new ECUMap(@"
x	0	x
137.89514	0.714999974
206.84271	0.875999987
269.9986831	1
345.0136466	1.130399942
414.0301526	1.238299966
482.9777489	1.337000012
kPa
");

			Map_FlowRateHighMultVsPressure = new ECUMap(@"
x	0	x
137.89514	0.714999974
206.84271	0.875999987
269.9986831	1
345.0136466	1.137199998
414.0301526	1.250399947
482.9777489	1.337000012
kPa
");

			Map_BreakpointMult = new ECUMap(@"	
x	0	x
137.89514	0.714999974
206.84271	0.875999987
269.9986831	1
345.0136466	1.020799994
414.0301526	1.215399981
482.9777489	1.337000012
kPa
");

			Map_Offset = new ECUMap(@"
x	0	x
6	5.202000029
7	3.315789159
8	2.184000099
9	1.770676696
10	1.434999984
11	1.210000017
12	1.04100001
13	0.90699998
13.5	0.834586448
14	0.789000012
15	0.699000026
15.89999962	0.586466165
V
");

			Map_OffsetMult = new ECUMap(@"
x	0	x
137.89514	0.88499999
206.84271	0.944999993
269.9997877	1
344.73785	1.203999996
413.68542	1.163800001
482.63299	1.18599999
kPa
");
		}
	}
}