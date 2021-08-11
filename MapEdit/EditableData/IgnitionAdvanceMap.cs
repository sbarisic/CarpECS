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
		public IgnitionAdvanceMap(EngineData EngineData) : base(EditMode.Grid, AxisParameters.RPM, AxisParameters.EngineLoad) {
			ValueName = "Spark advance [° before TDC]";
			DefaultValue = 0.0;

			TopLeft = 10;
			BottomLeft = -10;
			TopRight = 10;
			BottomRight = 10;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(SolidColor.Blue, SolidColor.Green, SolidColor.Red, -20, 20, (float)Num, Center: 0);
		}
	}

}
