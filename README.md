# ProducerConsumerFramework
A basic producer consumer framework using C#. This can be used with both .NET framework and .NET Core.

The code in this project provides a basic framework to quickly set up a pipeline of producer consumer blocks.
The main inspiration behind the code was the need to build a stability testing tool that can track crashes and other situations when testing an application
over a large set of documents. A few things to note:
* This is just a starting point. I'm certain that there are many improvements possible on this code.
* The code attempts to provide a generic wrapper that implements queuing and synchronization when multiple processing blocks are chained together.

## Example use - a commandline task parallelizer for Windows
The example code contains a simple task parallelizer. 
* The parallelizer takes multiple commands and executes them in parallel (limited to 8 parallel threads/sub-processes).
* It can also take commands from a file - each line will be treated as a command. Any line starting with "[e]" will be considered as a direct call to an executable, otherwise calls are wrapped in a call to cmd.exe.
