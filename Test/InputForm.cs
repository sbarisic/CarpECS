using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test {
	public partial class InputForm : Form {
		public InputForm() {
			InitializeComponent();
		}

		private void InputForm_Load(object sender, EventArgs e) {
			List<Button> IntButtons = new List<Button>() { button1, button2, button3, button4, button5, button6, button7 };

			foreach (var B in IntButtons)
				B.Click += OnClick;
		}

		void OnClick(object S, EventArgs E) {
			Program.Interrupt(int.Parse(((Button)S).Text));
		}
	}
}
