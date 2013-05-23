using System;
using System.IO.Ports;

namespace Earlz.MonoSump
{
	public class Serial : IPort, IDisposable
	{
		public SerialPort SerialPort;
		object locker=new object();
		public byte? ReadByteIfAvailable()
		{
			lock(locker)
			{
				if(SerialPort.BytesToRead>0)
				{
					return (byte)SerialPort.ReadByte(); //we can never have "end of stream", so -1 isn't a worry
				}
				else
				{
					return null;
				}
			}
		}

		public void WriteByte(byte value)
		{
			SerialPort.Write(new byte[]{value}, 0, 1);
		}
		public void WriteBytes(byte[] values)
		{
			SerialPort.Write(values, 0, values.Length);
		}
		public byte? ReadByte(int timeout)
		{
			lock(locker)
			{
				try
				{
					return (byte)SerialPort.ReadByte();
				}
				catch //clause?
				{
					return null;
				}
			}
		}

		public int BitRate
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void Dispose()
		{
			SerialPort.Close();
			SerialPort.Dispose();
		}

		public Serial(string name, int baudrate)
		{
			SerialPort=new SerialPort(name, baudrate);
			SerialPort.Open();
		}
	}
}

