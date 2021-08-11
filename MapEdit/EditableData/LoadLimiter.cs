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
	[DesignerCategory("Maps"), DisplayName("Load limiter by gear map")]
	public class LoadLimiter : EditableData {
		public LoadLimiter(EngineData EngineData) : base(EditMode.Grid) {
			XName = "Engine speed [RPM]";
			YName = "Gearbox gear";
			ValueName = "Engine load limit [% of max load]";
			DefaultValue = 100.0;
		}

		public override void PopulateSheet(Worksheet Sheet) {
			GenerateXAxis(Sheet, 26, (i) => string.Format("{0}", i * 500));
			GenerateYAxis(Sheet, 6, (i) => string.Format("{0}", i + 1));

			// Top left
			Sheet[0, 0] = 100.0;

			// Bottom left
			Sheet[Sheet.RowCount - 1, 0] = 100.0;

			// Top right
			Sheet[0, Sheet.ColumnCount - 1] = 100.0;

			// Bottom right
			Sheet[Sheet.RowCount - 1, Sheet.ColumnCount - 1] = 100.0;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(SolidColor.Red, SolidColor.Green, SolidColor.Blue, 0, 200, (float)Num, Center: 100);
		}
	}
}
