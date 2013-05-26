MonoSump
========

A minimalistic sump logic analyzer with no dependencies that runs on Mono and .Net that is done right

# Usage

    monosump [options] devicename

For instance, on my linux box, I use it like so:

    monosump /dev/ttyUSB1

The command line is basically not developed at this point. Only the bare metal interface is considered "complete" right now


# Why?

There are a few other clients out there, even some that appear to work, why did I create my own? 

Basically, the other clients SUCK. Not just bad, but really bad. 

Of course, there is the official Java client... but after trying to get rxtx to work on my 3.x 64-bit linux kernel for 4 hours, I decided the protocol was easy enough to write my own client

There is another client called pyLogicAnalyzer that I've looked at. This one actually does work on my system, however it has a horribly buggy user interface

# How is this better?

First off, I'm sticking to .Net with absolutely no native code dependencies. It runs on Mono and .Net 4.0+. 

It's also the only client I've seen that is BSD licensed, so if you want to use this library in a commercial project, you easily can. 


# Where does it run?

The simple command line client should run everywhere. I actively test on Windows(.Net) and Linux(mono), including the Raspberry Pi. It should work everywhere there is a functioning SerialPort implementation in Mono. 

