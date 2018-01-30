using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;

namespace Mapper {
	public partial class MapperForm : Form {
		const int MaxRPM = 20000;
		const int RPMMapRange = 500;
		const int MinRPM = 500;
		const int MaxLimiterDiff = 1000;
		const int ThrottleDiv = 10;

		int RevLimit_TurnOn;
		int RevLimit_TurnOff;
		int LaunchControl_BelowSpeed;

		public MapperForm() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			HardRevLimitNum.Minimum = MinRPM + (int)(MinRPM * 0.1);
			HardRevLimitNum.Maximum = MaxRPM;
			HardRevLimitNum.ValueChanged += (S, E) => UpdateDataSettings();
			HardRevLimitNum.Value = MaxRPM;

			HardRevLimitOffNum.Minimum = 0;
			HardRevLimitOffNum.Maximum = MaxLimiterDiff;
			HardRevLimitOffNum.ValueChanged += (S, E) => UpdateDataSettings();
			HardRevLimitOffNum.Value = 10;

			HardRevLimitEnabled.CheckedChanged += (S, E) => UpdateDataSettings();
			LaunchControlEnabled.CheckedChanged += (S, E) => UpdateDataSettings();

			LaunchControlMinSpeed.Minimum = 0;
			LaunchControlMinSpeed.Maximum = 200;
			LaunchControlMinSpeed.Value = 5;
			LaunchControlMinSpeed.ValueChanged += (S, E) => UpdateDataSettings();

			ReoGrid.SheetTabWidth = 250;
			ReoGrid.CurrentWorksheetChanged += OnWorksheetChanged;

			Worksheet WS = ReoGrid.CurrentWorksheet;
			WS.Name = "RPM/Throttle Advance Map";

			int Cols = (MaxRPM - MinRPM) / RPMMapRange;
			WS.ColumnCount = Cols;

			int Rows = 100 / ThrottleDiv;
			WS.RowCount = Rows;

			for (int i = 0; i < Cols; i++) {
				WS.ColumnHeaders[i].Text = string.Format("{0} RPM", MinRPM + i * RPMMapRange);
			}

			for (int i = 0; i < Rows; i++) {
				WS.RowHeaders[i].Text = string.Format("{0} %", i * ThrottleDiv);
				WS.RowHeaders[i].Height *= 2;
			}

			for (int y = 0; y < Rows; y++)
				for (int x = 0; x < Cols; x++) {
					Cell C = WS.Cells[y, x];
					C.Style.HAlign = ReoGridHorAlign.Center;
					C.Style.VAlign = ReoGridVerAlign.Middle;
					C.DataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Number;

					C.Data = 0;
				}

			UpdateDataSettings();
		}

		void UpdateDataSettings() {
			RevLimit_TurnOn = RevLimit_TurnOff = -1;

			if (HardRevLimitEnabled.Checked) {
				RevLimit_TurnOff = (int)HardRevLimitNum.Value - (int)HardRevLimitOffNum.Value;
				RevLimit_TurnOn = (int)HardRevLimitNum.Value;

				if (RevLimit_TurnOff < 1) {
					HardRevLimitOffNum.Value = RevLimit_TurnOff + RevLimit_TurnOn - 1;
					UpdateDataSettings();
					return;
				}

				RevLimitLabel.Text = string.Format("Rev Limit: {0} RPM / {1} RPM", RevLimit_TurnOn, RevLimit_TurnOff);
			} else
				RevLimitLabel.Text = "Rev Limit Off";

			if (LaunchControlEnabled.Checked) {
				LaunchControl_BelowSpeed = (int)LaunchControlMinSpeed.Value;

				LaunchControlLabel.Text = string.Format("Launch Control < {0} km/h", LaunchControl_BelowSpeed);
			} else
				LaunchControlLabel.Text = "Launch Control Disabled";
		}

		void OnWorksheetChanged(object S, EventArgs E) {
			StatusLabel.Text = ReoGrid.CurrentWorksheet.Name;
		}
	}
}
