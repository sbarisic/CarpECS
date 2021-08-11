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

	[DesignerCategory("Maps"), DisplayName("Ignition advance map")]
	public class IgnitionAdvanceMap : EditableData {
		EngineData EngineData;

		public IgnitionAdvanceMap(EngineData EngineData) : base(EditMode.Grid) {
			this.EngineData = EngineData;

			XName = "Engine speed [RPM]";
			YName = "Engine load [%]";
			ValueName = "Spark advance [° before TDC]";
			DefaultValue = 0.0;
		}

		public override void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet, 26, (i) => string.Format("{0}", i * 500));
			GenerateYAxis(Sheet, 21, (i) => string.Format("{0} %", i * 5));

			Sheet[0, 0] = Sheet[Sheet.RowCount - 1, 0] = 2.0;
			Sheet[0, Sheet.ColumnCount - 1] = Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = -30.0;

			// Top left
			Sheet[0, 0] = 10.0;

			// Bottom left
			Sheet[Sheet.RowCount - 1, 0] = -10.0;

			// Top right
			Sheet[0, Sheet.ColumnCount - 1] = 10.0;

			// Bottom right
			Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = 10.0;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(SolidColor.Blue, SolidColor.Green, SolidColor.Red, -20, 20, (float)Num, Center: 0);
		}
	}

}
