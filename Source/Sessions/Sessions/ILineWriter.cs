namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Represents a way to write observed cache messages as lines.
	/// </summary>
	public interface ILineWriter
	{
		/// <summary>
		/// Writes an observed cache message as a line.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using <paramref name="format"/>.</param>
		void WriteLine(string format, params object[] arg);
	}
}