using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
		[JsonProperty("stages")]
		public TriggerStageConfiguration[] TriggerSetup{get;private set;}
		[JsonProperty("readCount")]
		public int ReadCount{get;set;}
		[JsonProperty("delayCount")]
		public int DelayCount{get;set;}
		[JsonProperty("sampleFrequency")]
		public int SampleFrequency{get;set;}
		[JsonProperty("demux")]
		public bool Demux{get;set;}
		[JsonProperty("noiseFilter")]
		public bool Filter{get;set;}
		[JsonProperty("disabledGroups")]
		public IDictionary<int, bool> DisabledChannelGroups{get;private set;}
		[JsonProperty("externalClock")]
		public bool ExternalClock{get;set;}
		[JsonProperty("invertClock")]
		public bool InvertedClock{get;set;}

		public SumpConfiguration()
		{
			TriggerSetup=new TriggerStageConfiguration[4];
			DisabledChannelGroups=new Dictionary<int, bool>();
			ReadCount=10;
			DelayCount=10;
			SampleFrequency=1000;
			Demux=false;
			Filter=false;
			ExternalClock=false;
			InvertedClock=false;
		}
	}
	public class TriggerStageConfiguration
	{
		[JsonIgnore]
		public const int CHANNEL_COUNT=32;
		[JsonProperty("triggers")]
		public IDictionary<int, int> Values{get;private set;}
		[JsonProperty("delay")]
		public int Delay{get;set;}
		[JsonProperty("serialChannel")]
		public int? Channel{get;set;}
		[JsonProperty("level")]
		public int Level{get;set;}
		[JsonProperty("start")]
		public bool Start{get;set;}
		public TriggerStageConfiguration()
		{
			Values=new Dictionary<int, int>();
			Delay=0;
			Level=0;
			Channel=null;
			Start=false;
		}

	}
}

