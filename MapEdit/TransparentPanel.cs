using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEdit {
	public class TransparentPanel : Panel {
		public Action<Graphics> DoPaint;

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				// cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
				return cp;
			}
		}

		public TransparentPanel() {
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserPaint, true);

			BackColor = Color.Transparent;
		}

		protected override void OnPaint(PaintEventArgs e) {
			// base.OnPaint(e);
			DoPaint(e.Graphics);
		}
	}
}
