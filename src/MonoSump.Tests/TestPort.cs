using System;
using System.Collections.Generic;

namespace Earlz.MonoSump.Tests
{
	public class TestPort : IPort
	{

		public byte? ReadByteIfAvailable()
		{
			throw new NotImplementedException();
		}
		public List<byte> ReceivedBytes=new List<byte>();
		public void WriteByte(byte value)
		{
			ReceivedBytes.Add(value);
		}

		public void WriteBytes(byte[] values)
		{
			throw new NotImplementedException();
		}
		public List<byte> BytesToSend=new List<byte>();
		public int SendPosition=0;
		public bool ForceTimeout=false;
		public byte? ReadByte(int timeout)
		{
			if(ForceTimeout)
			{
				return null;
			}
			return BytesToSend[SendPosition++];
		}

		public int BitRate
		{
			get
			{
				throw new NotImplementedException();
			}
		}


		public TestPort()
		{
		}
	}
}

