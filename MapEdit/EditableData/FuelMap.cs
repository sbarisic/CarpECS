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

		public FuelMap(EngineData EngineData) : base(EditMode.Grid, AxisParameters.RPM, AxisParameters.EngineLoad) {
			this.EngineData = EngineData;

			//XName = "Engine speed [RPM]";
			//YName = "Engine load [%]";

			ValueName = "Target A/F";
			DefaultValue = 10.0;

			TopLeft = 14;
			BottomLeft = 14;
			TopRight = 11;
			BottomRight = 11;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(new SolidColor(104, 162, 255), SolidColor.Green, new SolidColor(255, 0, 0), EngineData.MinAFR, EngineData.MaxAFR, (float)Num);
		}
	}
}
