using System;
using System.Collections.Generic;

namespace Earlz.MonoSump.Core
{
	public class SumpController
	{
		public ISumpCommander Sump{get;private set;}
		public bool Verbose;
		public SumpController(ISumpCommander sump, bool verbose)
		{
			Sump=sump;
			Verbose=verbose;
		}
		public IList<bool[]> Execute(SumpConfiguration config)
		{
			Sump.Reset();
			var flags=new SumpFlags
          	{
				Filter=config.Filter,
				Demux=config.Demux,
				ExternalClock=config.ExternalClock,
				InvertedClock=config.InvertedClock
			};
			for(int i=0;i<4;i++)
			{
				if(config.DisabledChannelGroups.ContainsKey(i) && config.DisabledChannelGroups[i]==true)
				{
					flags.DisabledChannelGroups[i]=true;
				}
			}
			Sump.SetFlags(flags);
			if(config.TriggerSetup.Length>4)
			{
				throw new NotSupportedException("It is not possible to have more than 4 trigger stages");
			}
			for(int i=0;i<config.TriggerSetup.Length;i++) 
			{
				if(config.TriggerSetup[i]==null)
				{
					continue;
				}
				//setup stages
				var stage=new TriggerConfiguration();
				stage.Channel=config.TriggerSetup[i].Channel ?? 0;
				stage.Delay=config.TriggerSetup[i].Delay;
				stage.Level=config.TriggerSetup[i].Level;
				stage.Serial=config.TriggerSetup[i].Channel.HasValue;
				stage.Start=config.TriggerSetup[i].Start;
				Sump.SetTriggerConfigurations(i, stage);

				bool[] mask=new bool[32];
				bool[] values=new bool[32];
				for(int j=0;j<32;j++)
				{
					var item=config.TriggerSetup[i].Values;
					//setup masks
					if(item.ContainsKey(j))
					{
						mask[j]=true;
						values[j]=item[j] > 0 ? true : false;
					}
				}
				Sump.SetTriggerMasks(i, mask);
				Sump.SetTriggerValues(i, values);
			}
			//setup sample count
			Sump.SetReadAndDelayCount(config.ReadCount/4, config.DelayCount/4);
			Sump.SetDivider(Sump.Clock / config.SampleFrequency);
			Sump.Run();
			return Sump.GetData(config.ReadCount/4, 100000, 500);
		}
	}
}

