using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace InjectorCalculator {
	class RaylibGrid {
		Vector2 Pos;
		Vector2 Size;

		public Vector2 AxisSize;
		public Vector2 AxisDiv;

		string XName;
		string YName;

		int W;
		int H;

		Font Fnt;

		public RaylibGrid(Vector2 Pos, Vector2 Size, int W, int H) {
			this.Pos = Pos;
			this.Size = Size;

			this.W = W;
			this.H = H;
		}

		public void SetAxis(string XName, string YName, Vector2 AxisSize, Vector2 AxisDiv) {
			this.XName = XName;
			this.YName = YName;

			this.AxisSize = AxisSize;
			this.AxisDiv = AxisDiv;
		}

		public void SetFont(Font Fnt) {
			this.Fnt = Fnt;
		}

		float ScaleXAxis(float V) {
			return V * (Size.X / AxisSize.X);
		}

		float ScaleYAxis(float V) {
			return V * (Size.Y / AxisSize.Y);
		}

		void PlotText(string Text, int X, int Y, Color Clr) {
			DrawTextEx(Fnt, Text, new Vector2(X, Y), 16, 2, Clr);
		}

		void PlotAxisLabel(bool XAxis, float Val, string Text) {
			Vector2 Offset = Pos;
			int TxtFont = 16;

			if (XAxis) {
				float XPos = ScaleXAxis(Val);
				PlotLine(new Vector2(XPos, 0) + Offset, new Vector2(XPos, -5) + Offset, Color.WHITE, 2);

				int TextSize = MeasureText(Text, TxtFont);

				PlotText(Text, (int)(Offset.X + XPos - TextSize / 2), H - (int)(Offset.Y - 12), Color.WHITE);
			} else {
				float YPos = ScaleYAxis(Val);
				PlotLine(new Vector2(-5, YPos) + Offset, new Vector2(0, YPos) + Offset, Color.WHITE, 2);

				int TextSize = MeasureText(Text, TxtFont);

				PlotText(Text, (int)(Offset.X - TextSize - 24), H - (int)(Offset.Y + YPos + TxtFont / 2), Color.WHITE);
			}
		}

		void PlotLine(Vector2 A, Vector2 B, Color Clr, float Thick) {
			DrawLineEx(new Vector2(A.X, H - A.Y), new Vector2(B.X, H - B.Y), Thick, Clr);
		}

		public void PlotPixel(Vector2 Val, Color Clr) {
			int X = (int)(Pos.X + ScaleXAxis(Val.X));
			int Y = (int)(Pos.Y + ScaleYAxis(Val.Y));

			DrawPixel(X, H - Y, Clr);
		}

		List<Vector2> Points = new List<Vector2>();
		bool UsePixels = false;

		public void BeginLine(bool UsePixels) {
			this.UsePixels = UsePixels;
			Points.Clear();
		}

		public void PlotLinePoint(Vector2 Val) {
			int X = (int)(Pos.X + ScaleXAxis(Val.X));
			int Y = (int)(Pos.Y + ScaleYAxis(Val.Y));

			if (UsePixels)
				DrawPixel(X, H - Y, Color.RED);
			else
				Points.Add(new Vector2(X, Y));
		}

		public void EndLine(float Thick, Color Clr) {
			if (UsePixels)
				return;

			for (int i = 1; i < Points.Count; i++) {
				Vector2 A = Points[i - 1];
				Vector2 B = Points[i];

				DrawLineEx(new Vector2(A.X, H - A.Y), new Vector2(B.X, H - B.Y), Thick, Clr);
			}
		}

		public void Draw() {
			PlotLine(Pos, Pos + new Vector2(Size.X, 0), Color.WHITE, 1);
			PlotLine(Pos, Pos + new Vector2(0, Size.Y), Color.WHITE, 1);

			int XCount = (int)(AxisSize.X / AxisDiv.X) + 1;
			for (int X = 0; X < XCount; X++) {
				float Div = AxisDiv.X;
				float Val = Div * X;

				PlotAxisLabel(true, Val, Val.ToString());
			}

			int YCount = (int)(AxisSize.Y / AxisDiv.Y) + 1;
			for (int Y = 0; Y < YCount; Y++) {
				float Div = AxisDiv.Y;
				float Val = Div * Y;

				Val = (float)Math.Round(Val, 3);

				PlotAxisLabel(false, Val, Val.ToString());
			}
		}
	}
}