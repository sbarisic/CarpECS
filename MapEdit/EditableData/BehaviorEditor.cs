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
	[DesignerCategory("Properties"), DisplayName("LookupTableSettings")]
	public class BehaviorEditor : EditableData {
		public BehaviorEditor() : base(EditMode.Nodes) {

		}



	}
}
