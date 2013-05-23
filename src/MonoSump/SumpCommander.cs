using System;
using System.Text;

namespace Earlz.MonoSump
{
	public class SumpCommander : ISumpCommander
	{
		IPort Port;
		public SumpCommander(IPort port)
		{
			Port=port;
		}


		public void Reset()
		{
			var bytes=new byte[]{0x00,0x00,0x00,0x00,0x00};
			Port.WriteBytes(bytes);
		}

		public void Run()
		{
			throw new NotImplementedException();
		}

		public string GetID()
		{
			Port.WriteByte(0x02);
			byte[] bytes=new byte[4];
			for(int i=3;i>=0;i--) //little endian, so we have to reverse it
			{
				bytes[i]=(byte)Port.ReadByte(100);
			}
			return Encoding.ASCII.GetString(bytes);
		}

		public void SetTriggerMasks(bool[] mask)
		{
			throw new NotImplementedException();
		}

		public void SetTriggerValues(bool[] values)
		{
			throw new NotImplementedException();
		}

		public void SetTriggerConfigurations(TriggerConfiguration[] configs)
		{
			throw new NotImplementedException();
		}

	}
}

