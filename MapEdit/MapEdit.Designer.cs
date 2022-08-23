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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEdit));
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Test");
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Misc", new System.Windows.Forms.TreeNode[] {
            treeNode3});
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.toolStripTree = new System.Windows.Forms.ToolStrip();
			this.btnRefreshTree = new System.Windows.Forms.ToolStripButton();
			this.btnNewTable = new System.Windows.Forms.ToolStripButton();
			this.btnDeleteTable = new System.Windows.Forms.ToolStripButton();
			this.Tree = new System.Windows.Forms.TreeView();
			this.PropertyPanel = new System.Windows.Forms.Panel();
			this.Properties = new System.Windows.Forms.PropertyGrid();
			this.GridPanel = new System.Windows.Forms.Panel();
			this.toolStripTable = new System.Windows.Forms.ToolStrip();
			this.btnTools = new System.Windows.Forms.ToolStripDropDownButton();
			this.diffByRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.diffByRowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.diffByColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnTracer = new System.Windows.Forms.ToolStripButton();
			this.btnRefreshColors = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.textBox = new System.Windows.Forms.ToolStripTextBox();
			this.btnSet = new System.Windows.Forms.ToolStripButton();
			this.btnAdd = new System.Windows.Forms.ToolStripButton();
			this.btnMul = new System.Windows.Forms.ToolStripButton();
			this.XAxisLabel = new System.Windows.Forms.Label();
			this.YAxisLabel = new RotatingLabel.RotatingLabel();
			this.Grid = new MapEditNamespace.ReoGridControlHAAAX();
			this.iconList = new System.Windows.Forms.ImageList(this.components);
			this.formTimer = new System.Windows.Forms.Timer(this.components);
			this.nodesCtrl = new NodeEditor.NodesControl();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStripTree.SuspendLayout();
			this.PropertyPanel.SuspendLayout();
			this.GridPanel.SuspendLayout();
			this.toolStripTable.SuspendLayout();
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
			this.splitContainer1.Panel1.Controls.Add(this.toolStripTree);
			this.splitContainer1.Panel1.Controls.Add(this.Tree);
			this.splitContainer1.Panel1MinSize = 300;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.nodesCtrl);
			this.splitContainer1.Panel2.Controls.Add(this.PropertyPanel);
			this.splitContainer1.Panel2.Controls.Add(this.GridPanel);
			this.splitContainer1.Size = new System.Drawing.Size(1473, 890);
			this.splitContainer1.SplitterDistance = 300;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 0;
			// 
			// toolStripTree
			// 
			this.toolStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefreshTree,
            this.btnNewTable,
            this.btnDeleteTable});
			this.toolStripTree.Location = new System.Drawing.Point(0, 0);
			this.toolStripTree.Name = "toolStripTree";
			this.toolStripTree.Size = new System.Drawing.Size(300, 25);
			this.toolStripTree.TabIndex = 1;
			this.toolStripTree.Text = "toolStrip2";
			// 
			// btnRefreshTree
			// 
			this.btnRefreshTree.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshTree.Image")));
			this.btnRefreshTree.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRefreshTree.Name = "btnRefreshTree";
			this.btnRefreshTree.Size = new System.Drawing.Size(66, 22);
			this.btnRefreshTree.Text = "Refresh";
			this.btnRefreshTree.Click += new System.EventHandler(this.btnRefreshTree_Click);
			// 
			// btnNewTable
			// 
			this.btnNewTable.Image = ((System.Drawing.Image)(resources.GetObject("btnNewTable.Image")));
			this.btnNewTable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNewTable.Name = "btnNewTable";
			this.btnNewTable.Size = new System.Drawing.Size(81, 22);
			this.btnNewTable.Text = "New Table";
			this.btnNewTable.Click += new System.EventHandler(this.btnNewTable_Click);
			// 
			// btnDeleteTable
			// 
			this.btnDeleteTable.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteTable.Image")));
			this.btnDeleteTable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDeleteTable.Name = "btnDeleteTable";
			this.btnDeleteTable.Size = new System.Drawing.Size(90, 22);
			this.btnDeleteTable.Text = "Delete Table";
			this.btnDeleteTable.Click += new System.EventHandler(this.btnDeleteTable_Click);
			// 
			// Tree
			// 
			this.Tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.Tree.Location = new System.Drawing.Point(3, 28);
			this.Tree.Name = "Tree";
			treeNode3.Name = "Test";
			treeNode3.Text = "Test";
			treeNode4.Name = "Misc";
			treeNode4.Text = "Misc";
			this.Tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
			this.Tree.Size = new System.Drawing.Size(294, 856);
			this.Tree.TabIndex = 0;
			// 
			// PropertyPanel
			// 
			this.PropertyPanel.Controls.Add(this.Properties);
			this.PropertyPanel.Location = new System.Drawing.Point(549, 15);
			this.PropertyPanel.Name = "PropertyPanel";
			this.PropertyPanel.Size = new System.Drawing.Size(483, 486);
			this.PropertyPanel.TabIndex = 2;
			// 
			// Properties
			// 
			this.Properties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Properties.Location = new System.Drawing.Point(3, 3);
			this.Properties.Name = "Properties";
			this.Properties.Size = new System.Drawing.Size(477, 480);
			this.Properties.TabIndex = 0;
			// 
			// GridPanel
			// 
			this.GridPanel.BackColor = System.Drawing.SystemColors.Control;
			this.GridPanel.Controls.Add(this.toolStripTable);
			this.GridPanel.Controls.Add(this.XAxisLabel);
			this.GridPanel.Controls.Add(this.YAxisLabel);
			this.GridPanel.Controls.Add(this.Grid);
			this.GridPanel.Location = new System.Drawing.Point(17, 15);
			this.GridPanel.Name = "GridPanel";
			this.GridPanel.Size = new System.Drawing.Size(506, 489);
			this.GridPanel.TabIndex = 1;
			// 
			// toolStripTable
			// 
			this.toolStripTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTools,
            this.btnTracer,
            this.btnRefreshColors,
            this.toolStripSeparator1,
            this.textBox,
            this.btnSet,
            this.btnAdd,
            this.btnMul});
			this.toolStripTable.Location = new System.Drawing.Point(0, 0);
			this.toolStripTable.Name = "toolStripTable";
			this.toolStripTable.Size = new System.Drawing.Size(506, 25);
			this.toolStripTable.TabIndex = 3;
			this.toolStripTable.Text = "toolStrip1";
			// 
			// btnTools
			// 
			this.btnTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diffByRowToolStripMenuItem});
			this.btnTools.Image = ((System.Drawing.Image)(resources.GetObject("btnTools.Image")));
			this.btnTools.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnTools.Name = "btnTools";
			this.btnTools.Size = new System.Drawing.Size(63, 22);
			this.btnTools.Text = "Tools";
			// 
			// diffByRowToolStripMenuItem
			// 
			this.diffByRowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diffByRowToolStripMenuItem1,
            this.diffByColumnToolStripMenuItem});
			this.diffByRowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("diffByRowToolStripMenuItem.Image")));
			this.diffByRowToolStripMenuItem.Name = "diffByRowToolStripMenuItem";
			this.diffByRowToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
			this.diffByRowToolStripMenuItem.Text = "Difference";
			// 
			// diffByRowToolStripMenuItem1
			// 
			this.diffByRowToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("diffByRowToolStripMenuItem1.Image")));
			this.diffByRowToolStripMenuItem1.Name = "diffByRowToolStripMenuItem1";
			this.diffByRowToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
			this.diffByRowToolStripMenuItem1.Text = "Diff by Row";
			this.diffByRowToolStripMenuItem1.Click += new System.EventHandler(this.diffByRowToolStripMenuItem_Click);
			// 
			// diffByColumnToolStripMenuItem
			// 
			this.diffByColumnToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("diffByColumnToolStripMenuItem.Image")));
			this.diffByColumnToolStripMenuItem.Name = "diffByColumnToolStripMenuItem";
			this.diffByColumnToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.diffByColumnToolStripMenuItem.Text = "Diff by Column";
			this.diffByColumnToolStripMenuItem.Click += new System.EventHandler(this.diffByColumnToolStripMenuItem_Click);
			// 
			// btnTracer
			// 
			this.btnTracer.CheckOnClick = true;
			this.btnTracer.Image = ((System.Drawing.Image)(resources.GetObject("btnTracer.Image")));
			this.btnTracer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnTracer.Name = "btnTracer";
			this.btnTracer.Size = new System.Drawing.Size(58, 22);
			this.btnTracer.Text = "Tracer";
			// 
			// btnRefreshColors
			// 
			this.btnRefreshColors.Checked = true;
			this.btnRefreshColors.CheckOnClick = true;
			this.btnRefreshColors.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnRefreshColors.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshColors.Image")));
			this.btnRefreshColors.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRefreshColors.Name = "btnRefreshColors";
			this.btnRefreshColors.Size = new System.Drawing.Size(103, 22);
			this.btnRefreshColors.Text = "Refresh Colors";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// textBox
			// 
			this.textBox.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(100, 25);
			this.textBox.Text = "0";
			this.textBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnSet
			// 
			this.btnSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSet.Image = ((System.Drawing.Image)(resources.GetObject("btnSet.Image")));
			this.btnSet.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSet.Name = "btnSet";
			this.btnSet.Size = new System.Drawing.Size(23, 22);
			this.btnSet.Text = "toolStripButton1";
			this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
			this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(23, 22);
			this.btnAdd.Text = "toolStripButton2";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnMul
			// 
			this.btnMul.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnMul.Image = ((System.Drawing.Image)(resources.GetObject("btnMul.Image")));
			this.btnMul.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMul.Name = "btnMul";
			this.btnMul.Size = new System.Drawing.Size(23, 22);
			this.btnMul.Text = "toolStripButton3";
			this.btnMul.Click += new System.EventHandler(this.btnMul_Click);
			// 
			// XAxisLabel
			// 
			this.XAxisLabel.AutoSize = true;
			this.XAxisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.XAxisLabel.Location = new System.Drawing.Point(69, 25);
			this.XAxisLabel.Name = "XAxisLabel";
			this.XAxisLabel.Size = new System.Drawing.Size(59, 20);
			this.XAxisLabel.TabIndex = 1;
			this.XAxisLabel.Text = "X Axis";
			// 
			// YAxisLabel
			// 
			this.YAxisLabel.AutoSize = true;
			this.YAxisLabel.BackColor = System.Drawing.SystemColors.Control;
			this.YAxisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.YAxisLabel.Location = new System.Drawing.Point(3, 103);
			this.YAxisLabel.MinimumSize = new System.Drawing.Size(20, 250);
			this.YAxisLabel.Name = "YAxisLabel";
			this.YAxisLabel.NewText = "Y Axis";
			this.YAxisLabel.RotateAngle = -90;
			this.YAxisLabel.Size = new System.Drawing.Size(20, 250);
			this.YAxisLabel.TabIndex = 2;
			// 
			// Grid
			// 
			this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Grid.BackColor = System.Drawing.Color.White;
			this.Grid.ColumnHeaderContextMenuStrip = null;
			this.Grid.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.Grid.LeadHeaderContextMenuStrip = null;
			this.Grid.Location = new System.Drawing.Point(29, 48);
			this.Grid.Name = "Grid";
			this.Grid.RowHeaderContextMenuStrip = null;
			this.Grid.Script = null;
			this.Grid.SheetTabContextMenuStrip = null;
			this.Grid.SheetTabNewButtonVisible = false;
			this.Grid.SheetTabVisible = false;
			this.Grid.SheetTabWidth = 60;
			this.Grid.ShowScrollEndSpacing = true;
			this.Grid.Size = new System.Drawing.Size(474, 438);
			this.Grid.TabIndex = 0;
			this.Grid.Text = "reoGridControl1";
			// 
			// iconList
			// 
			this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
			this.iconList.TransparentColor = System.Drawing.Color.Transparent;
			this.iconList.Images.SetKeyName(0, "table.png");
			this.iconList.Images.SetKeyName(1, "table_gear.png");
			this.iconList.Images.SetKeyName(2, "script.png");
			this.iconList.Images.SetKeyName(3, "application.png");
			this.iconList.Images.SetKeyName(4, "cog.png");
			this.iconList.Images.SetKeyName(5, "pencil.png");
			this.iconList.Images.SetKeyName(6, "page_edit.png");
			// 
			// formTimer
			// 
			this.formTimer.Interval = 50;
			// 
			// nodesCtrl
			// 
			this.nodesCtrl.Context = null;
			this.nodesCtrl.Location = new System.Drawing.Point(46, 524);
			this.nodesCtrl.Name = "nodesCtrl";
			this.nodesCtrl.Size = new System.Drawing.Size(1019, 338);
			this.nodesCtrl.TabIndex = 3;
			// 
			// MapEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1473, 890);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MapEdit";
			this.Text = "MapEdit";
			this.Load += new System.EventHandler(this.MapEdit_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.toolStripTree.ResumeLayout(false);
			this.toolStripTree.PerformLayout();
			this.PropertyPanel.ResumeLayout(false);
			this.GridPanel.ResumeLayout(false);
			this.GridPanel.PerformLayout();
			this.toolStripTable.ResumeLayout(false);
			this.toolStripTable.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.PropertyGrid Properties;
		private MapEditNamespace.ReoGridControlHAAAX Grid;
		private System.Windows.Forms.TreeView Tree;
		private System.Windows.Forms.Panel PropertyPanel;
		private System.Windows.Forms.Panel GridPanel;
		private System.Windows.Forms.Label XAxisLabel;
		private RotatingLabel.RotatingLabel YAxisLabel;
        private System.Windows.Forms.ImageList iconList;
		private System.Windows.Forms.ToolStrip toolStripTable;
		private System.Windows.Forms.ToolStripDropDownButton btnTools;
		private System.Windows.Forms.ToolStripMenuItem diffByRowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem diffByRowToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem diffByColumnToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton btnRefreshColors;
		private System.Windows.Forms.ToolStrip toolStripTree;
		private System.Windows.Forms.ToolStripButton btnRefreshTree;
		private System.Windows.Forms.ToolStripButton btnNewTable;
		private System.Windows.Forms.ToolStripButton btnDeleteTable;
		private System.Windows.Forms.ToolStripTextBox textBox;
		private System.Windows.Forms.ToolStripButton btnSet;
		private System.Windows.Forms.ToolStripButton btnAdd;
		private System.Windows.Forms.ToolStripButton btnMul;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Timer formTimer;
		private System.Windows.Forms.ToolStripButton btnTracer;
		private NodeEditor.NodesControl nodesCtrl;
	}
}