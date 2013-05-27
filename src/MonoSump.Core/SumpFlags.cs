using System;

namespace Earlz.MonoSump.Core
{
	public class SumpFlags
	{
		public bool Demux{get;set;}
		public bool Filter{get;set;}
		public bool[] DisabledChannelGroups{get;private set;}
		public bool ExternalClock{get;set;}
		public bool InvertedClock{get;set;}
		public SumpFlags()
		{
			DisabledChannelGroups=new bool[4];
		}
	}
}

