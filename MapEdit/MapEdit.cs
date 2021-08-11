using MapEdit.RealTime;

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

using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;

namespace MapEdit {
	public partial class MapEdit : Form {
		EditableData CurrentEdited;
		ContextMenu CtxMenu;
		GridSelection Selection;

		public MapEdit() {
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			InitializeComponent();
		}

		private void MapEdit_Load(object sender, EventArgs e) {
			Properties.PropertySort = PropertySort.Categorized;

			Grid.DisableSettings(WorkbookSettings.View_ShowSheetTabControl);
			Grid.Click += GridOnClick;
			Grid.MouseClick += OnMouseClick;

			Edit(null);
			Tree.AfterSelect += TreeSelected;

			UpdateTimer.Tick += UpdateTimer_Tick;
			UpdateTimer.Start();
		}

		private void UpdateTimer_Tick(object sender, EventArgs e) {
			Grid.UpdateTracer();
			Grid.Invalidate();
		}

		private void OnMouseClick(object S, MouseEventArgs E) {
			if (E.Button == MouseButtons.Right) {
				Worksheet Sheet = Grid.CurrentWorksheet;
				RangePosition CurRange = Sheet.SelectionRange;

				if (CtxMenu == null) {
					CtxMenu = new ContextMenu();
					CtxMenu.MenuItems.Add(new MenuItem("Interpolate", (SS, EE) => Interpolate(Selection)));
				}

				if (Selection == null)
					Selection = new GridSelection();

				Selection.Sheet = Sheet;
				Selection.A = Sheet.Cells[CurRange.StartPos];
				Selection.D = Sheet.Cells[CurRange.EndPos];
				Selection.B = Sheet.Cells[Selection.A.Row, Selection.D.Column];
				Selection.C = Sheet.Cells[Selection.D.Row, Selection.A.Column];

				CtxMenu.Show(Grid, Grid.PointToClient(MousePosition));
			}
		}

		private void GridOnClick(object Sender, EventArgs E) {
			ColorSheet(Grid.CurrentWorksheet);
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

		void ColorSheet(Worksheet Sheet) {
			if (Sheet == null)
				return;

			for (int X = 0; X < Sheet.ColumnCount; X++)
				for (int Y = 0; Y < Sheet.RowCount; Y++) {
					Cell CurCell = Sheet.Cells[Y, X];
					CurrentEdited.ColorCell(X, Y, CurCell.Data, ref CurCell);
				}
		}

		public void SetEditable(EditableData[] DataSet) {
			Tree.Nodes.Clear();

			foreach (EditableData ED in DataSet) {
				Type EDType = ED.GetType();

				DesignerCategoryAttribute Cat = EDType.GetCustomAttribute<DesignerCategoryAttribute>();
				DisplayNameAttribute DispName = EDType.GetCustomAttribute<DisplayNameAttribute>() ?? new DisplayNameAttribute(EDType.Name);

				TreeNode CatNode = FindOrCreateCategory(Cat.Category);
				CatNode.Tag = null;

				TreeNode EditNode = CatNode.Nodes.Add(DispName.DisplayName);
				EditNode.Tag = ED;
			}

			Tree.ExpandAll();
		}

		TreeNode FindOrCreateCategory(string Name) {
			foreach (TreeNode N in Tree.Nodes)
				if (N.Text == Name)
					return N;

			return Tree.Nodes.Add(Name);
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

			if (Data == null)
				return;

			if (!Data.DataEnabled)
				return;

			Grid.CurrentData = Data;

			switch (Data.EditMode) {
				case EditMode.Grid: {
						XAxisLabel.Text = Data.XParam.AxisName;
						YAxisLabel.NewText = Data.YParam.AxisName;
						ValueLabel.Text = Data.ValueName;

						GridPanel.Visible = true;
						Grid.Worksheets.Clear();

						if (Data.Worksheet == null) {
							Worksheet WSheet = Grid.Worksheets.Create(string.Format("{0} / {1}", Data.XParam.AxisName, Data.YParam.AxisName));
							Data.Worksheet = WSheet;

							WSheet.RowCount = 1;
							WSheet.ColumnCount = 1;

							Data.PopulateSheet(WSheet);
							Interpolate(new GridSelection(WSheet));
						}

						Grid.CurrentWorksheet = Data.Worksheet;
						ColorSheet(Data.Worksheet);
						break;
					}

				case EditMode.Property: {
						PropertyPanel.Visible = true;
						Properties.SelectedObject = Data;
						break;
					}

				default:
					throw new Exception("Invalid edit mode " + Data.EditMode);
			}
		}
	}
}
