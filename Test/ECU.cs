using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Test {
	static class ECU {
		const string DllName = "ECU_Mk1";
		const CallingConvention CConv = CallingConvention.Cdecl;

		[DllImport(DllName, CallingConvention = CConv)]
		public static extern void ecu_init();

		[DllImport(DllName, CallingConvention = CConv)]
		public static extern void ecu_loop();

		[DllImport(DllName, CallingConvention = CConv)]
		public static extern void ecu_interrupt(int n);
	}
}
