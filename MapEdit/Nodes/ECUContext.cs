using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NodeEditor;

namespace MapEdit {
	public class ECUContext : INodesContext {
		public NodeVisual CurrentProcessingNode {
			get; set;
		}

		public event Action<string, NodeVisual, FeedbackType, object, bool> FeedbackInfo;

		[Node("Scalar", "Input", IsCallable = false)]
		public void Scalar(out float Value) {
			Console.WriteLine("Scalar!");

			Value = 1;
		}

		[Node("Output", "Output", IsCallable = false, IsExecutionInitiator = true)]
		public void Output(float Value) {
			var CurNode = this.CurrentProcessingNode;


			Console.WriteLine("Output called!");

			// Do something

		}

		[Node("Add", "Operators", "Basic", "Adds two input values.", false)]
		public void Add(float a, float b, out float result) {
			Console.WriteLine("Add!");

			result = a + b;
		}

		[Node("Substract", "Operators", "Basic", "Substracts two input values.", false)]
		public void Substract(float a, float b, out float result) {
			Console.WriteLine("Substract!");

			result = a - b;
		}

		[Node("Multiply", "Operators", "Basic", "Multiplies two input values.", false)]
		public void Multiplty(float a, float b, out float result) {
			Console.WriteLine("Multiplty!");

			result = a * b;
		}

		[Node("Divide", "Operators", "Basic", "Divides two input values.", false)]
		public void Divid(float a, float b, out float result) {
			Console.WriteLine("Divid!");

			result = a / b;
		}

		/*
		[Node("Show Value", "Helper", "Basic", "Shows input value in the message box.")]
		public void ShowMessageBox(float x) {
			MessageBox.Show(x.ToString(), "Show Value", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		[Node("Starter", "Helper", "Basic", "Starts execution", true, true)]
		public void Starter() {

		}*/
	}
}
