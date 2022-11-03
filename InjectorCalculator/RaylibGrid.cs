using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace InjectorCalculator {
	class RaylibGrid {
		public Vector2 Pos;
		public Vector2 Size;

		public string XName;
		public string YName;

		public RaylibGrid(Vector2 Pos, Vector2 Size, string XName, string YName) {
			this.Pos = Pos;
			this.Size = Size;
			this.XName = XName;
			this.YName = YName;
		}


		public void Draw() {

		}

	}
}
