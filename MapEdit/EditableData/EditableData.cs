using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;

namespace MapEdit {
	[DesignerCategory("Misc")]
	public class EditableData {
		public EditMode EditMode;

		public string XName;
		public string YName;
		public string ValueName;
		public object DefaultValue;
		public Worksheet Worksheet;

		[Browsable(false)]
		public virtual bool DataEnabled {
			get {
				return true;
			}
		}

		public EditableData(EditMode EditMode) {
			this.EditMode = EditMode;
			XName = "X Axis";
			YName = "Y Axis";
			ValueName = "Value";
			Worksheet = null;
		}

		protected void GenerateXAxis(Worksheet Sheet, int Count, Func<int, string> GenName) {
			Sheet.ColumnCount = Count;
			Sheet.SetColumnsWidth(0, Count, 45);

			for (int i = 0; i < Count; i++)
				Sheet.ColumnHeaders[i].Text = GenName(i);
		}

		protected void GenerateYAxis(Worksheet Sheet, int Count, Func<int, string> GenName) {
			Sheet.RowCount = Count;

			for (int i = 0; i < Count; i++)
				Sheet.RowHeaders[i].Text = GenName(i);
		}

		public virtual void PopulateSheet(Worksheet Sheet) {
		}

		public virtual void ColorCell(int X, int Y, object Value, ref Cell C) {
		}
	}
}
