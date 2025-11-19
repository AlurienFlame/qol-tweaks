# Qol Tweaks

### building

building in `Debug` configuration will automatically copy the mod to the 'mods/' directory of your game:

```
dotnet build -c Debug
```

but building in `Release` configuration won't copy it anywhere other than build folder (
`ExampleMod/bin/Release/net9.0/`):

```
dotnet build -c Release
```