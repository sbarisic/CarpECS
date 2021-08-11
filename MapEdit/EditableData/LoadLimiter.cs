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
		public LoadLimiter(EngineData EngineData) : base(EditMode.Grid, AxisParameters.RPM, AxisParameters.GearboxGear) {
			//XName = "Engine speed [RPM]";
			//YName = "Gearbox gear";

			ValueName = "Engine load limit [% of max load]";
			DefaultValue = 100.0;

			TopLeft = 100;
			BottomLeft = 100;
			TopRight = 100;
			BottomRight = 100;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(SolidColor.Red, SolidColor.Green, SolidColor.Blue, 0, 200, (float)Num, Center: 100);
		}
	}
}
