using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static Raylib_cs.Raylib;

namespace TurboCalculator {
    class Program {
        const int W = 900;
        const int H = 900;
        static Vector2 Origin;
        static Font TxtFnt;

        static void Main(string[] args) {
            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            SetConfigFlags(ConfigFlags.FLAG_WINDOW_HIGHDPI);
            InitWindow(W, H, "Turbo Calculator");
            SetTargetFPS(30);

            Origin = new Vector2(50, 50);
            TxtFnt = LoadFont("consola.ttf");

            LoadData();

            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                BeginDrawing();
                ClearBackground(Color.WHITE);

                DrawGrid();

                EndDrawing();
            }

            CloseWindow();
        }

        static float ScaleMassFlowAxis(float Pt) {
            return Pt * 5f;
        }

        static float ScalePressureRatioAxis(float Pt) {
            return (Pt - 1) * 350;
        }

        static Vector2 TransGraphPt(Vector2 Pt) {
            return new Vector2(ScaleMassFlowAxis(Pt.X), ScalePressureRatioAxis(Pt.Y)) + Origin;
        }

        static void Line(Vector2 A, Vector2 B, Color Clr, float Thick) {
            DrawLineEx(new Vector2(A.X, H - A.Y), new Vector2(B.X, H - B.Y), Thick, Clr);
        }

        static void DoText(string Text, int X, int Y, Color Clr) {
            DrawTextEx(TxtFnt, Text, new Vector2(X, Y), 16, 2, Clr);
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
            int TxtFont = 16;

            if (XAxis) {
                float XPos = ScaleMassFlowAxis(Val);
                Line(new Vector2(XPos, 5) + Offset, new Vector2(XPos, -5) + Offset, Color.BLACK, 2);

                int TextSize = MeasureText(Text, TxtFont);
                DoText(Text, (int)(Offset.X + XPos - TextSize / 2), H - (int)(Offset.Y - 10), Color.BLACK);
            } else {
                float YPos = ScalePressureRatioAxis(Val);
                Line(new Vector2(-5, YPos) + Offset, new Vector2(5, YPos) + Offset, Color.BLACK, 2);

                int TextSize = MeasureText(Text, TxtFont);
                DoText(Text, (int)(Offset.X - TextSize - 20), H - (int)(Offset.Y + YPos + TxtFont / 2), Color.BLACK);
            }
        }

        static Vector2[] SurgeLines;
        static Vector2[] PressureRatioMax;

        static double[,] Efficiency;
        static double[] Efficiency_XAxis;
        static double[] Efficiency_YAxis;

        static double[,] Speed;
        static double[] Speed_XAxis;
        static double[] Speed_YAxis;


        static double ParseDouble(string Txt) {
            return double.Parse(Txt, System.Globalization.CultureInfo.InvariantCulture);
        }


        static float ParseFloat(string Txt) {
            return (float)ParseDouble(Txt);
        }


        static Vector2[] Load1D(string Data) {
            List<Vector2> DataList = new List<Vector2>();

            string[] Lines = Data.Trim().Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            float[] XLine = Lines[0].Trim().Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(ParseFloat).ToArray();
            float[] YLine = Lines[1].Trim().Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(ParseFloat).ToArray();

            for (int i = 0; i < XLine.Length; i++) {
                DataList.Add(new Vector2(XLine[i], YLine[i]));
            }

            return DataList.ToArray();
        }

        static double[,] Load2D(string Data, out double[] XAxis, out double[] YAxis) {
            string[] Lines = Data.Trim().Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            double[][] FloatData = Lines.Select(L => L.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(ParseDouble).ToArray()).ToArray();

            XAxis = FloatData[0];
            YAxis = new double[FloatData.Length - 1];
            for (int i = 0; i < YAxis.Length; i++) {
                YAxis[i] = FloatData[i + 1][0];

                FloatData[i + 1] = FloatData[i + 1].Skip(1).ToArray();
            }

            FloatData = FloatData.Skip(1).ToArray();
            double[,] RetArray = new double[FloatData.Length, FloatData[0].Length];

            int X0 = RetArray.GetLowerBound(0);
            int X1 = RetArray.GetUpperBound(0) + 1;
            int Y0 = RetArray.GetLowerBound(1);
            int Y1 = RetArray.GetUpperBound(1) + 1;

            for (int Y = 0; Y < Y1; Y++) {
                for (int X = 0; X < X1; X++) {
                    RetArray[X, Y] = FloatData[X][Y];
                }
            }

            return RetArray;
        }

