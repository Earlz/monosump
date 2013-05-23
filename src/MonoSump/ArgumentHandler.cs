using System;

namespace Earlz.MonoSump
{
	public class ArgumentHandler
	{
		public ArgumentHandler()
		{
		}
		public Config ParseCommands(string[] args)
		{
			var config=new Config();
			foreach(var arg in args)
			{
				switch(arg)
				{
				case "--identify":
					config.IdentifyOnly=true;
					break;
				default:
					if(config.DeviceName==null)
					{
						config.DeviceName=arg;
					}
					else
					{
						throw new ApplicationException("unexpected extra argument after devicename");
					}
					break;
				}
			}
			return config;
		}
	}
}

