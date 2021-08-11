namespace MapEdit {
	partial class MapEdit {
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
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Test");
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Misc", new System.Windows.Forms.TreeNode[] {
            treeNode3});
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.Tree = new System.Windows.Forms.TreeView();
			this.PropertyPanel = new System.Windows.Forms.Panel();
			this.Properties = new System.Windows.Forms.PropertyGrid();
			this.GridPanel = new System.Windows.Forms.Panel();
			this.XAxisLabel = new System.Windows.Forms.Label();
			this.Grid = new unvell.ReoGrid.ReoGridControl();
			this.YAxisLabel = new RotatingLabel.RotatingLabel();
			this.ValueLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.PropertyPanel.SuspendLayout();
			this.GridPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.Tree);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.PropertyPanel);
			this.splitContainer1.Panel2.Controls.Add(this.GridPanel);
			this.splitContainer1.Size = new System.Drawing.Size(1526, 786);
			this.splitContainer1.SplitterDistance = 450;
			this.splitContainer1.TabIndex = 0;
			// 
			// Tree
			// 
			this.Tree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.Tree.Location = new System.Drawing.Point(0, 0);
			this.Tree.Name = "Tree";
			treeNode3.Name = "Test";
			treeNode3.Text = "Test";
			treeNode4.Name = "Misc";
			treeNode4.Text = "Misc";
			this.Tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
			this.Tree.Size = new System.Drawing.Size(450, 786);
			this.Tree.TabIndex = 0;
			// 
			// PropertyPanel
			// 
			this.PropertyPanel.Controls.Add(this.Properties);
			this.PropertyPanel.Location = new System.Drawing.Point(562, 3);
			this.PropertyPanel.Name = "PropertyPanel";
			this.PropertyPanel.Size = new System.Drawing.Size(507, 780);
			this.PropertyPanel.TabIndex = 2;
			// 
			// Properties
			// 
			this.Properties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Properties.Location = new System.Drawing.Point(3, 3);
			this.Properties.Name = "Properties";
			this.Properties.Size = new System.Drawing.Size(501, 774);
			this.Properties.TabIndex = 0;
			// 
			// GridPanel
			// 
			this.GridPanel.BackColor = System.Drawing.SystemColors.Control;
			this.GridPanel.Controls.Add(this.ValueLabel);
			this.GridPanel.Controls.Add(this.XAxisLabel);
			this.GridPanel.Controls.Add(this.YAxisLabel);
			this.GridPanel.Controls.Add(this.Grid);
			this.GridPanel.Location = new System.Drawing.Point(3, 3);
			this.GridPanel.Name = "GridPanel";
			this.GridPanel.Size = new System.Drawing.Size(553, 780);
			this.GridPanel.TabIndex = 1;
			// 
			// XAxisLabel
			// 
			this.XAxisLabel.AutoSize = true;
			this.XAxisLabel.Location = new System.Drawing.Point(68, 24);
			this.XAxisLabel.Name = "XAxisLabel";
			this.XAxisLabel.Size = new System.Drawing.Size(36, 13);
			this.XAxisLabel.TabIndex = 1;
			this.XAxisLabel.Text = "X Axis";
			// 
			// Grid
			// 
			this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Grid.BackColor = System.Drawing.Color.White;
			this.Grid.ColumnHeaderContextMenuStrip = null;
			this.Grid.LeadHeaderContextMenuStrip = null;
			this.Grid.Location = new System.Drawing.Point(21, 40);
			this.Grid.Name = "Grid";
			this.Grid.RowHeaderContextMenuStrip = null;
			this.Grid.Script = null;
			this.Grid.SheetTabContextMenuStrip = null;
			this.Grid.SheetTabNewButtonVisible = true;
			this.Grid.SheetTabVisible = true;
			this.Grid.SheetTabWidth = 60;
			this.Grid.ShowScrollEndSpacing = true;
			this.Grid.Size = new System.Drawing.Size(529, 737);
			this.Grid.TabIndex = 0;
			this.Grid.Text = "reoGridControl1";
			// 
			// YAxisLabel
			// 
			this.YAxisLabel.AutoSize = true;
			this.YAxisLabel.BackColor = System.Drawing.SystemColors.Control;
			this.YAxisLabel.Location = new System.Drawing.Point(3, 73);
			this.YAxisLabel.MinimumSize = new System.Drawing.Size(12, 250);
			this.YAxisLabel.Name = "YAxisLabel";
			this.YAxisLabel.NewText = "Y Axis";
			this.YAxisLabel.RotateAngle = -90;
			this.YAxisLabel.Size = new System.Drawing.Size(12, 250);
			this.YAxisLabel.TabIndex = 2;
			// 
			// ValueLabel
			// 
			this.ValueLabel.AutoSize = true;
			this.ValueLabel.Location = new System.Drawing.Point(18, 3);
			this.ValueLabel.Name = "ValueLabel";
			this.ValueLabel.Size = new System.Drawing.Size(34, 13);
			this.ValueLabel.TabIndex = 3;
			this.ValueLabel.Text = "Value";
			// 
			// MapEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1526, 786);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MapEdit";
			this.Text = "MapEdit";
			this.Load += new System.EventHandler(this.MapEdit_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.PropertyPanel.ResumeLayout(false);
			this.GridPanel.ResumeLayout(false);
			this.GridPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.PropertyGrid Properties;
		private unvell.ReoGrid.ReoGridControl Grid;
		private System.Windows.Forms.TreeView Tree;
		private System.Windows.Forms.Panel PropertyPanel;
		private System.Windows.Forms.Panel GridPanel;
		private System.Windows.Forms.Label XAxisLabel;
		private RotatingLabel.RotatingLabel YAxisLabel;
		private System.Windows.Forms.Label ValueLabel;
	}
}