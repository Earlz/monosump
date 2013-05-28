MonoSump
========

A minimalistic [SUMP](http://www.sump.org/projects/analyzer/) logic analyzer client with no external dependencies that runs on Mono and .Net that is done the right way

# Download

You can download a precompiled binary release at http://earlz.net/static/monosump_1.0.zip

# Usage

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

    Loading a configuration file will cause all command line options to be ignored,
    except for --verbose and --json

    Because of the complexity of SUMP, only a basic subset of features is exposed in the command line
    To get multi-stage triggers and more advanced configuration options, you must use a config file
    For an example configuration file see https://github.com/Earlz/monosump/blob/master/exampleConfig.config

For instance, on my linux box, I can use it like so:

    monosump /dev/ttyUSB1

On Windows you should be able to use it like

    monosump COM1

# Json data output

For easy interoperability with other langauges, the API and command line client have built in JSON support. The json is quite simple and looks like so:

    {
      channels: [[0,1,0,1 ...], [0,0,0....], ...]
    }

Basically each value is the value of a channel, and each nested array is a single sample. At some point this might be extended to include useful info like sample frequency

# Plain data output

For ease of reading samples at a glance and for use with more primitive tools that don't support JSON, there is also the plain text data output. It looks like this:

    101011110010... 
    100000110011...

Each line is a sample and there are 32 rows of characters representing each channel. 1 is high, 0 is low, of course. 

# Config file

Because it'd be extremely difficult and complicated to implement a full SUMP interface with only command line arguments, I only expose the most useful and basic options.

If you want to setup multi-stage triggers, serial triggers, or have access to everything a SUMP analyzer can do, you need a configuration file. 

An example configuration file is at `/exampleConfig.config`

# Why reinvent the wheel?

There are a few other clients out there, even some that appear to work, why did I create my own? 

Basically, the other clients SUCK. Not just bad, but really bad. 

Of course, there is the official Java client... but after trying to get rxtx to work on my Linux 3.x 64-bit linux kernel for 4 hours, I decided the protocol was easy enough to write my own client

There is another client called pyLogicAnalyzer that I've looked at. This one actually does work on my system, however it has a horribly buggy user interface

# How is this better?

First off, I'm sticking to .Net with absolutely no native code dependencies. It runs on Mono and .Net 4.0+. 

It's also the only client I've seen that is BSD licensed, so if you want to use this library in a commercial project, you easily can. 

It also has a very clean separation between the SUMP protocol and it's user interface, so it's ideal if you want to implement your own client and is the only command line client I've seen.


# Supported platforms:

The simple command line client should run everywhere a full (with SerialPort) implementation of Mono runs. I test with the latest version of mono in git and mono 2.10.9

Tested platforms:

1. 64-bit PC running Linux
2. 64-bit PC running Windows 8
3. Raspberry Pi with Arch Linux

# What's broken

There is probably a lot more broken than I know of. I mean, this project didn't exist a week ago as of this writing. However, things seem to work OK at the moment, so who knows. 

There are a few things that are surely not implemented due to lack of test hardware and lack of protocol documentation

1. RLE (runtime length encoding)
2. Test Mode

# What's next

Going to work out bugs as I find them, but next up is probably going to be a simple ASP.Net web app to interface with this. 
This way, I can utilize all of the cross platform web technologies for building a real user interface