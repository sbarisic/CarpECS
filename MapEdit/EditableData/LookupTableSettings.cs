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
	[DesignerCategory("Properties"), DisplayName("LookupTableSettings")]
	public class LookupTableSettings : EditableData {
		LookupTable2D Tbl;

		public LookupTableSettings(LookupTable2D Tbl) : base(EditMode.Property) {
			this.Tbl = Tbl;
		}

		[Description("Table Name"), Category("Table")]
		public string TableName {
			get {
				return Tbl.Name;
			}

			set {
				string Val = value?.Trim() ?? "";

				if (string.IsNullOrEmpty(Val)) {
					Val = GetType().Name;
				}

				Tbl.Name = Val;
			}
		}

		[Description("Axis X Name"), Category("Axis")]
		public string XName {
			get {
				return Tbl.Axis_X.AxisName;
			}

			set {
				Tbl.Axis_X.AxisName = value;
			}
		}

		[Description("Axis Y Name"), Category("Axis")]
		public string YName {
			get {
				return Tbl.Axis_Y.AxisName;
			}

			set {
				Tbl.Axis_Y.AxisName = value;
			}
		}

		// Rows
		[Description("Row Count"), Category("Row")]
		public int Rows {
			get {
				return Tbl.Axis_Y.AxisLength;
			}
		}

		[Description("Rows"), Category("Row")]
		public string RowValues {
			get {
				return string.Join(" ", Tbl.Axis_Y.Data.Select(D => D.ToString()));
			}

			set {
				double[] NewData = ParseStringCSV(value);

				if (NewData != null && NewData.Length == Tbl.Axis_Y.Data.Length)
					Tbl.Axis_Y.Data = NewData;
			}
		}

		// Columns
		[Description("Column Count"), Category("Column")]
		public int Columns {
			get {
				return Tbl.Axis_X.AxisLength;
			}
		}

		[Description("Columns"), Category("Column")]
		public string ColumnValues {
			get {
				return string.Join(" ", Tbl.Axis_X.Data.Select(D => D.ToString()));
			}

			set {
				double[] NewData = ParseStringCSV(value);

				if (NewData != null && NewData.Length == Tbl.Axis_X.Data.Length)
					Tbl.Axis_X.Data = NewData;
			}
		}

		double[] ParseStringCSV(string Src) {
			try {
				string[] SplitSrc = Src.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				return SplitSrc.Select(Utils.ParseDouble).ToArray();
			} catch (Exception) {
				return null;
			}
		}
	}
}
