namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Handles actions observed in a session by writing a line to the debugger.
	/// </summary>
	public class DebugSessionObserver<TCacheKey, TEntity> : WriteLineSessionObserver<TCacheKey, TEntity>
		where TCacheKey : class
		where TEntity : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DebugSessionObserver{TCacheKey,TEntity}"/> class.
		/// <see cref="DebugLineWriter"/> will accept written lines.
		/// </summary>
		public DebugSessionObserver()
			: base(new DebugLineWriter())
		{
		}
	}
}