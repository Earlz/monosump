using System;
using System.Text;
using System.Collections.Generic;

namespace Earlz.MonoSump
{
	public class SumpCommander : ISumpCommander
	{
		//this is copied almost verbatim from GPL code, but it's just constants, so it shouldn't matter that this is BSD licensed
		/// <summary>
		/// demultiplex
		/// </summary>
		const int FLAG_DEMUX = 0x00000001;		// demultiplex
		/// <summary>
		/// Noise filter
		/// </summary>
	    const int FLAG_FILTER = 0x00000002;		// noise filter
		/// <summary>
		/// Disable channel group 0
		/// </summary>
	    const int FLAG_DISABLE_G0 = 0x00000004;	// disable channel group 0
		/// <summary>
		/// Disable channel gorup 1
		/// </summary>
	    const int FLAG_DISABLE_G1 = 0x00000008;	// disable channel group 1
		/// <summary>
		/// Disable channel group 2
		/// </summary>
	    const int FLAG_DISABLE_G2 = 0x00000010;	// disable channel group 2
		/// <summary>
		/// Disable channel group 3
		/// </summary>
	    const int FLAG_DISABLE_G3 = 0x00000020;	// disable channel group 3
		/// <summary>
		/// Use external clock
		/// </summary>
	    const int FLAG_EXTERNAL = 0x00000040;	// disable channel group 3
		/// <summary>
		/// Invert the clock (only works with internal clock)
		/// </summary>
	    const int FLAG_INVERTED = 0x00000080;	// disable channel group 3
		/// <summary>
		/// Run length encoding (currently no plans to implement, since I don't have hardware capable of testing)
		/// </summary>
	    const int FLAG_RLE = 0x00000100;	// run length encoding
		/// <summary>
		/// Mask for delay value
		/// </summary>
	    const int TRIGGER_DELAYMASK = 0x0000ffff;// mask for delay value
		/// <summary>
		/// Mask for level value
		/// </summary>
	    const int TRIGGER_LEVELMASK = 0x00030000;// mask for level value
		/// <summary>
		/// Mask for channel for the trigger
		/// </summary>
	    const int TRIGGER_CHANNELMASK = 0x01f00000;// mask for level value
		/// <summary>
		/// Trigger uses serial mode
		/// </summary>
	    const int TRIGGER_SERIAL = 0x04000000;	// trigger operates in serial mode
		/// <summary>
		/// Trigger should start capture when fired
		/// </summary>
	    const int TRIGGER_CAPTURE = 0x08000000;	// trigger will start capture when fired
		/// <summary>
		/// clock speed in Hz
		/// </summary>
	    const int CLOCK = 100000000;	// device clock in Hz
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
			Port.WriteByte(0x01);
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

		public void SetTriggerMasks(int stage, bool[] mask)
		{
			Port.WriteByte((byte)(0xC0|(stage<<2)));
			for(int i=3;i>=0;i--)
			{
				int val=0;
				for(int j=0;j<8;j++)
				{
					val|=(mask[j*i] ? 1 : 0) << j;
				}
				Port.WriteByte((byte)val);
			}
		}
		public void SetTriggerValues(int stage, bool[] values)
		{
			Port.WriteByte((byte)(0xC1|(stage<<2)));
			for(int i=3;i>=0;i--)
			{
				int val=0;
				for(int j=0;j<8;j++)
				{
					val|=(values[j*i] ? 1 : 0) << j;
				}
				Port.WriteByte((byte)val);
			}
		}
		public void SetTriggerConfigurations(int stage, TriggerConfiguration config)
		{
			Port.WriteByte((byte)(0xC2|(stage<<2)));
			Port.WriteByte((byte)(config.Delay & 0xFF));
			Port.WriteByte((byte)(config.Delay & 0xFF00 >> 8));
			int tmp=(config.Channel & 0xF) << 4;
			tmp|=config.Level;
			Port.WriteByte((byte)tmp);
			tmp=0;
			tmp=(config.Start ? 1 : 0) << 3;
			tmp|=(config.Serial ? 1 : 0) << 2;
			tmp|=(config.Channel & 0x1F) >> 4;
			Port.WriteByte((byte)tmp);
		}
		public void SetDivider(int divider)
		{
			throw new NotImplementedException();
		}
		public void SetSampleSizeAndDelay(int size, int delay)
		{
			throw new NotImplementedException();
		}
		public void SetFlags(SumpFlags flags)
		{
			throw new NotImplementedException();
		}
		public IList<bool[]> GetData(int expectedSize, int timeout)
		{
			var list=new List<bool[]>(expectedSize);
			var current=new bool[32]; //32 channels
			var currentTimeout=timeout;
			while(true)
			{
				for(int i=0;i<4;i++)
				{
					var tmp=Port.ReadByte(currentTimeout);
					if(tmp==null)
					{
						return list;
					}
					int data=tmp.Value;
					for(int j=0;j<8;j++)
					{
						current[i*j]=(data & (1 << j)) > 0;
					}
					currentTimeout=1000; //use a much more reasonable timeout after we start getting data
				}
				list.Add(current);
				current=new bool[32];
			}
		}
	}
}

