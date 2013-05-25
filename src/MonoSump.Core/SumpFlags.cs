using System;

namespace Earlz.MonoSump.Core
{
	public class SumpFlags
	{
		public bool Demux{get;set;}
		public bool Filter{get;set;}
		public bool[] ChannelGroups{get;private set;}
		public bool ExternalClock{get;set;}
		public bool InvertedClock{get;set;}
		public SumpFlags()
		{
			ChannelGroups=new bool[4];
		}
	}
}

