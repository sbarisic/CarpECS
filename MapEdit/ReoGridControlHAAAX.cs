using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;
using MapEdit;

namespace MapEditNamespace {
	public class ReoGridControlHAAAX : ReoGridControl {
		public bool EnableTracer = false;
		public EditableData CurrentData;

		Timer UpdateTimer;

		int TracerX;
		int TracerY;

		int[] TracerHistoryX = new int[20];
		int[] TracerHistoryY = new int[20];

		Color[] Colors = new Color[] {
			Color.FromArgb(235, 52, 52),
			Color.FromArgb(235, 95, 52),
			Color.FromArgb(235, 125, 52),
			Color.FromArgb(235, 156, 52),
			Color.FromArgb(235, 180, 52),
			Color.FromArgb(235, 198, 52),
			Color.FromArgb(235, 223, 52),
			Color.FromArgb(205, 235, 52),
			Color.FromArgb(153, 235, 52),
			Color.FromArgb(104, 235, 52),
			Color.FromArgb(52, 235, 98),
			Color.FromArgb(52, 235, 180),
			Color.FromArgb(52, 235, 232),
			Color.FromArgb(52, 168, 235),
			Color.FromArgb(52, 131, 235),
			Color.FromArgb(52, 86, 235),
			Color.FromArgb(55, 52, 235),
			Color.FromArgb(110, 52, 235),
			Color.FromArgb(159, 52, 235),
			Color.FromArgb(201, 52, 235)
		};

		public ReoGridControlHAAAX() {
			UpdateTimer = new Timer();
			UpdateTimer.Interval = 100;
			UpdateTimer.Tick += UpdateTimer_Tick;
			UpdateTimer.Start();
		}

		private void UpdateTimer_Tick(object sender, EventArgs e) {
			if (EnableTracer && Program.ECUMonitor.MonitorAvailable()) {
				//UpdateTracer();
				Invalidate();
			}
		}

		/*public void UpdateTracer() {
			if (CurrentData == null)
				return;

			TracerX = CurrentData.XParam.GetMonitorColumnIndex();
			TracerY = CurrentData.YParam.GetMonitorColumnIndex();

			for (int i = 1; i < TracerHistoryX.Length; i++) {
				TracerHistoryX[i - 1] = TracerHistoryX[i];
				TracerHistoryY[i - 1] = TracerHistoryY[i];
			}

			TracerHistoryX[TracerHistoryX.Length - 1] = TracerX;
			TracerHistoryY[TracerHistoryX.Length - 1] = TracerY;
		}

		void DrawTracer(Graphics Gfx, float X, float Y) {
			float Size = 10;
			Gfx.FillEllipse(Brushes.Red, X - Size / 2, Y - Size / 2, Size, Size);
		}*/

		float CalcX(int Column) {
			float X = CurrentWorksheet.RowHeaderWidth / 2;

			for (int i = 0; i < Column + 1; i++) {
				X += CurrentWorksheet.ColumnHeaders[i].Width;
			}

			return X;
		}

		float CalcY(int Row) {
			float Y = CurrentWorksheet.RowHeaders[0].Height / 2;
			for (int i = 0; i < CurrentWorksheet.RowCount; i++)
				Y += CurrentWorksheet.RowHeaders[i].Height;

			for (int i = Row - 1; i >= 0; i--) {
				Y -= CurrentWorksheet.RowHeaders[i].Height;
			}

			return Y;
		}

		/*void DrawTracer(Graphics Gfx, int Column, int Row) {
			if (Column < 0 || Row < 0 || Column >= CurrentWorksheet.ColumnCount || Row >= CurrentWorksheet.RowCount)
				return;

			DrawTracer(Gfx, CalcX(Column), CalcY(Row));
		}*/

		void DrawTracerLine(Graphics Gfx, int StartColumn, int StartRow, int EndColumn, int EndRow, Color Clr) {
			if (!PosValid(StartColumn, StartRow) || !PosValid(EndColumn, EndRow))
				return;

			Gfx.DrawLine(new Pen(Clr, 3), CalcX(StartColumn), CalcY(StartRow), CalcX(EndColumn), CalcY(EndRow));
		}

		bool PosValid(int Column, int Row) {
			if (Column < 0 || Row < 0 || Column >= CurrentWorksheet.ColumnCount || Row >= CurrentWorksheet.RowCount)
				return false;

			return true;
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			if (EnableTracer) {
				for (int i = 1; i < TracerHistoryX.Length; i++) {
					DrawTracerLine(e.Graphics, TracerHistoryX[i - 1], TracerHistoryY[i - 1], TracerHistoryX[i], TracerHistoryY[i], Colors[i]);
				}

				//DrawTracer(e.Graphics, TracerX, TracerY);
			}
		}
	}
}
