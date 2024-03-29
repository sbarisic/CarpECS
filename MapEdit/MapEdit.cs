﻿using MapEdit.RealTime;
using MapEdit.ReoGridCtls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NodeEditor;

using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;
using unvell.ReoGrid.Drawing.Shapes;

using ReoPoint = unvell.ReoGrid.Graphics.Point;

namespace MapEdit {
	public delegate void OnCellAction(int X, int Y, ref Cell C);

	public partial class MapEdit : Form {
		ECUContext ecuContext;

		EditableData[] DataSet;
		EditableData CurrentEdited;

		ContextMenu CtxMenu;
		GridSelection Selection;

		RoundedRectangleShape Floater;
		TailShape FloaterTail;

		public MapEdit() {
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			InitializeComponent();
		}

		private void MapEdit_Load(object sender, EventArgs e) {
			ecuContext = new ECUContext();

			Properties.PropertySort = PropertySort.Categorized;

			Grid.DisableSettings(WorkbookSettings.View_ShowSheetTabControl);
			Grid.Click += GridOnClick;
			Grid.MouseClick += OnMouseClick;

			Edit(null);
			Tree.AfterSelect += TreeSelected;
			Tree.ImageList = iconList;

			btnTracer.CheckedChanged += BtnTracer_CheckedChanged;

			formTimer.Tick += FormTimer_Tick;
			formTimer.Start();
		}

		private void BtnTracer_CheckedChanged(object sender, EventArgs e) {
			Worksheet WSheet = Grid.CurrentWorksheet;

			if (WSheet == null)
				return;

			if (btnTracer.Checked) {
				SpawnFloater(WSheet);
			} else {
				Floater = null;
				FloaterTail = null;
				WSheet.FloatingObjects.Clear();
			}
		}

		private void FormTimer_Tick(object sender, EventArgs e) {
			if (!GridPanel.Visible)
				return;

			if (!btnTracer.Checked)
				return;

			Worksheet WSheet = Grid.CurrentWorksheet;

			if (WSheet == null)
				return;

			SpawnFloater(WSheet);

			int RPM = (int)Program.ECUMonitor.GetRPM();
			double YData = Program.ECUMonitor.GetSampleData(11, 28, 1.55);

			Console.WriteLine(YData);

			ReoPoint Pt = CalculatePhysicalPos(RPM, YData);
			FloaterTail.PushPoint(Pt);

			Pt.X -= 4;
			Pt.Y -= 5;
			Floater.Location = Pt;
			Floater.Invalidate();
		}

		ReoPoint CalculatePhysicalPos(double XVal, double YVal) {
			CurrentEdited.Table.IndexData(XVal, YVal, out int Col, out int Row, out float OffX, out float OffY);

			GetCellPos(Col, Row, out float X, out float Y, out float W, out float H);

			if (OffX > 0) {
				GetCellPos(Col + 1, Row, out float X2, out float Y2, out float W2, out float H2);
				X = Utils.Lerp(X, X2, OffX);
			}

			if (OffY > 0) {
				GetCellPos(Col, Row + 1, out float X2, out float Y2, out float W2, out float H2);
				Y = Utils.Lerp(Y, Y2, OffY);
			}

			return new ReoPoint(X + W / 2, Y + H / 2);
		}

		bool GetCellPos(int Col, int Row, out float X, out float Y, out float W, out float H) {
			Worksheet WSheet = Grid.CurrentWorksheet;

			if (Col < 0 || Col >= WSheet.ColumnCount || Row < 0 || Row >= WSheet.RowCount) {
				X = Y = W = H = 0;
				return false;
			}

			ReoPoint Pt = WSheet.GetCellPhysicsPosition(Row, Col);

			X = Pt.X;
			Y = Pt.Y;
			H = WSheet.GetRowHeight(Row);
			W = WSheet.GetColumnWidth(Col);
			return true;
		}

		void SpawnFloater(Worksheet WSheet) {
			if (Floater == null) {
				Floater = new RoundedRectangleShape();
				Floater.RoundRate = 1;
				Floater.Size = new unvell.ReoGrid.Graphics.Size(10, 10);
				Floater.Style.FillColor = SolidColor.Red;

				WSheet.FloatingObjects.Add(Floater);
			}

			if (FloaterTail == null) {
				FloaterTail = new TailShape();
				WSheet.FloatingObjects.Add(FloaterTail);
			}
		}

