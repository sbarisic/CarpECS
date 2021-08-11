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

	[DesignerCategory("Maps"), DisplayName("Fuel map")]
	public class FuelMap : EditableData {
		EngineData EngineData;

		public override bool DataEnabled => !EngineData.LambdaEnabled;

		public FuelMap(EngineData EngineData) : base(EditMode.Grid) {
			this.EngineData = EngineData;

			XName = "Engine speed [RPM]";
			YName = "Engine load [%]";
			ValueName = "Target A/F";
			DefaultValue = 10.0;
		}

		public override void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet, 26, (i) => string.Format("{0}", i * 500));
			GenerateYAxis(Sheet, 21, (i) => string.Format("{0} %", i * 5));

			// Top left
			Sheet[0, 0] = 14.7;

			// Bottom left
			Sheet[Sheet.RowCount - 1, 0] = 14.0;

			// Top right
			Sheet[0, Sheet.ColumnCount - 1] = 13.0;

			// Bottom right
			Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = 11.0;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(new SolidColor(104, 162, 255), SolidColor.Green, new SolidColor(255, 0, 0), EngineData.MinAFR, EngineData.MaxAFR, (float)Num);
		}
	}
}
