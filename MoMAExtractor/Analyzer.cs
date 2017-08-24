using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MoMA.Analyzer;
using Ionic.Zip;

namespace MoMAExtractor
{
	public enum Versions { v20, v30, v35, v40, v45, v451, v452, v46, v461, v462, v47, Mobile, WinForms };
	public class Analyzer {

		static string MonoBin = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Mono\bin\mono.exe");

		public static void Analyze(Versions version, bool count_only) {

			// Parameters to fiddle with
			bool use_20 = version == Versions.v20; // Include the 2.0 framework
			bool use_30 = version == Versions.v30; // Include the 3.0 framework
			bool use_35 = version == Versions.v35; // Include the 3.5 framework
			bool use_40 = version == Versions.v40;  // Include the 4.0 framework
			bool use_45 = version == Versions.v45;  // Include the 4.0 framework
			bool use_451 = version == Versions.v451;  // Include the 4.0 framework
			bool use_452 = version == Versions.v452;  // Include the 4.0 framework
			bool use_46 = version == Versions.v46;  // Include the 4.0 framework
			bool use_461 = version == Versions.v461;  // Include the 4.0 framework
			bool use_462 = version == Versions.v462;  // Include the 4.0 framework
			bool use_47 = version == Versions.v47;  // Include the 4.0 framework
			bool use_mobile = version == Versions.Mobile;

			bool use_design = true; // Include *Design namespaces
			bool mwf_only = version == Versions.WinForms;   // Only do System.Windows.Forms (overrides others)

			string output_path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Definitions");

			// Get the assemblies we want to examine
			List<string> mono_assemblies = AssemblyManager.GetAssemblies(true, use_20, use_30, use_35, use_40, use_45, use_451, use_452, use_46, use_461, use_462, use_47, use_mobile, use_design, mwf_only);
			List<string> ms_assemblies = AssemblyManager.GetAssemblies(false, use_20, use_30, use_35, use_40, use_45, use_451, use_452, use_46, use_461, use_462, use_47, use_mobile, use_design, mwf_only);

			string displayVersion;
			switch (version) {
				case Versions.v20: displayVersion = "2.0"; break;
				case Versions.v30: displayVersion = "3.0"; break;
				case Versions.v35: displayVersion = "3.5"; break;
				case Versions.v40: displayVersion = "4.0"; break;
				case Versions.v45: displayVersion = "4.5"; break;
				case Versions.v451: displayVersion = "4.5.1"; break;
				case Versions.v452: displayVersion = "4.5.2"; break;
				case Versions.v46: displayVersion = "4.6"; break;
				case Versions.v461: displayVersion = "4.6.1"; break;
				case Versions.v462: displayVersion = "4.6.2"; break;
				case Versions.v47: displayVersion = "4.7"; break;
				case Versions.Mobile: displayVersion = "Mobile"; break;
				default: displayVersion = version.ToString(); break;
			}
			Console.CursorTop = 0;
			Console.WriteLine();
			Console.WriteLine($"Analyzing for .NET {displayVersion}...");

			var procInfo = new ProcessStartInfo(MonoBin, "-V") {
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				UseShellExecute = false
			};
			var monoproc = Process.Start(procInfo);
			var res = monoproc.StandardOutput.ReadToEnd();
			var monoVersion = Regex.Match(res, "[0-9.]+").Value;
			var monoName = $"{monoVersion}-{displayVersion}-defs";

			output_path = Path.Combine(output_path, monoName);

			if (!Directory.Exists(output_path)) Directory.CreateDirectory(output_path);

			// Extract all methods from the MS assemblies
			SortedList<string, Method> ms_all = new SortedList<string, Method> ();

			Console.CursorTop = 3;
			Console.WriteLine(".NET Assemblies:                                  ");
			foreach (string assembly in ms_assemblies) {
				Console.CursorTop = 5;
				MethodExtractor.ExtractFromAssembly(assembly, ms_all, null, null);
			}

			// Extract all, NIEX, and TODO methods from Mono assemblies
			SortedList<string, Method> missing = new SortedList<string, Method> ();
			SortedList<string, Method> all = new SortedList<string, Method> ();
			SortedList<string, Method> todo = new SortedList<string, Method> ();
			SortedList<string, Method> nie = new SortedList<string, Method> ();

			Console.CursorTop = 3;
			Console.WriteLine("Mono Assemblies:                              ");
			foreach (string assembly in mono_assemblies) {
				Console.CursorTop = 5;
				MethodExtractor.ExtractFromAssembly(assembly, all, nie, todo);
			}

			// Only report the TODO's that are also in MS's assemblies
			SortedList<string, Method> final_todo = new SortedList<string, Method> ();

			foreach (string s in todo.Keys)
				if (ms_all.ContainsKey (s))
					final_todo[s] = todo[s];

			var monotodoTxt = Path.Combine(output_path, "monotodo.txt");
			WriteListToFile (final_todo, monotodoTxt, true);
			
			// Only report the NIEX's that are also in MS's assemblies
			SortedList<string, Method> final_nie = new SortedList<string, Method> ();

			foreach (string s in nie.Keys)
				if (ms_all.ContainsKey (s))
					final_nie[s] = nie[s];

			var exceptionTxt = Path.Combine(output_path, "exception.txt");
			WriteListToFile (final_nie, exceptionTxt, false);
			
			// Write methods that are both TODO and NIEX
			SortedList<string, Method> todo_niex = new SortedList<string, Method> ();

			foreach (string s in nie.Keys)
				if (todo.ContainsKey (s))
					todo_niex.Add (s, todo[s]);

			var dupeTxt = Path.Combine(output_path, "dupe.txt");
			WriteListToFile (todo_niex, dupeTxt, true);
			
			// Find methods that exist in MS but not in Mono (Missing methods)
			MethodExtractor.ComputeMethodDifference (ms_all, all, missing, use_design);

			var missingTxt = Path.Combine(output_path, "missing.txt");
			WriteListToFile (missing, missingTxt, false);

			// summary

			Console.CursorTop = 3;
			Console.WriteLine("Summary...                                      ");
			var summaryTxt = Path.Combine(output_path, "summary.txt");
			StreamWriter sw = new StreamWriter(summaryTxt);

			foreach (string assembly in ms_assemblies) {
				ms_all.Clear();
				missing.Clear();

				// Get all methods in MS assembly
				Console.CursorTop = 5;
				MethodExtractor.ExtractFromAssembly(assembly, ms_all, null, null);
				string file = Path.GetFileName(assembly);

				// We only want MS method counts
				if (count_only) {
					sw.WriteLine("{0}: {1}", file, ms_all.Count);
					continue;
				}

				// Find the matching Mono assembly
				string mono_file = string.Empty;

				foreach (string s in mono_assemblies)
					if (s.ToLower().Contains(file.ToLower()))
						mono_file = s;

				if (string.IsNullOrEmpty(mono_file)) {
					sw.WriteLine("No Mono assembly found for " + file);
					continue;
				}

				// Do the MoMA extracts/compares, and output the results
				all.Clear();
				todo.Clear();
				nie.Clear();

				Console.CursorTop = 5;
				MethodExtractor.ExtractFromAssembly(mono_file, all, nie, todo);

				final_todo.Clear();

				foreach (string s in todo.Keys)
					if (ms_all.ContainsKey(s))
						final_todo[s] = todo[s];

				sw.WriteLine(file);
				sw.WriteLine(string.Format("TODO: {0}", final_todo.Count));

				final_nie.Clear();

				foreach (string s in nie.Keys)
					if (ms_all.ContainsKey(s))
						final_nie[s] = nie[s];

				sw.WriteLine(string.Format("NIEX: {0}", final_nie.Count));

				MethodExtractor.ComputeMethodDifference(ms_all, all, missing, true);
				sw.WriteLine(string.Format("MISS: {0}", missing.Count));
			}

			sw.Close();
			sw.Dispose();

			var versionTxt = Path.Combine(output_path, "version.txt");
			File.WriteAllText(versionTxt, $@"Mono {monoVersion} ({displayVersion} Profile)
{DateTime.Now.ToString("MM/dd/yyyy")}");

			var zipName = Path.Combine(Path.GetDirectoryName(output_path), $"{monoName}.zip");
			if (File.Exists(zipName)) File.Delete(zipName);
			using (var zip = new ZipFile(zipName)) {
				zip.AddFiles(new string[] { monotodoTxt, exceptionTxt, dupeTxt, missingTxt, summaryTxt, versionTxt }, false, "/");
				zip.Save();
			}

			Directory.Delete(output_path, true);
		}

		private static void WriteListToFile (SortedList<string, Method> list, string filename, bool writeDataField)
		{
			StreamWriter sw = new StreamWriter (filename);
			
			foreach (Method m in list.Values) {
				if (writeDataField)
					sw.WriteLine ("{0}-{1}", m.RawMethod, m.Data);
				else
					sw.WriteLine (m.RawMethod);
			}
			
			sw.Close ();
		}
	}
}
