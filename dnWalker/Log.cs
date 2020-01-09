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
	using System.Collections;
	using MMC.Util;

	[System.Flags]
	enum LogPriority {

		None		= 0,
		Debug 		= 1<<1,
		Notice		= 1<<2,
		Message		= 1<<3,
		Warning		= 1<<4,
		Severe		= 1<<5,
		Fatal		= 1<<6,
		Trace		= 1<<7,
		Lookup		= 1<<8,
		Interactive = 1<<9,
		Call		= 1<<10,
		TLocalOnly	= 1<<21,	// print trace iff pc is in asm to check.
		TInstrOnly	= 1<<22		// only print instr in trace (no local state).
	}

	class Logger {

		static Logger instance = new Logger();
		IList m_outputs;
		LogPriority m_logFilter;

		public static Logger l {

			get { return instance; }
		}

		public int OutputCount {

			get { return m_outputs.Count; }
		}

		public bool ParseAndSetLogFilter(string format) {

			LogPriority newLogFilter = LogPriority.None;
			char[] fmtChars = format.ToCharArray();

			int i=0;
			if (fmtChars[0] == '+') {
				newLogFilter = m_logFilter;
				++i;
			} 
			else if (fmtChars[0] == '-') {
				newLogFilter = ~m_logFilter;
				++i;
			} 
			else if (fmtChars[0] == '=')
				++i;

			for (; i < fmtChars.Length; ++i) {
				switch (fmtChars[i]) {
				case 'd':
					newLogFilter |= LogPriority.Debug;
					newLogFilter |= LogPriority.Call;
					break;
				case 'f':
					newLogFilter |= LogPriority.Fatal;
					break;
				case 'i':
					newLogFilter |= LogPriority.TInstrOnly;
					break;
				case 'l':
					newLogFilter |= LogPriority.TLocalOnly;
					break;
				case 'o':
					newLogFilter |= LogPriority.Lookup;
					break;
				case 'm':
					newLogFilter |= LogPriority.Message;
					break;
				case 'n':
					newLogFilter |= LogPriority.Notice;
					break;
				case 's':
					newLogFilter |= LogPriority.Severe;
					break;
				case 't':
					newLogFilter |= LogPriority.Trace;
					break;
				case 'w':
					newLogFilter |= LogPriority.Warning;
					break;
				default:
					return false;
				}
			}
			if (fmtChars[0] == '-')
				m_logFilter = ~newLogFilter;
			else
				m_logFilter = newLogFilter;

			return true;
		}

		public LogPriority Filter {

			get { return m_logFilter; }
		}

		public int AddOutput(ILoggerOutput output) {

			return m_outputs.Add(output);
		}

		public void Debug(string msg, params object[] values) {

			Log(LogPriority.Debug, msg, values);
		}

		public void Notice(string msg, params object[] values) {

			Log(LogPriority.Notice, msg, values);
		}

		public void Message(string msg, params object[] values) {

			Log(LogPriority.Message, msg, values);
		}

		public void Warning(string msg, params object[] values) {

			Log(LogPriority.Warning, msg, values);
		}

		public void Trace(string msg, params object[] values) {

			Log(LogPriority.Trace, msg, values);
		}

		public void Lookup(string msg, params object[] values) {

			Log(LogPriority.Lookup, msg, values);
		}

		public void Log(LogPriority pr, string msg, params object[] values) {

			if ((pr & m_logFilter) != 0) {
				if (values.Length > 0)
					msg = string.Format(msg, values);
				foreach (ILoggerOutput output in m_outputs)
					output.Log(pr, msg);
			}
		}

		public void FlushOutputs() {
			foreach (ILoggerOutput output in m_outputs)
				output.Flush();
		}

		public void CloseAll() {

			foreach (ILoggerOutput output in m_outputs)
				output.Close();
		}

		private /* singleton */ Logger() {

			m_outputs = new ArrayList();
			m_logFilter = LogPriority.Notice |
				LogPriority.Message | LogPriority.Warning | 
				LogPriority.Severe | LogPriority.Fatal;
		}
	}

	interface ILoggerOutput {

		void Log(LogPriority lp, string msg);
		void Flush();
		void Close();
	}

	class TextLoggerOutput : ILoggerOutput {

		TextWriter m_sink;

		public TextLoggerOutput(TextWriter sink) {

			m_sink = sink;
		}

		public void Log(LogPriority lp, string msg) {

			System.DateTime now = System.DateTime.Now;
			m_sink.WriteLine("{0:D2}:{1:D2}:{4:D2} [{2,12}] {3}", now.Hour, now.Minute, lp, msg, now.Second);
		}

		public void Flush() {

			m_sink.Flush();
		}

		public void Close() {

			m_sink.Close();
		}
	}

}
