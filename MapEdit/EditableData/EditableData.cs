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

		//public string XName;
		//public string YName;
		public AxisParameter XParam;
		public AxisParameter YParam;

		public double TopLeft;
		public double BottomLeft;
		public double TopRight;
		public double BottomRight;

		public string ValueName;

		public object DefaultValue;
		public Worksheet Worksheet;

		[Browsable(false)]
		public virtual bool DataEnabled {
			get {
				return true;
			}
		}

		public EditableData(EditMode EditMode, AxisParameter XParam, AxisParameter YParam) {
			this.EditMode = EditMode;

			//XName = "X Axis";
			//YName = "Y Axis";
			this.XParam = XParam;
			this.YParam = YParam;

			ValueName = "Value";
			Worksheet = null;
		}

		protected void GenerateXAxis(Worksheet Sheet /*, int Count, Func<int, string> GenName*/) {
			if (EditMode != EditMode.Grid)
				throw new Exception("Invalid axis on grid control");

			int Count = XParam.Count;

			Sheet.ColumnCount = Count;
			Sheet.SetColumnsWidth(0, Count, 45);

			for (int i = 0; i < Count; i++)
				Sheet.ColumnHeaders[i].Text = XParam.ColumnName[i]; //GenName(i);
		}

		protected void GenerateYAxis(Worksheet Sheet /*, int Count, Func<int, string> GenName*/) {
			if (EditMode != EditMode.Grid)
				throw new Exception("Invalid axis on grid control");

			int Count = YParam.Count;
			Sheet.RowCount = Count;

			/*for (int i = 0; i < Count; i++)
				Sheet.RowHeaders[Count - i - 1].Text = GenName(i);*/

			for (int i = 0; i < Count; i++)
				Sheet.RowHeaders[Count - i - 1].Text = YParam.ColumnName[i]; //GenName(i);
		}

		public virtual void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet);
			GenerateYAxis(Sheet);

			Sheet[0, 0] = TopLeft;
			Sheet[Sheet.RowCount - 1, 0] = BottomLeft;
			Sheet[0, Sheet.ColumnCount - 1] = TopRight;
			Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = BottomRight;
		}

		public virtual void ColorCell(int X, int Y, object Value, ref Cell C) {
		}
	}
}
