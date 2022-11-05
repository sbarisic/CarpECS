using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using unvell.ReoGrid;
using unvell.ReoGrid.Drawing;
using unvell.ReoGrid.Drawing.Shapes;
using unvell.ReoGrid.Rendering;
using unvell.ReoGrid.Graphics;
using System.Drawing;

using ReoPoint = unvell.ReoGrid.Graphics.Point;

namespace MapEdit.ReoGridCtls {
	public class TailShape : PathShape {
		List<Vector2> TailPoints = new List<Vector2>();

		public TailShape() {
			Style.FillColor = SolidColor.Transparent;
			Style.LineWidth = 20;

			Location = new ReoPoint(0, 0);
			Size = new unvell.ReoGrid.Graphics.Size(1, 1);
		}

		public void PushPoint(Vector2 Pt) {
			while (TailPoints.Count > 121)
				TailPoints.RemoveAt(0);

			TailPoints.Add(Pt);
			UpdatePath();
			Invalidate();
		}

		public void PushPoint(ReoPoint Pt) {
			PushPoint(new Vector2(Pt.X, Pt.Y));
		}

		protected override void UpdatePath() {
			Path.Reset();

			if (TailPoints.Count <= 1)
				return;

			for (int i = 1; i < TailPoints.Count; i++) {
				Vector2 A = TailPoints[i - 1];
				Vector2 B = TailPoints[i];

				Path.AddLine(A.X, A.Y, B.X, B.Y);
			}
		}
	}
}
