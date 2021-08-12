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

	[DesignerCategory("Maps"), DisplayName("Volumetric Efficiency Map")]
	public class VolumetricEfficiencyMap : EditableData {
		EngineData EngineData;

		public override bool DataEnabled => !EngineData.LambdaEnabled;

		public VolumetricEfficiencyMap(EngineData EngineData) : base(EditMode.Grid, AxisParameters.RPM, AxisParameters.MAP) {
			this.EngineData = EngineData;

			//XName = "Engine speed [RPM]";
			//YName = "Engine load [%]";

			ValueName = "Volumetric Efficiency [%]";
			DefaultValue = 10.0;

			TopLeft = 90;
			BottomLeft = 20;
			TopRight = 95;
			BottomRight = 50;
		}

		public override void ColorCell(int X, int Y, object Value, ref Cell C) {
			C.Style.BackColor = SolidColor.Transparent;

			if (Value is double Num)
				C.Style.BackColor = Utils.Lerp(new SolidColor(255, 0, 0), SolidColor.Green, new SolidColor(104, 162, 255), EngineData.MinVE, EngineData.MaxVE, (float)Num);
		}
	}
}
