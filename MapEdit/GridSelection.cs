using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unvell.ReoGrid;

namespace MapEdit {
	class GridSelection {
		public Worksheet Sheet;
		public Cell A, B, C, D;

		public int X1 {
			get {
				return A.Column;
			}
		}

		public int X2 {
			get {
				return B.Column;
			}
		}

		public int Y1 {
			get {
				return A.Row;
			}
		}

		public int Y2 {
			get {
				return C.Row;
			}
		}

		public int Width {
			get {
				return X2 - X1 + 1;
			}
		}

		public int Height {
			get {
				return Y2 - Y1 + 1;
			}
		}

		public GridSelection() {
		}

		public GridSelection(Worksheet Sheet) {
			this.Sheet = Sheet;
			A = Sheet.Cells[0, 0];
			B = Sheet.Cells[0, Sheet.ColumnCount - 1];
			C = Sheet.Cells[Sheet.RowCount - 1, 0];
			D = Sheet.Cells[Sheet.RowCount - 1, Sheet.ColumnCount - 1];
		}

		public RangePosition ToRange() {
			return new RangePosition(Y1, X1, Height, Width);
		}
	}
}
