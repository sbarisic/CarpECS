namespace Mapper {
	partial class MapperForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.ReoGrid = new unvell.ReoGrid.ReoGridControl();
			this.HardRevLimitNum = new System.Windows.Forms.NumericUpDown();
			this.HardRevLimitEnabled = new System.Windows.Forms.CheckBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.LaunchControlMinSpeed = new System.Windows.Forms.NumericUpDown();
			this.LaunchControlEnabled = new System.Windows.Forms.CheckBox();
			this.HardRevLimitOffNum = new System.Windows.Forms.NumericUpDown();
			this.RevLimitLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.Sep1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.Sep2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.LaunchControlLabel = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.HardRevLimitNum)).BeginInit();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LaunchControlMinSpeed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.HardRevLimitOffNum)).BeginInit();
			this.SuspendLayout();
			// 
			// ReoGrid
			// 
			this.ReoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ReoGrid.BackColor = System.Drawing.Color.White;
			this.ReoGrid.ColumnHeaderContextMenuStrip = null;
			this.ReoGrid.LeadHeaderContextMenuStrip = null;
			this.ReoGrid.Location = new System.Drawing.Point(12, 68);
			this.ReoGrid.Name = "ReoGrid";
			this.ReoGrid.RowHeaderContextMenuStrip = null;
			this.ReoGrid.Script = null;
			this.ReoGrid.SheetTabContextMenuStrip = null;
			this.ReoGrid.SheetTabNewButtonVisible = true;
			this.ReoGrid.SheetTabVisible = true;
			this.ReoGrid.SheetTabWidth = 60;
			this.ReoGrid.ShowScrollEndSpacing = true;
			this.ReoGrid.Size = new System.Drawing.Size(913, 541);
			this.ReoGrid.TabIndex = 0;
			this.ReoGrid.Text = "reoGridControl1";
			// 
			// HardRevLimitNum
			// 
			this.HardRevLimitNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.HardRevLimitNum.Location = new System.Drawing.Point(719, 12);
			this.HardRevLimitNum.Name = "HardRevLimitNum";
			this.HardRevLimitNum.Size = new System.Drawing.Size(100, 22);
			this.HardRevLimitNum.TabIndex = 1;
			// 
			// HardRevLimitEnabled
			// 
			this.HardRevLimitEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.HardRevLimitEnabled.AutoSize = true;
			this.HardRevLimitEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.HardRevLimitEnabled.Checked = true;
			this.HardRevLimitEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HardRevLimitEnabled.Location = new System.Drawing.Point(548, 13);
			this.HardRevLimitEnabled.Name = "HardRevLimitEnabled";
			this.HardRevLimitEnabled.Size = new System.Drawing.Size(165, 21);
			this.HardRevLimitEnabled.TabIndex = 2;
			this.HardRevLimitEnabled.Text = "Hard Rev Limit [RPM]";
			this.HardRevLimitEnabled.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.Sep1,
            this.RevLimitLabel,
            this.Sep2,
            this.LaunchControlLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 626);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(937, 25);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// StatusLabel
			// 
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size(45, 20);
			this.StatusLabel.Text = "Done";
			// 
			// LaunchControlMinSpeed
			// 
			this.LaunchControlMinSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LaunchControlMinSpeed.Location = new System.Drawing.Point(825, 40);
			this.LaunchControlMinSpeed.Name = "LaunchControlMinSpeed";
			this.LaunchControlMinSpeed.Size = new System.Drawing.Size(100, 22);
			this.LaunchControlMinSpeed.TabIndex = 4;
			// 
			// LaunchControlEnabled
			// 
			this.LaunchControlEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LaunchControlEnabled.AutoSize = true;
			this.LaunchControlEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.LaunchControlEnabled.Checked = true;
			this.LaunchControlEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.LaunchControlEnabled.Location = new System.Drawing.Point(584, 41);
			this.LaunchControlEnabled.Name = "LaunchControlEnabled";
			this.LaunchControlEnabled.Size = new System.Drawing.Size(235, 21);
			this.LaunchControlEnabled.TabIndex = 5;
			this.LaunchControlEnabled.Text = "Launch control min speed [km/h]";
			this.LaunchControlEnabled.UseVisualStyleBackColor = true;
			// 
			// HardRevLimitOffNum
			// 
			this.HardRevLimitOffNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.HardRevLimitOffNum.Location = new System.Drawing.Point(825, 12);
			this.HardRevLimitOffNum.Name = "HardRevLimitOffNum";
			this.HardRevLimitOffNum.Size = new System.Drawing.Size(100, 22);
			this.HardRevLimitOffNum.TabIndex = 6;
			// 
			// RevLimitLabel
			// 
			this.RevLimitLabel.Name = "RevLimitLabel";
			this.RevLimitLabel.Size = new System.Drawing.Size(66, 20);
			this.RevLimitLabel.Text = "RevLimit";
			// 
			// Sep1
			// 
			this.Sep1.Name = "Sep1";
			this.Sep1.Size = new System.Drawing.Size(13, 20);
			this.Sep1.Text = "|";
			// 
			// Sep2
			// 
			this.Sep2.Name = "Sep2";
			this.Sep2.Size = new System.Drawing.Size(13, 20);
			this.Sep2.Text = "|";
			// 
			// LaunchControlLabel
			// 
			this.LaunchControlLabel.Name = "LaunchControlLabel";
			this.LaunchControlLabel.Size = new System.Drawing.Size(104, 20);
			this.LaunchControlLabel.Text = "LaunchControl";
			// 
			// MapperForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(937, 651);
			this.Controls.Add(this.HardRevLimitOffNum);
			this.Controls.Add(this.LaunchControlEnabled);
			this.Controls.Add(this.LaunchControlMinSpeed);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.HardRevLimitEnabled);
			this.Controls.Add(this.HardRevLimitNum);
			this.Controls.Add(this.ReoGrid);
			this.Name = "MapperForm";
			this.Text = "CarpECU Mapper";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.HardRevLimitNum)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.LaunchControlMinSpeed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.HardRevLimitOffNum)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private unvell.ReoGrid.ReoGridControl ReoGrid;
		private System.Windows.Forms.NumericUpDown HardRevLimitNum;
		private System.Windows.Forms.CheckBox HardRevLimitEnabled;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
		private System.Windows.Forms.NumericUpDown LaunchControlMinSpeed;
		private System.Windows.Forms.CheckBox LaunchControlEnabled;
		private System.Windows.Forms.NumericUpDown HardRevLimitOffNum;
		private System.Windows.Forms.ToolStripStatusLabel RevLimitLabel;
		private System.Windows.Forms.ToolStripStatusLabel Sep1;
		private System.Windows.Forms.ToolStripStatusLabel Sep2;
		private System.Windows.Forms.ToolStripStatusLabel LaunchControlLabel;
	}
}

