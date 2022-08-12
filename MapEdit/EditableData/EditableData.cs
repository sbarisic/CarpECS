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

		/*public AxisParameter XParam;
		public AxisParameter YParam;

		public double TopLeft;
		public double BottomLeft;
		public double TopRight;
		public double BottomRight;*/

		LookupTable2D Table;
		public string XAxisName;
		public string YAxisName;

		public string ValueName;

		public object DefaultValue;
		public Worksheet Worksheet;

		[Browsable(false)]
		public virtual bool DataEnabled {
			get {
				return true;
			}
		}

		/*public EditableData(EditMode EditMode, AxisParameter XParam, AxisParameter YParam) {
			this.EditMode = EditMode;

			//XName = "X Axis";
			//YName = "Y Axis";
			this.XParam = XParam;
			this.YParam = YParam;

			ValueName = "Value";
			Worksheet = null;
		}*/

		public EditableData(EditMode EditMode) {
			this.EditMode = EditMode;
		}

		public EditableData(EditMode EditMode, LookupTable2D Table) {
			this.EditMode = EditMode;

			this.Table = Table;
			XAxisName = Table.Axis_X.AxisName;
			YAxisName = Table.Axis_Y.AxisName;
		}

		protected void GenerateXAxis(Worksheet Sheet /*, int Count, Func<int, string> GenName*/) {
			if (EditMode != EditMode.Grid)
				throw new Exception("Invalid axis on grid control");

			int Count = Table.Axis_X.AxisLength;

			Sheet.ColumnCount = Count;
			Sheet.SetColumnsWidth(0, Count, 45);

			for (int i = 0; i < Count; i++)
				Sheet.ColumnHeaders[i].Text = Table.Axis_X.Data[i].ToString();
		}

		protected void GenerateYAxis(Worksheet Sheet /*, int Count, Func<int, string> GenName*/) {
			if (EditMode != EditMode.Grid)
				throw new Exception("Invalid axis on grid control");

			int Count = Table.Axis_Y.AxisLength;
			Sheet.RowCount = Count;

			for (int i = 0; i < Count; i++)
				Sheet.RowHeaders[Count - i - 1].Text = Table.Axis_Y.Data[i].ToString();
		}

		public virtual void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet);
			GenerateYAxis(Sheet);

			for (int Y = 0; Y < Table.Axis_Y.AxisLength; Y++) {
				for (int X = 0; X < Table.Axis_X.AxisLength; X++) {
					Sheet[Y, X] = (double)Table.GetDataRaw(X, Y);
				}
			}
		}

		public virtual void ColorCell(int X, int Y, object Value, ref Cell C) {
		}
	}
}
