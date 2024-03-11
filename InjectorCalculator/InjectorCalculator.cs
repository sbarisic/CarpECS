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

			//CSVPoint[] Points = HPTCSV.ParseEntriesHPT("test1.csv");

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

				float MS = CalcPWFord(FuelMass, false, out float MSIdeal);

				Grid.PlotPixel(new Vector2(MSIdeal, FuelMass), Color.GRAY);
				Grid.PlotLinePoint(new Vector2(MS, FuelMass));
			}

			Grid.EndLine(1, Color.BLUE);//*/

            //---


            Grid.BeginLine(false);

            for (int i = 0; i < MassCount; i++) {
                float FuelMass = i * MassStep;

				float MS = CalcPWFord(FuelMass, true, out float MSIdeal);

				Grid.PlotPixel(new Vector2(MSIdeal, FuelMass), Color.GRAY);
				Grid.PlotLinePoint(new Vector2(MS, FuelMass));
			}

			Grid.EndLine(1, Color.GREEN);

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
		const float BatteryVoltage = 13.0f;
		const bool IgnoreIGNV = false;

		static float CalcPWFord(float FuelMass, bool Alternate, out float PWIdeal) {
			//float PressureDropRef = 270.0f; // kPa
			float RailFuelPressure = 270.0f; // kPa

			if (Alternate) {
				RailFuelPressure = 360.0f;
			}

			float FlowRateLow = 9.322684f; // g/s
			float FlowRateHigh = 7.165399f; // g/s
			float Breakpoint = 0.006549868f; // g

            float BreakpointMult = Map_BreakpointMult.Index(0, RailFuelPressure);
            float LowMult = Map_FlowRateLowMultVsPressure.Index(0, RailFuelPressure);
            float HighMult = Map_FlowRateHighMultVsPressure.Index(0, RailFuelPressure);


            float Offset = Map_Offset.Index(0, BatteryVoltage);
            float OffsetMult = Map_OffsetMult.Index(0, RailFuelPressure);
            float IGNV = Offset * OffsetMult;

			if (IgnoreIGNV) {
				IGNV = 0;
			}

			float CalcBreakpoint = Breakpoint * BreakpointMult;

			// Fuel mass breakpoint for low and high flow curves
			float HiOffset1 = (CalcBreakpoint / (FlowRateHigh * HighMult)) * 1000;
			float LoOffset1 = (CalcBreakpoint / (FlowRateLow * LowMult)) * 1000;
			float Diff = HiOffset1 - LoOffset1;

			float LowCurveOffset = 0;
			float CurrentFlowRate = 0;

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
			PWIdeal = (FuelMass / (FlowRateHigh * HighMult)) * 1000;

			// Current pulse width = ideal + injector lag + ideal curve from low and high compensation
			float PWCur = (FuelMass / CurrentFlowRate) * 1000 + IGNV + LowCurveOffset;

            // Current pulse width = ideal + injector lag + ideal curve from low and high compensation
            float PWCur = FuelMass / CurrentFlowRate * 1000 + IGNV + LowCurveOffset;


            /*if (PWIdeal >= 0 && PWIdeal <= 4f) {
				Console.WriteLine("{0} - {1}", ShortPulseAdder, PWIdeal);
			} else if (PWIdeal > 4)
				Console.WriteLine("Done!");*/

            return PWCur;
        }

		static float CalcPW(float FuelMass, out float PWIdeal) {
			float ShortPulseLimit = 4;
			float StaticFlowRate = 7.181884765625f; // 6.2998046875f; // g/s

            float StaticFlowRateLo = 8.784600696766525f; // g/s
            float Bkpt = 0.0075694814317f; // g

			float OffsetPressIGNV = Map_OffsetPressIGNV.Index(0, BatteryVoltage);

			if (IgnoreIGNV) {
				OffsetPressIGNV = 0;
			}


			float FlowRateMultVsIAT = Map_FlowRateMultVsIAT.Index(IAT, 0);

            float FlowRate = StaticFlowRate * FlowRateMultVsIAT;
            float MsLoOffset = 0;

            if (FuelMass < Bkpt) {
                FlowRate = StaticFlowRateLo * FlowRateMultVsIAT;
                MsLoOffset += 0.15f;
            }

			float ShortPulseAdder = 0;
			bool NEW_STYLE_SPA_VALUES = true; // if true, divides values by 16

			if (MS < ShortPulseLimit) {
				ShortPulseAdder = Map_ShortPulseAdder.Index(MS, 0);

				if (NEW_STYLE_SPA_VALUES) {
					ShortPulseAdder = ShortPulseAdder / 16;
				}
			}

			return MS + ShortPulseAdder + OffsetPressIGNV;

            return (MS - ShortPulseAdder) + OffsetPressIGNV + MsLoOffset;

        }
        static void LoadData() {
            Map_OffsetPressIGNV = new ECUMap(@"
ms	128	148	168	188	208	228	248	268	288	308	328	348	368	388	408	428	448	468	488	508	528	548	568	588	608	628	648	668	688	708	728	748	768	kPa
4	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0
5	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0
6	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875	7.71875
7	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125	5.3203125
8	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625	3.140625
9	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625	2.2890625
10	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125	1.828125
11	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875	1.546875
12	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375	1.3359375
13	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875	1.171875
14	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875	1.046875
15	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875	0.9296875
16	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125	0.8828125
17	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375	0.8359375
18	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125	0.8203125
19	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125	0.8125
20	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875	0.796875
V
");

            Map_FlowRateMultVsIAT = new ECUMap(@"
	-40	-30	-20	-10	0	10	20	30	40	50	60	70	80	90	100	110	120	°C
Multiplier	1	1	1	1	1	1	1	1	1	1	1	1	1	1	1	1	1
");

			Map_ShortPulseAdder = new ECUMap(@"
ms	0	0.125	0.25	0.375	0.5	0.625	0.75	0.875	1	1.125	1.25	1.375	1.5	1.625	1.75	1.875	2	2.125	2.25	2.375	2.5	2.625	2.75	2.875	3	3.125	3.25	3.375	3.5	3.625	3.75	3.875	4	
PulseWidthAdder	0	3	2.5	2.046875	1.6015625	1.15625	0.7109375	0.265625	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0	0
");



            //-- Ford

            Map_FlowRateLowMultVsPressure = new ECUMap(@"
x	0	x
138.03303829616928	0.7149999737739563
206.98060829616927	0.8759999871253967
269.99868306794355	1
345.01364659233855	1.1299999952316284
414.03015258971783	1.2380000352859497
482.9777488911285	1.3370000123977661
kPa
");

            Map_FlowRateHighMultVsPressure = new ECUMap(@"
x	0	x
138.03303829616928	0.7149999737739563
206.98060829616927	0.8759999871253967
269.99868306794355	1
345.01364659233855	1.1299999952316284
414.03015258971783	1.2380000352859497
482.9777488911285	1.3370000123977661
kPa
");

            Map_BreakpointMult = new ECUMap(@"	
x	0	x
138.03303829616928	0.7590000033378601
206.98060829616927	0.9100000262260437
269.99868306794355	1
345.01364659233855	1.0850000381469727
414.03015258971783	1.4079999923706055
482.9777488911285	1.4630000591278076
kPa
");

            Map_Offset = new ECUMap(@"
x	0	x
6	7.714988198131323
7	5.323078949004412
8	2.931169932708144
9	2.3803659714758396
10	1.8295630579814315
11	1.5498809516429901
12	1.3358270516619086
13	1.1716129956766963
13.5	1.1078090174123645
14	1.0440050391480327
14.5	0.987946055829525
15	0.9318860247731209
V
");

            Map_OffsetMult = new ECUMap(@"
x	0	x
138.03303829616928	0.921999990940094
206.98060829616927	0.949999988079071
269.99868306794355	1
345.01364659233855	1.0479999780654907
414.03015258971783	1.0110000371932983
482.9777488911285	1.1019999980926514
482.9777488911285	1.1019999980926514
kPa
");
        }
    }
}