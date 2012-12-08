namespace Junior.Persist.Sessions.Sessions
{
	/// <summary>
	/// Handles actions observed in a session by writing a line to the console.
	/// </summary>
	public class ConsoleSessionObserver<TCacheKey, TEntity> : WriteLineSessionObserver<TCacheKey, TEntity>
		where TCacheKey : class
		where TEntity : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleSessionObserver{TCacheKey,TEntity}"/> class.
		/// <see cref="ConsoleLineWriter"/> will accept written lines.
		/// </summary>
		public ConsoleSessionObserver()
			: base(new ConsoleLineWriter())
		{
		}
	}
}