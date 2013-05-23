using System;

namespace Earlz.MonoSump
{
	/// <summary>
	/// An interface for describing an opcode
	/// </summary>
	public interface IOpcode
	{
		byte[] GetBytes();
	}
}

