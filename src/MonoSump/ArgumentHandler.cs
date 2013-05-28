using System;

namespace Earlz.MonoSump
{
	public class ArgumentParseException : ApplicationException
	{
		public ArgumentParseException(string message) : base(message)
		{
		}
	}
	public class ArgumentHandler
	{
		enum CurrentState
		{
			Bare, //bare (ready to accept options or device name)
			Trigger, //ready to accept N=V for trigger option
			Samples, //ready to accept N for sample count
			Frequency, //ready to accept F for sample frequency
			Config //ready to accept config file for --config
		}
		public ArgumentHandler()
		{
		}
		public Commands ParseCommands(string[] args)
		{
			if(args.Length==0)
			{
				PrintHelp();
				return null;
			}
			var commands=new Commands();
			var state=CurrentState.Bare;
			bool resetState=true;
			foreach(var arg in args)
			{
				resetState=true;
				if(state == CurrentState.Bare)
				{
					switch(arg)
					{
					case "--help":
						PrintHelp();
						return null;
					case "--identify":
						commands.Identify=true;
						break;
					case "--json":
						commands.JsonOut=true;
						break;
					case "--verbose":
						commands.Verbose=true;
						break;
					case "--trigger":
						state=CurrentState.Trigger;
						resetState=false;
						break;
					case "--frequency":
						state=CurrentState.Frequency;
						resetState=false;
						break;
					case "--samples":
						state=CurrentState.Samples;
						resetState=false;
						break;
					case "--config":
						state=CurrentState.Config;
						resetState=false;
						break;
					default:
						if(commands.DeviceName==null)
						{
							commands.DeviceName=arg;
						}
						else if(commands.DataOut==null)
						{
							commands.DataOut=arg;
						}
						else
						{
							throw new ApplicationException("unexpected extra argument after dataout");
						}
						break;
					}
				}
				else if(state == CurrentState.Trigger)
				{
					int n, v;
					try
					{
					n=int.Parse(arg.Split(new char[]{'='})[0]);
					v=int.Parse(arg.Split(new char[]{'='})[1]);
					}catch
					{
						throw new ArgumentParseException("--trigger parameter is wrong. Should look like `N=V` where N and V are integers, like --trigger 10=1");
					}
					commands.Triggers.Add(n, v);
				}
				else if(state==CurrentState.Frequency)
				{
					int f=0;
					if(!int.TryParse(arg, out f))
					{
						throw new ArgumentParseException("--frequency parameter is wrong. Should look like F where F is an integer, like --frequency 1000");
					}
					commands.Frequency=f;
				}
				else if(state==CurrentState.Samples)
				{
					int n=0;
					if(!int.TryParse(arg, out n))
					{
						throw new ArgumentParseException("--samples parameter is wrong. Should look like N where N is an integer, like --samples 100");
					}
					commands.Samples=n;
				}
				else if(state==CurrentState.Config)
				{
					commands.ConfigFile=arg;
				}
				else
				{
					throw new NotImplementedException("that's not suppose to happen :/");
				}




				if(resetState && state == CurrentState.Bare)
				{
					state=CurrentState.Bare;
				}
			}
			if(state!=CurrentState.Bare)
			{
				Console.WriteLine("Warning: probably an error parsing the last parameter on the command line");
			}
			if(commands.DeviceName==null)
			{
				throw new ArgumentParseException("No device name was given. See --help");
			}
			return commands;
		}
		public static void PrintHelp()
		{
			Console.WriteLine(
				@"monosump is a SUMP logic analyzer client for .Net
usage: monosump [options] devicename [dataoutput]

Options:

--help        #this screen
--identify    #get the device ID
--json        #output data to dataout in JSON
--verbose     #show extra info
--trigger N=V #Setup a stage-1 trigger at channel N for value V(1 or 0)
--samples N   #Get N number of samples
--frequency F #Set the sample frequency to F Hz
--config file #load a configuration file (this will overwrite all command line options except for --verbose and --json

Loading a configuration file will cause all command lien options to be ignored,
except for --verbose and --json

Because of the complexity of SUMP, only a basic subset of features is exposed in the command line
To get multi-stage triggers and more advanced configuration options, you must use a config file
For an example configuration file see https://github.com/Earlz/monosump/blob/master/exampleConfig.config
");


		}
	}
}