		bool CalculateSelection() {
			Worksheet Sheet = Grid.CurrentWorksheet;
			RangePosition CurRange = Sheet.SelectionRange;

			if (Selection == null)
				Selection = new GridSelection();

			Selection.Sheet = Sheet;
			Selection.A = Sheet.Cells[CurRange.StartPos];
			Selection.D = Sheet.Cells[CurRange.EndPos];
			Selection.B = Sheet.Cells[Selection.A.Row, Selection.D.Column];
			Selection.C = Sheet.Cells[Selection.D.Row, Selection.A.Column];

			if (Selection == null)
				return false;

			return true;
		}

		private void OnMouseClick(object S, MouseEventArgs E) {
			if (E.Button == MouseButtons.Right) {
				CalculateSelection();

				if (CtxMenu == null) {
					CtxMenu = new ContextMenu();

					CtxMenu.MenuItems.Add(new MenuItem("Interpolate", (SS, EE) => Interpolate(Selection)));
					CtxMenu.MenuItems.Add(new MenuItem("Diff by Row", (SS, EE) => diffByRowToolStripMenuItem_Click(SS, EE)));
					CtxMenu.MenuItems.Add(new MenuItem("Diff by Col", (SS, EE) => diffByColumnToolStripMenuItem_Click(SS, EE)));

					CtxMenu.MenuItems.Add("-");

					CtxMenu.MenuItems.Add(new MenuItem("Set", (SS, EE) => btnSet_Click(SS, EE)));
					CtxMenu.MenuItems.Add(new MenuItem("Add", (SS, EE) => btnAdd_Click(SS, EE)));
					CtxMenu.MenuItems.Add(new MenuItem("Mul", (SS, EE) => btnMul_Click(SS, EE)));
				}

				CtxMenu.Show(Grid, Grid.PointToClient(MousePosition));
			}
		}

		private void GridOnClick(object Sender, EventArgs E) {
			if (btnRefreshColors.Checked) {
				ColorSheet(Grid.CurrentWorksheet);
			}
		}

		void Interpolate(GridSelection Sel) {
			Cell A = Sel.A, B = Sel.B, C = Sel.C, D = Sel.D;

			for (int Y = Sel.Y1; Y <= Sel.Y2; Y++)
				for (int X = Sel.X1; X <= Sel.X2; X++) {
					double Data;

					if (Sel.Width == 1)
						Data = Utils.Lerp((double)A.Data, (double)D.Data, Sel.Y1, Sel.Y2, Y);
					else if (Sel.Height == 1)
						Data = Utils.Lerp((double)A.Data, (double)B.Data, Sel.X1, Sel.X2, X);
					else
						Data = Utils.Bilinear((double)A.Data, (double)B.Data, (double)C.Data, (double)D.Data, Sel.X1, Sel.X2, Sel.Y1, Sel.Y2, X, Y);

					Sel.Sheet.Cells[Y, X].Data = Math.Round(Data, 2);
				}

			ColorSheet(Sel.Sheet);
		}

		void ForEachCell(Worksheet Sheet, OnCellAction OnCell) {
			if (Sheet == null)
				return;

			for (int X = 0; X < Sheet.ColumnCount; X++)
				for (int Y = 0; Y < Sheet.RowCount; Y++) {
					Cell CurCell = Sheet.Cells[Y, X];
					OnCell(X, Y, ref CurCell);
				}
		}

		void ForEachSelectedCell(Worksheet Sheet, RangePosition Rng, OnCellAction OnCell) {
			ReferenceRange RefRng = Sheet.Ranges[Rng];

			foreach (Cell C in RefRng.Cells) {
				Cell CurCell = C;
				OnCell(CurCell.Column, CurCell.Row, ref CurCell);
			}
		}

		void ColorSheet(Worksheet Sheet) {
			ForEachCell(Sheet, (int X, int Y, ref Cell C) => {
				CurrentEdited.ColorCell(X, Y, C.Data, ref C);
			});

			Grid.Refresh();
		}

		public void SetEditable(EditableData[] DataSet) {
			this.DataSet = DataSet;
			Tree.Nodes.Clear();

			foreach (EditableData ED in DataSet) {
				Type EDType = ED.GetType();

				DesignerCategoryAttribute Cat = EDType.GetCustomAttribute<DesignerCategoryAttribute>();
				DisplayNameAttribute DispName = EDType.GetCustomAttribute<DisplayNameAttribute>() ?? new DisplayNameAttribute(ED.Name ?? EDType.Name);

				TreeNode CatNode = FindOrCreateCategory(Cat.Category);
				CatNode.Tag = null;

				TreeNode MainNode = CreateNode(CatNode, DispName.DisplayName, ED);
				if (ED.DataProperties != null) {
					CreateNode(MainNode, DispName.DisplayName + " Props", ED.DataProperties);
				}

				MainNode.Collapse();
				CatNode.Expand();
			}

			// Tree.ExpandAll();
		}