        static void LoadData() {
            SurgeLines = Load1D(File.ReadAllText("SURGE.csv"));
            PressureRatioMax = Load1D(File.ReadAllText("PRESSURE_RAT_MAX.csv"));

            Efficiency = Load2D(File.ReadAllText("EFFICIENCY.csv"), out Efficiency_XAxis, out Efficiency_YAxis);
            Speed = Load2D(File.ReadAllText("SPEED.csv"), out Speed_XAxis, out Speed_YAxis);
        }

        static void DrawGrid() {
            int X = (int)Origin.X;
            int Y = (int)Origin.Y;

            int Size = W - 100;

            Line(Origin, Origin + new Vector2(1, 0) * Size, Color.BLACK, 2);
            Line(Origin, Origin + new Vector2(0, 1) * Size, Color.BLACK, 2);


            for (int i = 0; i < 11; i++) {
                float Val = i * 15;
                float LbsMin = (int)Math.Round(Val * 0.132277, 0);

                AxisLabel(true, Val, String.Format("{0}|{1}", Val, LbsMin));
            }

            for (int i = 2; i < 10; i++) {
                float Val = i * 0.5f;
                AxisLabel(false, Val, Val.ToString());
            }


            // Surge lines
            TransLines(2, SurgeLines.ToArray());

            // Pressure ratio max
            TransLines(2, PressureRatioMax.ToArray());

            DrawEfficiencyLines();
            DrawSpeedLines();
        }

        static void DrawEfficiencyLines() {
            List<double> ZPoints = new List<double>();
            for (int i = 0; i < 25; i++) {
                ZPoints.Add((30 + i * 2) / 100.0);
            }

            double[] ZArr = ZPoints.ToArray();

            List<double> Drawn = new List<double>();
            Conrec.Contour(Efficiency, Efficiency_YAxis, Efficiency_XAxis, ZArr, new RendererDelegate((X1, Y1, X2, Y2, Z) => {
                Vector2 A = new Vector2((float)X1, (float)Y1);
                Vector2 B = new Vector2((float)X2, (float)Y2);

                A = TransGraphPt(A);
                B = TransGraphPt(B);
                Color Clr = GetColorForTurboEfficiency(Z);


                Line(A, B, Clr, 1);

                if (!Drawn.Contains(Z)) {
                    Drawn.Add(Z);
                    DoText(Z.ToString(), (int)A.X, H - (int)A.Y, Clr);
                }
            }));
        }

        static void DrawSpeedLines() {
            List<double> ZPoints = new List<double>();
            for (int i = 0; i < 40; i++) {
                ZPoints.Add(50000 + i * 25000);
            }

            List<double> Drawn = new List<double>();
            Conrec.Contour(Speed, Speed_YAxis, Speed_XAxis, ZPoints.ToArray(), new RendererDelegate((X1, Y1, X2, Y2, Z) => {
                Vector2 A = new Vector2((float)X1, (float)Y1);
                Vector2 B = new Vector2((float)X2, (float)Y2);

                A = TransGraphPt(A);
                B = TransGraphPt(B);
                Color Clr = GetColorForTurboSpeed(Z);

                Line(A, B, Clr, 1);

                if (!Drawn.Contains(Z)) {
                    Drawn.Add(Z);
                    DoText(Z.ToString(), (int)A.X, H - (int)A.Y, Clr);
                }
            }));
        }

        static Color GetColorForTurboEfficiency(double Eff) {
            float EffRange = 0.7f;

            if (Eff <= EffRange) {
                return Utils.Lerp(Color.RED, Color.GREEN, Utils.Normalize((float)Eff, 0.6f, EffRange));
            } else if (Eff > EffRange) {
                return Utils.Lerp(Color.GREEN, Color.BLUE, Utils.Normalize((float)Eff, EffRange, 0.8f));
            }

            return Color.BLACK;
        }

        static Color GetColorForTurboSpeed(double Spd) {
            float SpeedRange = 175000;

            if (Spd <= SpeedRange) {
                return Utils.Lerp(Color.BLUE, Color.GREEN, Utils.Normalize((float)Spd, 100000, SpeedRange));
            } else if (Spd > SpeedRange) {
                return Utils.Lerp(Color.GREEN, Color.RED, Utils.Normalize((float)Spd, SpeedRange, 225000));
            }

            return Color.BLACK;
        }
    }
}