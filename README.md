# testmap

Use ```MapWinGIS``` **x64**, just the ocx is fine.  Use the installer normally.  No need to run ```regsvr32``` for now because we are not going to use the ocx directly. That is, as of **MapWinGIS v5.3.0**, we need to add the COM object to our program via DLLs because VS2022 has become 64-bit and it does not like the ocx for some reason. So, 

Run ```Aximp.exe``` on ```MapWinGIS.ocx``` under development command prompt to generate 2 DLLs
- AxMapWinGIS.dll
- MapWinGIS.dll

Create a project as .NET or .NET framework, then reference both DLLs in the project.  You may need to point to location where the DLLs were generated previously.

In project Settings->Build options, choose either ```Any CPU``` or ```x64```.  Untick ```Prefer 32-bit```.

To load the map in a form, we need to do it programmatically.
First, add the following namespaces
```
using AxMapWinGIS;
using MapWinGIS;
```
Use ```AxMapWinGIS.AxMap``` as the main map control. That is, create a class member and add its instance to the form's (or panel's) control list.  Please see code how this is done.