		public void AddEditable(EditableData NewData) {
			List<EditableData> AllNewData = new List<EditableData>();
			AllNewData.AddRange(DataSet);
			AllNewData.Add(NewData);

			SetEditable(AllNewData.ToArray());
		}

		TreeNode CreateNode(TreeNode RootNode, string DisplayName, EditableData Data) {
			TreeNode EditNode = RootNode.Nodes.Add(DisplayName);
			EditNode.Tag = Data;
			EditNode.ImageIndex = (int)Data.Icon;
			EditNode.SelectedImageIndex = (int)Data.Icon;
			return EditNode;
		}

		TreeNode FindOrCreateCategory(string Name) {
			foreach (TreeNode N in Tree.Nodes)
				if (N.Text == Name)
					return N;

			TreeNode CatNode = Tree.Nodes.Add(Name);

			CatNode.ImageIndex = (int)ImageIndex.CATEGORY;
			CatNode.SelectedImageIndex = (int)ImageIndex.CATEGORY;
			return CatNode;
		}

		void TreeSelected(object Sender, TreeViewEventArgs E) {
			if (E.Node.Tag is EditableData ED)
				Edit(ED);
		}

		void Edit(EditableData Data) {
			CurrentEdited = Data;
			GridPanel.Dock = DockStyle.Fill;
			GridPanel.Visible = false;

			PropertyPanel.Dock = DockStyle.Fill;
			PropertyPanel.Visible = false;

			nodesCtrl.Dock = DockStyle.Fill;
			nodesCtrl.Visible = false;

			if (Data == null)
				return;

			if (!Data.DataEnabled)
				return;

			Grid.CurrentData = Data;

			switch (Data.EditMode) {
				case EditMode.Grid: {
						XAxisLabel.Text = Data.XAxisName;
						YAxisLabel.NewText = Data.YAxisName;

						GridPanel.Visible = true;
						Grid.Worksheets.Clear();

						if (Data.Worksheet == null) {
							Worksheet WSheet = Grid.Worksheets.Create(string.Format("{0} / {1}", Data.XAxisName, Data.YAxisName));
							Data.Worksheet = WSheet;



							WSheet.DisableSettings(WorksheetSettings.Edit_AllowAdjustColumnWidth | WorksheetSettings.Edit_AllowAdjustRowHeight);
							WSheet.DisableSettings(WorksheetSettings.Edit_DragSelectionToMoveCells);

							WSheet.RowCount = 1;
							WSheet.ColumnCount = 1;

							Data.PopulateSheet(WSheet);
							// Interpolate(new GridSelection(WSheet));
						}

						Grid.CurrentWorksheet = Data.Worksheet;
						ColorSheet(Data.Worksheet);
						StylizeSheet(Data.Worksheet);
						break;
					}

				case EditMode.Property: {
						PropertyPanel.Visible = true;
						Properties.SelectedObject = Data;
						break;
					}


				case EditMode.Nodes: {
						nodesCtrl.Visible = true;
						nodesCtrl.Context = ecuContext;


						break;
					}

				default:
					throw new Exception("Invalid edit mode " + Data.EditMode);
			}
		}

		void StylizeSheet(Worksheet Sheet) {
			RangePosition Rng = new RangePosition(0, 0, Sheet.RowCount, Sheet.ColumnCount);

			WorksheetRangeStyle Style = Sheet.GetRangeStyles(Rng);
			Style.Flag = PlainStyleFlag.FontSize | PlainStyleFlag.FontName;
			Style.FontName = "Consolas";
			Style.FontSize = 9;

			Sheet.SetRangeStyles(Rng, Style);
		}

