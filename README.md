Rosie
================

Rosie is my First attempt at Home Automation.

Goals
====
* Secured
* Amazon Echo Support
* Z-Wave device Control
* Azure IoT Backend
* Run great on Raspberry Pi
* Plugable 
	* Additional Devices 
	* Logging Backends

Running
===== 
You can run the compiled Rosie.Server.exe on a Raspberry pie using "mono-service Rosie.Server.exe"

Debugging on the Raspberry pie via XS can be achieved by using (https://github.com/logicethos/SSHDebugger/)

Secrets.json
===
This is just a json file with Key Value Pairs. It's a place to put your API keys (src/Core/Rosie/Secrets.json)

`{
    "AzureIoTUrl":"http://...",
	"Foo":"bar"
}`


Future Plans
===========
* Home Kit Support (Maybe via https://github.com/nfarina/homebridge )
* Windows IoT Support
	* Sockets port
	* Http listener port