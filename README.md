It's still the work from https://github.com/andrewkirillov/AForge.NET
main changes: 
- replaced TargetFramework to support only the newest (currently .Net5), 64 bit.
- removed the multiple sub solutions. It's 1 solution now. Especially unit tests are not separated anymore, and can be run immediately.
A couple of (win)Forms are excluded from their projects, if just updating the namespaces didn't solve compile errors.
A few exception expecting unit tests are disabled for now, until I can check, what's the intension behind the test.