		private void diffByRowToolStripMenuItem_Click(object sender, EventArgs e) {
			if (CalculateSelection() && Selection.Sheet != null) {
				Worksheet Sheet = Selection.Sheet;

				//Sheet.Cells[Selection.A.Position].Style.BackColor = Color.YellowGreen;

				int ARow = Selection.A.Position.Row;
				int ACol = Selection.A.Position.Col;
				int DRow = Selection.D.Position.Row;
				int DCol = Selection.D.Position.Col;

				int Height = DRow - ARow;
				//int Width = DCol - ACol;

				if (ARow == DRow)
					return;

				for (int Column = ACol; Column <= DCol; Column++) {
					bool RowsSame = true;

					for (int Row = 1; Row <= Height; Row++) {
						object PrevRow = Sheet[ARow + Row - 1, Column];
						object CurRow = Sheet[ARow + Row, Column];

						if (!PrevRow.Equals(CurRow)) {
							RowsSame = false;
							break;
						}
					}

					for (int Row = 0; Row <= Height; Row++) {
						Color FillColor = Color.GreenYellow;

						if (!RowsSame) {
							FillColor = Color.Orange;
						}

						Sheet.Cells[ARow + Row, Column].Style.BackColor = FillColor;
					}

					Grid.Refresh();
				}
			}
		}

		private void diffByColumnToolStripMenuItem_Click(object sender, EventArgs e) {
			if (CalculateSelection() && Selection.Sheet != null) {
				Worksheet Sheet = Selection.Sheet;

				//Sheet.Cells[Selection.A.Position].Style.BackColor = Color.YellowGreen;

				int ARow = Selection.A.Position.Row;
				int ACol = Selection.A.Position.Col;
				int DRow = Selection.D.Position.Row;
				int DCol = Selection.D.Position.Col;

				//int Height = DRow - ARow;
				int Width = DCol - ACol;

				if (ARow == DRow)
					return;

				for (int Row = ARow; Row <= DRow; Row++) {
					bool ColsSame = true;

					for (int Column = 1; Column <= Width; Column++) {
						object PrevCol = Sheet[Row, Column + ACol - 1];
						object CurCol = Sheet[Row, Column + ACol];

						if (!PrevCol.Equals(CurCol)) {
							ColsSame = false;
							break;
						}
					}

					for (int Column = 0; Column <= Width; Column++) {
						Color FillColor = Color.GreenYellow;

						if (!ColsSame) {
							FillColor = Color.Orange;
						}

						Sheet.Cells[Row, Column + ACol].Style.BackColor = FillColor;
					}

					Grid.Refresh();
				}
			}
		}

		private void btnRefreshTree_Click(object sender, EventArgs e) {
			SetEditable(DataSet);
		}

		private void btnNewTable_Click(object sender, EventArgs e) {
			TreeNode Selected = Tree.SelectedNode;

			if (Selected == null)
				return;

			if (Selected.ImageIndex != (int)ImageIndex.CATEGORY)
				return;

			//
			try {
				EditableData NewTable = Utils.ParseTableFromClipboard(out LookupTable2D LookupTable);
				AddEditable(NewTable);
			} catch (Exception) {
			}
		}

		private void btnDeleteTable_Click(object sender, EventArgs e) {
			TreeNode Selected = Tree.SelectedNode;

			if (Selected == null)
				return;

			if (Selected.ImageIndex != (int)ImageIndex.TABLE)
				return;

			SetEditable(DataSet.Where(ED => ED != Selected.Tag).ToArray());
		}

		private void btnSet_Click(object sender, EventArgs e) {
			if (Utils.TryParseDouble(textBox.Text, out double F)) {
				if (CalculateSelection() && Selection.Sheet != null) {
					Worksheet Sheet = Selection.Sheet;

					ForEachSelectedCell(Sheet, Selection.ToRange(), (int X, int Y, ref Cell C) => {
						C.Data = F;
					});
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			if (Utils.TryParseDouble(textBox.Text, out double F)) {
				if (CalculateSelection() && Selection.Sheet != null) {
					Worksheet Sheet = Selection.Sheet;

					ForEachSelectedCell(Sheet, Selection.ToRange(), (int X, int Y, ref Cell C) => {
						C.Data = (double)C.Data + F;
					});
				}
			}
		}

		private void btnMul_Click(object sender, EventArgs e) {
			if (Utils.TryParseDouble(textBox.Text, out double F)) {
				if (CalculateSelection() && Selection.Sheet != null) {
					Worksheet Sheet = Selection.Sheet;

					ForEachSelectedCell(Sheet, Selection.ToRange(), (int X, int Y, ref Cell C) => {
						C.Data = (double)C.Data * F;
					});
				}
			}
		}

		private void compileBtn_Click(object sender, EventArgs e) {
			if (CurrentEdited == null)
				return;

			switch (CurrentEdited.EditMode) {
				case EditMode.Grid:
					break;

				case EditMode.Property:
					break;

				case EditMode.Nodes: {
						nodesCtrl.Execute();


						
						break;
					}

				default:
					break;
			}
		}

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e) {

        }
    }
}