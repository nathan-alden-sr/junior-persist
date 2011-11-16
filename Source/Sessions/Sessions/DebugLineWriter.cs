using System;
using System.Diagnostics;

using Junior.Common;

namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Write observed cache messages to the debugger as lines. <see cref="Debug.WriteLine(string, object[])"/> is used.
	/// </summary>
	public class DebugLineWriter : ILineWriter
	{
		/// <summary>
		/// Writes an observed cache message as a line.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using <paramref name="format"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
		public void WriteLine(string format, params object[] arg)
		{
			format.ThrowIfNull("format");

			Debug.WriteLine(format, arg);
		}
	}
}