using System;

namespace Earlz.MonoSump
{
	/// <summary>
	///  interface for a general "port" of some sort, such as a serial port
	/// (mainly to make testing easy)
	/// </summary>
	public interface IPort
	{
		/// <summary>
		/// Reads a byte if available. Otherwise, instantly will return null
		/// </summary>
		/// <returns>The byte if available.</returns>
		byte? ReadByteIfAvailable();
		/// <summary>
		/// Writes a byte to the port
		/// </summary>
		/// <param name="value">Value.</param>
		void WriteByte(byte value);
		/// <summary>
		/// Reads a byte when available. Blocks until it times out
		/// </summary>
		/// <returns>The byte.</returns>
		byte? ReadByte(int timeout);
		
		int BitRate{get;} //?
	}
}

