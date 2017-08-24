using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoMAExtractor {
	public class Program {

		public static void Main(string[] args) {
			for (Versions ver = Versions.v20; ver < Versions.WinForms; ver++) Analyzer.Analyze(ver, false);
			Console.Clear();
			Console.WriteLine("done.");
			Console.ReadLine();
		}
	}
}
