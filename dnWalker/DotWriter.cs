/*
 *   Copyright 2007 University of Twente, Formal Methods and Tools group
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *
 */

namespace MMC {

	using System.IO;
	using MMC.Util;

	enum DotOutputType { None, Normal, TeX }

	interface IDotFormatStringProvider {

		/// The string to write in the preamble of the Dot graph.
		string Begin { get; }
		/// <summary>Format string for writing nodes.</summary>
		/// The format parameters are as follows. 
		/// 0 (int) is the node ID
		/// 1 (string) is the list of runnable threads (comma separated).
		string Node { get; }
		/// <summary>The format for writing edges.</summary>
		/// The format parameters are as follows. 
		/// 0 (int) is the "from" node.
		/// 1 (int) is the "to" node.
		/// 2 (int) is the edge ID.
		/// 3 (int) is the thread that was run.
		string Edge { get; }
		/// <summary>Format string for a backtrack edge.</summary>
		/// Same format as Edge, except parameter 3 does not exist.
		/// \sa Edge
		string BacktrackEdge { get; }
		/// The string to write at the end of the Dot graph.
		string End { get; }
	}

	struct NormalFormatStringProvider : IDotFormatStringProvider {

		const string begin	= "digraph G {\n\t-1 [label=\"start\"];";
		const string node	= "\t{0} [label=\"{0} {{{1}}}\"];";
		const string edge	= "\t{0} -> {1} [label=\"{2}{3}\"];";
		const string btedge	= "\t{0} -> {1} [label=\"bt\",style=dotted];";
		const string end	= "}";

		public string Begin { get { return begin; } }
		public string Node { get { return node; } }
		public string Edge { get { return edge; } }
		public string BacktrackEdge { get { return btedge; } }
		public string End { get { return end; } }
	}

	struct TeXFormatStringProvider : IDotFormatStringProvider {

		const string begin	= "digraph G {\n\t-1 [label=\"start\"];";
		const string node	= "\t{0} [label=\"{0}:\\ {1}\"]";
		const string edge	= "\t{0} -> {1} [label=\"$e_{{{2}}}: \\{{{3}\\}}$\"];";
		const string btedge	= "\t{0} -> {1} [label=\"$b_{{ {2} }}$\"];";
		const string end	= "}";

		public string Begin { get { return begin; } }
		public string Node { get { return node; } }
		public string Edge { get { return edge; } }
		public string BacktrackEdge { get { return btedge; } }
		public string End { get { return end; } }
	}

	class DotWriter {

		static TextWriter s_dotsink = null;
		static int s_edgecount = 0;
		
		static DotOutputType s_outputType = DotOutputType.None;
		static IDotFormatStringProvider s_fmtProvider = null;

		public static bool IsEnabled() {

			return s_dotsink != null;
		}

		public static DotOutputType OutputType {

			get { return s_outputType; }
			set {
				s_outputType = value;
				if (s_outputType == DotOutputType.Normal)
					s_fmtProvider = new NormalFormatStringProvider();
				else if (s_outputType == DotOutputType.TeX)
					s_fmtProvider = new TeXFormatStringProvider();
			}
		}

		public static void Begin(TextWriter dotsink) {

			if (dotsink == null)
				throw new System.ArgumentNullException("dotsink");
			if (s_dotsink != null)
				throw new System.InvalidOperationException("old dot writer not closed");
			s_dotsink = dotsink;
			if (s_fmtProvider == null)
				OutputType = DotOutputType.Normal;
			s_dotsink.WriteLine(s_fmtProvider.Begin);
		}

		public static void NewNode(int id, System.Collections.IList runnable) {

			if (s_dotsink == null)
				throw new System.InvalidOperationException("dot writer not initialized.");
			s_dotsink.WriteLine(s_fmtProvider.Node, id, new ListToString(runnable));
		}

		public static void NewEdge(int from, int to, int via) {

			if (s_dotsink == null)
				throw new System.InvalidOperationException("dot writer not initialized.");
			if (via == DotGraph.BacktrackEdge)
				s_dotsink.WriteLine(s_fmtProvider.BacktrackEdge, from, to);
			else {
				s_dotsink.WriteLine(s_fmtProvider.Edge, from, to, via, "");
			}
		}

		public static void NewEdge(int from, int to, int via, string label) {
			s_dotsink.WriteLine(s_fmtProvider.Edge, from, to, via, label);

		}


		public static void End() {

			if (s_dotsink != null) {
				s_dotsink.WriteLine(s_fmtProvider.End);
				s_dotsink.Flush();
				s_dotsink.Close();
				s_dotsink = null;
			}
		}
	}
}

// vim:nofen
