using System;
using System.Collections.Generic;

namespace Earlz.MonoSump.Core
{
	/*Proposed schema:
	 * 
	 * { 
	 * 	"ReadCount"=123,
	 *  "Delaycount..."=123,
	 * 
	 * "Triggers" = [
	 *   {
	 * 		"Values"=[ 1: 1, 5:0, 8:1 ];
	 * 		"Delay"=123.
	 *		//etc for rest of config
	 *   }, 
	 *   { //for all 4 stages of triggers
	 * 
	 *   }
	 * ]
	 * }
	 *
	 * 
	 * 
	 * 
	 * 
	 */
	public class SumpConfiguration
	{
		public TriggerStageConfiguration[] TriggerSetup{get;private set;}
		public int ReadCount{get;set;}
		public int DelayCount{get;set;}
		public int SampleFrequency{get;set;}

		public bool Demux{get;set;}
		public bool Filter{get;set;}
		public bool[] DisabledChannelGroups{get;private set;}
		public bool ExternalClock{get;set;}
		public bool InvertedClock{get;set;}

		public SumpConfiguration()
		{
			TriggerSetup=new TriggerStageConfiguration[4];
			DisabledChannelGroups=new bool[4];
		}
	}
	public class TriggerStageConfiguration
	{
		public const int CHANNEL_COUNT=32;
		public IDictionary<int, int> Values{get;private set;}
		public int Delay{get;set;}
		public int Channel{get;set;}
		public int Level{get;set;}
		public bool Start{get;set;}
		public bool Serial{get;set;}
		public TriggerStageConfiguration()
		{
			Values=new Dictionary<int, int>();
		}

	}
}

