using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test {
	class Program {
		public static ConcurrentQueue<int> IntQueue;

		public static void Interrupt(int N) {
			IntQueue.Enqueue(N);
		}

		[STAThread]
		static void Main(string[] args) {
			IntQueue = new ConcurrentQueue<int>();
			ECU.ecu_init();

			Thread InputThread = new Thread(ReadInput);
			InputThread.SetApartmentState(ApartmentState.STA);
			InputThread.IsBackground = true;
			InputThread.Start();

			while (true) {
				Thread.Sleep(5);

				if (IntQueue.TryDequeue(out int Int))
					ECU.ecu_interrupt(Int);

				ECU.ecu_loop();
			}
		}

		static void ReadInput() {
			InputForm InputForm = new InputForm();
			Application.EnableVisualStyles();
			Application.Run(InputForm);
			Environment.Exit(0);
		}
	}
}
