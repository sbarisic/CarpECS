using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RotatingLabel {
	public partial class RotatingLabel : Label {
		private int m_RotateAngle = 0;
		private string m_NewText = string.Empty;

		public int RotateAngle { get { return m_RotateAngle; } set { m_RotateAngle = value; Invalidate(); } }
		public string NewText { get { return m_NewText; } set { m_NewText = value; Invalidate(); } }

		protected override void OnPaint(PaintEventArgs e) {
			Func<double, double> DegToRad = (angle) => Math.PI * angle / 180.0;

			Brush brush = new SolidBrush(ForeColor);
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			SizeF size = e.Graphics.MeasureString(NewText, Font, Parent.Width);

			int normalAngle = ((RotateAngle % 360) + 360) % 360;
			double normaleRads = DegToRad(normalAngle);

			int hSinTheta = (int)Math.Ceiling((size.Height * Math.Sin(normaleRads)));
			int wCosTheta = (int)Math.Ceiling((size.Width * Math.Cos(normaleRads)));
			int wSinTheta = (int)Math.Ceiling((size.Width * Math.Sin(normaleRads)));
			int hCosTheta = (int)Math.Ceiling((size.Height * Math.Cos(normaleRads)));

			int rotatedWidth = Math.Abs(hSinTheta) + Math.Abs(wCosTheta);
			int rotatedHeight = Math.Abs(wSinTheta) + Math.Abs(hCosTheta);

			this.Width = rotatedWidth;
			this.Height = rotatedHeight;

			int numQuadrants =
				(normalAngle >= 0 && normalAngle < 90) ? 1 :
				(normalAngle >= 90 && normalAngle < 180) ? 2 :
				(normalAngle >= 180 && normalAngle < 270) ? 3 :
				(normalAngle >= 270 && normalAngle < 360) ? 4 :
				0;

			int ShiftX = 0;
			int ShiftY = 0;

			if (numQuadrants == 1) {
				ShiftX = Math.Abs(hSinTheta);
			} else if (numQuadrants == 2) {
				ShiftX = rotatedWidth;
				ShiftY = Math.Abs(hCosTheta);
			} else if (numQuadrants == 3) {
				ShiftX = Math.Abs(wCosTheta);
				ShiftY = rotatedHeight;
			} else if (numQuadrants == 4) {
				ShiftY = Math.Abs(wSinTheta);
			}

			e.Graphics.TranslateTransform(ShiftX, ShiftY);
			e.Graphics.RotateTransform(this.RotateAngle);

			e.Graphics.DrawString(this.NewText, this.Font, brush, 0f, 0f);
			base.OnPaint(e);
		}
	}
}