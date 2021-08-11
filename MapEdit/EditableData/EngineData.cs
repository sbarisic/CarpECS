﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;

namespace MapEdit {
	[DesignerCategory("Engine"), DisplayName("Engine data")]
	public class EngineData : EditableData {
		public EngineData() : base(EditMode.Property) {
		}

		[Description("Number of cylinders"), Category("Basic")]
		public int Cylinders { get; set; } = 1;

		[Display(Order = 1)]
		[Description("Engine displacement [cm^3]"), Category("Basic")]
		public int Displacement { get; set; } = 200;

		// Rev limiter

		[Description("Enable limiter"), Category("Rev limiter")]
		public bool EnableRevLimit { get; set; } = true;

		[Description("RPM above which the rev limiter activates [RPM]"), Category("Rev limiter")]
		public int RevLimit { get; set; } = 11000;

		[Description("RPM below which the rev limiter deactivates [RPM]"), Category("Rev limiter")]
		public int RevLimitStop { get; set; } = 10950;

		// Lambda

		[Description("Enable lambda sensor. If disabled, open loop fuel injection map is used."), Category("Lambda")]
		public bool LambdaEnabled { get; set; } = false;

		[Description("Sensor min voltage"), Category("Lambda")]
		public float LambdaStart { get; set; } = 0.1f;

		[Description("Sensor max voltage"), Category("Lambda")]
		public float LambdaEnd { get; set; } = 0.9f;

		[Description("Sensor min A/F ratio"), Category("Lambda")]
		public float LambdaRatioBottom { get; set; } = 6.0f;

		[Description("Sensor max A/F ratio"), Category("Lambda")]
		public float LambdaRatioTop { get; set; } = 17.0f;

		// Fuel injector

		[Description("Minimum allowed AFR [A/F]"), Category("Fuel injector")]
		public float MinAFR { get; set; } = 11.0f;

		[Description("Maximum allowed AFR [A/F]"), Category("Fuel injector")]
		public float MaxAFR { get; set; } = 16.0f;

		/*[Description("Calculated injector pulse range [ms]"), Category("Fuel injector")]
		public float PulseRange {
			get {
				return MaxPulseWidth - MinPulseWidth;
			}
		}*/

		[Description("Shut off fuel when coasting on 0% throttle"), Category("Fuel injector")]
		public bool FuelShutOff { get; set; } = false;
	}
}