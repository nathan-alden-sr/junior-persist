namespace Junior.Persist.Persistence.Finders
{
	/// <summary>
	/// Indicates how to handle when entity data is not found. 
	/// </summary>
	public enum EntityNotFoundHandling
	{
		/// <summary>
		/// An exception should be thrown when entity data is not found.
		/// </summary>
		ThrowException,
		/// <summary>
		/// Null should be returned when entity data is not found.
		/// </summary>
		ReturnNull
	}
}