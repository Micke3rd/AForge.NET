It's still the work from https://github.com/andrewkirillov/AForge.NET

main changes: 
- removed build settings from *.csproj files
- updated references to support .Net5 (example see below).
- removed the multiple sub solutions. It's 1 solution now. Especially unit tests are not separated anymore, and can be run immediately.

A couple of (win)Forms are excluded from their projects, if just updating the namespaces didn't solve compile errors.
A few "exception expecting" unit tests are disabled for now, until I can check, what's the intension behind the test.
Excluded Robotics.Terk & QwerkStart because of not embedded (external) references

How du build:

I use AForge as submodule, and therefore dont wanna separat buildsettings for it. Build settings inherited from Directory.Build.props in my root folder.
If you want build AForge as it is here, create file Directory.Build.props beside file AForge.sln, with content:
<?xml version="1.0" encoding="utf-8" ?>
<Project>
  <PropertyGroup>
    <CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>
	  <TargetFramework>net5.0-windows</TargetFramework>
	  <Platforms>x64</Platforms>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
</Project>
