using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;

namespace MapEdit {
	public class ReoGridControlHAAAX : ReoGridControl {
		public bool EnableTracer = false;
		public EditableData CurrentData;

		float TracerX;
		float TracerY;

		public void UpdateTracer() {
			if (CurrentData == null)
				return;

			TracerX = (float)CurrentData.XParam.GetMonitorValue();
			TracerY = (float)CurrentData.YParam.GetMonitorValue();
		}

		void DrawTracer(Graphics Gfx, float X, float Y) {
			float Size = 10;
			Gfx.FillEllipse(Brushes.Red, X - Size / 2, Y - Size / 2, Size, Size);
		}

		void DrawTracer(Graphics Gfx, int Column, int Row) {
			float X = CurrentWorksheet.RowHeaderWidth;


			float Y = CurrentWorksheet.RowHeaders[0].Height;

			for (int i = 0; i < Column + 1; i++) {
				X += CurrentWorksheet.ColumnHeaders[i].Width;
			}

			for (int i = 0; i < Row + 1; i++) {
				Y += CurrentWorksheet.RowHeaders[i].Height;


			}


			DrawTracer(Gfx, TracerX, TracerY);
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			if (EnableTracer) {
			}


			DrawTracer(e.Graphics, 0, 0);
		}
	}
}
