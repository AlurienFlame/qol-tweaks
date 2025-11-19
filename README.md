# ignitron template mod

use this template to get started with developing your mod for [ignitron](https://github.com/danilwhale/ignitron)

### usage

1. create new repo from this template with `Use this template`
2. clone the new repo on your pc
3. [install ignitron](https://github.com/danilwhale/ignitron#installing)
4. [configure the workspace](https://github.com/danilwhale/ignitron#workspace-configuration)
5. make some changes to make it yours:
    1. rename the project and solution files to match your mod name
    2. update metadata file:
        1. modify `assembly_relative_path` property to `<PROJECT_NAME>.dll`
        2. edit remaining properties to match your mod
    3. replace `LICENSE` with license you would like to use, such as MIT
6. now you're ready to work on your mod

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