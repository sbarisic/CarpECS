﻿using System;
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
			Grid.SetAxis("PWidth [ms]", "Flow", new Vector2(20, 0.1f), new Vector2(0.5f, 0.01f));
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


				for (int i = 0; i < Points.Length; i++) {
					CSVPoint Pt = Points[i];

					if (Pt.AccelPedal > 95) {
						Grid.PlotPixel(new Vector2(Points[i].InjMS, Points[i].CylAirmass / Points[i].AFR), Color.BLUE);
					}
				}


				EndDrawing();

				Vector2 MousePos = GetMousePosition();
				MousePos = new Vector2(MousePos.X, H - MousePos.Y);


				//bool Left = IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT);
				//bool Right = IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT);
			}

			CloseWindow();
		}

		static void Draw() {
			int MassCount = 400;
			float MassStep = Grid.AxisSize.Y / MassCount;

			Grid.BeginLine(false);

			for (int i = 0; i < MassCount; i++) {
				float FuelMass = i * MassStep;
				float MS = CalcPW(FuelMass, out float MSIdeal);

				Grid.PlotPixel(new Vector2(MSIdeal, FuelMass), Color.GREEN);
				Grid.PlotLinePoint(new Vector2(MS, FuelMass));
			}

			Grid.EndLine(1, Color.RED);
		}

		static ECUMap Map_OffsetPressIGNV;
		static ECUMap Map_FlowRateMultVsIAT;
		static ECUMap Map_ShortPulseAdder;

		static float CalcPW(float AirMass, float AFR, out float PWIdeal) {
			return CalcPW(AirMass / AFR, out PWIdeal);
		}

		static float CalcPW(float FuelMass, out float PWIdeal) {
			float BatteryVoltage = 14;
			float IAT = 40.0f;
			float ShortPulseLimit = 128.0f;
			float StaticFlowRate = 3.96826171875f; // g/s

			float OffsetPressIGNV = Map_OffsetPressIGNV.Index(0, BatteryVoltage);
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
4	13.96875	14.28125	14.59375	14.90625	15.21875	15.53125	15.84375	16.15625	16.46875	16.78125	17.09375	17.40625	17.71875	18.03125	18.34375	18.65625	18.96875	19.28125	19.59375	19.90625	20.21875	20.53125	20.84375	21.15625	21.46875	21.78125	22.09375	22.40625	22.71875	23.03125	23.34375	23.65625	23.96875
5	12.7109375	13.0234375	13.3359375	13.6484375	13.9609375	14.2734375	14.5859375	14.8984375	15.2109375	15.5234375	15.8359375	16.1484375	16.4609375	16.7734375	17.0859375	17.3984375	17.7109375	18.0234375	18.3359375	18.6484375	18.9609375	19.2734375	19.5859375	19.8984375	20.2109375	20.5234375	20.8359375	21.1484375	21.4609375	21.7734375	22.0859375	22.3984375	22.7109375
6	7.203125	7.515625	7.828125	8.140625	8.453125	8.765625	9.078125	9.390625	9.703125	10.015625	10.328125	10.640625	10.953125	11.265625	11.578125	11.890625	12.203125	12.515625	12.828125	13.140625	13.453125	13.765625	14.078125	14.390625	14.703125	15.015625	15.328125	15.640625	15.953125	16.265625	16.578125	16.890625	17.203125
7	2.8984375	3.2109375	3.5234375	3.8359375	4.1484375	4.4609375	4.7734375	5.0859375	5.3984375	5.7109375	6.0234375	6.3359375	6.6484375	6.9609375	7.2734375	7.5859375	7.8984375	8.2109375	8.5234375	8.8359375	9.1484375	9.4609375	9.7734375	10.0859375	10.3984375	10.7109375	11.0234375	11.3359375	11.6484375	11.9609375	12.2734375	12.5859375	12.8984375
8	1.984375	2.296875	2.609375	2.921875	3.234375	3.546875	3.859375	4.171875	4.484375	4.796875	5.109375	5.421875	5.734375	6.046875	6.359375	6.671875	6.984375	7.296875	7.609375	7.921875	8.234375	8.546875	8.859375	9.171875	9.484375	9.796875	10.109375	10.421875	10.734375	11.046875	11.359375	11.671875	11.984375
9	1.53125	1.84375	2.15625	2.46875	2.78125	3.09375	3.40625	3.71875	4.03125	4.34375	4.65625	4.96875	5.28125	5.59375	5.90625	6.21875	6.53125	6.84375	7.15625	7.46875	7.78125	8.09375	8.40625	8.71875	9.03125	9.34375	9.65625	9.96875	10.28125	10.59375	10.90625	11.21875	11.53125
10	1.2421875	1.5546875	1.8671875	2.1796875	2.4921875	2.8046875	3.1171875	3.4296875	3.7421875	4.0546875	4.3671875	4.6796875	4.9921875	5.3046875	5.6171875	5.9296875	6.2421875	6.5546875	6.8671875	7.1796875	7.4921875	7.8046875	8.1171875	8.4296875	8.7421875	9.0546875	9.3671875	9.6796875	9.9921875	10.3046875	10.6171875	10.9296875	11.2421875
11	1.03125	1.34375	1.65625	1.96875	2.28125	2.59375	2.90625	3.21875	3.53125	3.84375	4.15625	4.46875	4.78125	5.09375	5.40625	5.71875	6.03125	6.34375	6.65625	6.96875	7.28125	7.59375	7.90625	8.21875	8.53125	8.84375	9.15625	9.46875	9.78125	10.09375	10.40625	10.71875	11.03125
12	0.859375	1.171875	1.484375	1.796875	2.109375	2.421875	2.734375	3.046875	3.359375	3.671875	3.984375	4.296875	4.609375	4.921875	5.234375	5.546875	5.859375	6.171875	6.484375	6.796875	7.109375	7.421875	7.734375	8.046875	8.359375	8.671875	8.984375	9.296875	9.609375	9.921875	10.234375	10.546875	10.859375
13	0.7265625	1.0390625	1.3515625	1.6640625	1.9765625	2.2890625	2.6015625	2.9140625	3.2265625	3.5390625	3.8515625	4.1640625	4.4765625	4.7890625	5.1015625	5.4140625	5.7265625	6.0390625	6.3515625	6.6640625	6.9765625	7.2890625	7.6015625	7.9140625	8.2265625	8.5390625	8.8515625	9.1640625	9.4765625	9.7890625	10.1015625	10.4140625	10.7265625
14	0.6171875	0.9296875	1.2421875	1.5546875	1.8671875	2.1796875	2.4921875	2.8046875	3.1171875	3.4296875	3.7421875	4.0546875	4.3671875	4.6796875	4.9921875	5.3046875	5.6171875	5.9296875	6.2421875	6.5546875	6.8671875	7.1796875	7.4921875	7.8046875	8.1171875	8.4296875	8.7421875	9.0546875	9.3671875	9.6796875	9.9921875	10.3046875	10.6171875
15	0.515625	0.828125	1.140625	1.453125	1.765625	2.078125	2.390625	2.703125	3.015625	3.328125	3.640625	3.953125	4.265625	4.578125	4.890625	5.203125	5.515625	5.828125	6.140625	6.453125	6.765625	7.078125	7.390625	7.703125	8.015625	8.328125	8.640625	8.953125	9.265625	9.578125	9.890625	10.203125	10.515625
16	0.4375	0.75	1.0625	1.375	1.6875	2	2.3125	2.625	2.9375	3.25	3.5625	3.875	4.1875	4.5	4.8125	5.125	5.4375	5.75	6.0625	6.375	6.6875	7	7.3125	7.625	7.9375	8.25	8.5625	8.875	9.1875	9.5	9.8125	10.125	10.4375
17	0.375	0.6875	1	1.3125	1.625	1.9375	2.25	2.5625	2.875	3.1875	3.5	3.8125	4.125	4.4375	4.75	5.0625	5.375	5.6875	6	6.3125	6.625	6.9375	7.25	7.5625	7.875	8.1875	8.5	8.8125	9.125	9.4375	9.75	10.0625	10.375
18	0.3125	0.625	0.9375	1.25	1.5625	1.875	2.1875	2.5	2.8125	3.125	3.4375	3.75	4.0625	4.375	4.6875	5	5.3125	5.625	5.9375	6.25	6.5625	6.875	7.1875	7.5	7.8125	8.125	8.4375	8.75	9.0625	9.375	9.6875	10	10.3125
19	0.2421875	0.5546875	0.8671875	1.1796875	1.4921875	1.8046875	2.1171875	2.4296875	2.7421875	3.0546875	3.3671875	3.6796875	3.9921875	4.3046875	4.6171875	4.9296875	5.2421875	5.5546875	5.8671875	6.1796875	6.4921875	6.8046875	7.1171875	7.4296875	7.7421875	8.0546875	8.3671875	8.6796875	8.9921875	9.3046875	9.6171875	9.9296875	10.2421875
20	0.2265625	0.5390625	0.8515625	1.1640625	1.4765625	1.7890625	2.1015625	2.4140625	2.7265625	3.0390625	3.3515625	3.6640625	3.9765625	4.2890625	4.6015625	4.9140625	5.2265625	5.5390625	5.8515625	6.1640625	6.4765625	6.7890625	7.1015625	7.4140625	7.7265625	8.0390625	8.3515625	8.6640625	8.9765625	9.2890625	9.6015625	9.9140625	10.2265625
V
");

			Map_FlowRateMultVsIAT = new ECUMap(@"
	-40	-30	-20	-10	0	10	20	30	40	50	60	70	80	90	100	110	120	°C
Multiplier	1.030029296875	1.0206298828125	1.01129150390625	1.00189208984375	0.9925537109375	0.983154296875	0.9737548828125	0.96441650390625	0.95501708984375	0.94561767578125	0.936279296875	0.9268798828125	0.91754150390625	0.90814208984375	0.89874267578125	0.889404296875	0.8800048828125
");

			Map_ShortPulseAdder = new ECUMap(@"
ms	0	0.125	0.25	0.375	0.5	0.625	0.75	0.875	1	1.125	1.25	1.375	1.5	1.625	1.75	1.875	2	2.125	2.25	2.375	2.5	2.625	2.75	2.875	3	3.125	3.25	3.375	3.5	3.625	3.75	3.875	4	x
PulseWidthAdder	0	5.75	4.375	3.125	1.875	1.125	0.625	0.25	0.125	0	-0.125	-0.125	-0.25	-0.25	-0.25	-0.25	-0.25	-0.25	-0.25	-0.25	-0.25	-0.125	-0.125	-0.125	-0.125	-0.125	-0.125	-0.125	-0.125	-0.125	-0.125	-0.125	0
");
		}
	}
}