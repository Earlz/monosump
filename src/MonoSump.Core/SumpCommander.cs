using System;
using System.Collections.Generic;
using System.Text;

namespace Earlz.MonoSump.Core
{
	public class SumpCommander : ISumpCommander
	{
		//this is copied almost verbatim from GPL code, but it's just constants, so it shouldn't matter that this is BSD licensed
		/// <summary>
		/// demultiplex
		/// </summary>
		const byte FLAG_DEMUX = 0x01;		// demultiplex
		/// <summary>
		/// Noise filter
		/// </summary>
		const byte FLAG_FILTER = 0x02;		// noise filter
		/// <summary>
		/// Disable channel group 0
		/// </summary>
		const byte FLAG_DISABLE_G0 = 0x00000004;	// disable channel group 0
		/// <summary>
		/// Disable channel gorup 1
		/// </summary>
		const byte FLAG_DISABLE_G1 = 0x00000008;	// disable channel group 1
		/// <summary>
		/// Disable channel group 2
		/// </summary>
		const byte FLAG_DISABLE_G2 = 0x00000010;	// disable channel group 2
		/// <summary>
		/// Disable channel group 3
		/// </summary>
		const byte FLAG_DISABLE_G3 = 0x00000020;	// disable channel group 3
		/// <summary>
		/// Use external clock
		/// </summary>
		const byte FLAG_EXTERNAL = 0x00000040;	// disable channel group 3
		/// <summary>
		/// Invert the clock (only works with internal clock)
		/// </summary>
		const byte FLAG_INVERTED = 0x00000080;	// disable channel group 3
		/// <summary>
		/// Run length encoding (currently no plans to implement, since I don't have hardware capable of testing)
		/// </summary>
		// const byte FLAG_RLE = 0x00000100;	// run length encoding
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
			Assert(config.Delay > 0xFFFF, "Trigger Delay can not be greater than 0xFFFF!");
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
			Assert(divider>0xFFFFFF, "Divider can not be greater than 0xFFFFFF!");
			Port.WriteByte((byte)0x80);
			Port.WriteByte((byte)(divider&0xFF));
			Port.WriteByte((byte)((divider&0xFF00) >> 8));
			Port.WriteByte((byte)((divider&0xFF0000) >> 16));
			Port.WriteByte(0); //doesn't matter
		}
		
		public void SetReadAndDelayCount(int read, int delay)
		{
			Assert(read>0xFFFF, "ReadCount can not be greater than 0xFFFF!");
			Assert(delay>0xFFFF, "DelayCount can not be greater than 0xFFFF!");
			Port.WriteByte((byte)0x81);
			Port.WriteByte((byte)((read & 0xFF)));
			Port.WriteByte((byte)(((read & 0xFF00) >> 8)));
			Port.WriteByte((byte)((delay &0xFF)));
			Port.WriteByte((byte)(((delay & 0xFF00) >> 8)));
		}
		public void SetFlags(SumpFlags flags)
		{
			Port.WriteByte((byte)0x82);
			int tmp=0;
			if(flags.InvertedClock)
			{
				tmp|=FLAG_INVERTED;
			}
			if(flags.Demux)
			{
				tmp|=FLAG_DEMUX;
			}
			if(flags.ExternalClock)
			{
				tmp|=FLAG_INVERTED;
			}
			if(flags.Filter)
			{
				tmp|=FLAG_FILTER;
			}
			if(flags.DisabledChannelGroups[0])
			{
				tmp|=FLAG_DISABLE_G0;
			}
			if(flags.DisabledChannelGroups[1])
			{
				tmp|=FLAG_DISABLE_G1;
			}
			if(flags.DisabledChannelGroups[2])
			{
				tmp|=FLAG_DISABLE_G2;
			}
			if(flags.DisabledChannelGroups[3])
			{
				tmp|=FLAG_DISABLE_G3;
			}
			Port.WriteByte((byte)tmp);
			Port.WriteByte(0);
			Port.WriteByte(0);
			Port.WriteByte(0);
		}
		public int Clock
		{
			get
			{
				return 100000000;	// device clock in Hz
			}
		}
		void Assert(bool condition, string message)
		{
			if(condition)
				throw new ApplicationException(message);
		}

		public IList<bool[]> GetData(int expectedSize, int initialTimeout, int runningTimeout)
		{
			var list=new List<bool[]>(expectedSize);
			var current=new bool[32]; //32 channels
			var currentTimeout=initialTimeout;
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
					currentTimeout=runningTimeout; //use a much more reasonable timeout after we start getting data
				}
				list.Add(current);
				current=new bool[32];
			}
			return list;
		}
	}
}

