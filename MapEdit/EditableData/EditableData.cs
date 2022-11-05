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

		public LookupTable2D Table;
		public ImageIndex Icon;

		[Browsable(false)]
		public string XAxisName {
			get {
				if (EditMode != EditMode.Grid)
					return null;

				return Table.Axis_X.AxisName;
			}
		}

		[Browsable(false)]
		public string YAxisName {
			get {
				if (EditMode != EditMode.Grid)
					return null;

				return Table.Axis_Y.AxisName;
			}
		}

		[Browsable(false)]
		public string Name {
			get {
				return Table.Name;
			}

			set {
				Table.Name = value;
			}
		}

		public EditableData DataProperties;

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

			switch (EditMode) {
				case EditMode.Grid:
					Icon = ImageIndex.TABLE;
					break;

				case EditMode.Property:
					Icon = ImageIndex.PROPERTY_EDIT;
					break;

				case EditMode.Nodes:
					Icon = ImageIndex.NODE_EDIT;
					break;

				default:
					throw new NotImplementedException();
			}
		}

		public EditableData(EditMode EditMode, LookupTable2D Table, string ValueName) : this(EditMode) {
			this.EditMode = EditMode;

			this.Table = Table;
			//this.ValueName = ValueName;
			//XAxisName = Table.Axis_X.AxisName;
			//YAxisName = Table.Axis_Y.AxisName;
		}

		protected void GenerateXAxis(Worksheet Sheet /*, int Count, Func<int, string> GenName*/) {
			if (EditMode != EditMode.Grid)
				throw new Exception("Invalid axis on grid control");

			int Count = Table.Axis_X.AxisLength;

			Sheet.ColumnCount = Count;
			Sheet.SetColumnsWidth(0, Count, 42);

			for (int i = 0; i < Count; i++) {
				Sheet.ColumnHeaders[i].Text = Table.Axis_X.Data[i].ToString();
			}
		}

		protected void GenerateYAxis(Worksheet Sheet /*, int Count, Func<int, string> GenName*/) {
			if (EditMode != EditMode.Grid)
				throw new Exception("Invalid axis on grid control");

			int Count = Table.Axis_Y.AxisLength;
			Sheet.RowCount = Count;

			Sheet.SetRowsHeight(0, Count, 20);

			for (int i = 0; i < Count; i++)
				Sheet.RowHeaders[i].Text = Table.Axis_Y.Data[i].ToString();
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
			C.Style.BackColor = SolidColor.White;

			if (C.Data is double D) {
				double XX = 0;

				double Offset = 15;
				D += Offset;

				if (D < 0)
					XX = 0;
				else if (D < Offset)
					XX = D / Offset;
				else if (D >= Offset)
					XX = 1.0 + ((D - Offset) / 40.0);

				C.Style.BackColor = Utils.Lerp(SolidColor.FromArgb(70, 230, 60), SolidColor.FromArgb(255, 240, 60), SolidColor.FromArgb(255, 160, 90), (float)XX);
			}
		}
	}
}
