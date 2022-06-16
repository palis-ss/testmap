# testmap

Use ```WinMapGIS``` **x64**, just the ocx is fine.  Use the installer normally.  No need to run ```regsvr32```.

Run ```Aximp.exe``` on ```WinMapGIS.ocx``` under development command prompt to generate 2 DLLs
- AxWinMapGIS.dll
- WinMapGIS.dll

Craete a .NET 6 project

Reference both DLLs in the project

For now, we need to add the control to our form manually via code because VS2022 has become 64bit and it does not like the ocx for some reason.

Import and Use ```AxWinMapGIS``` as a control, which could be added to the form's control list.  Please see code how this is done.
