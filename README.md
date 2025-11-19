# Qol Tweaks
## Installation
Download and unzip the zip file from the mod's [Releases](https://github.com/AlurienFlame/qol-tweaks/releases), and place the file in the `mods` folder of your Allumeria install. This mod depends on the [ignitron](https://github.com/danilwhale/ignitron) mod loader, and you should have created a `mods` folder while installing that.
## Features
- **Sort Hotkey**: Press `Mouse3` while in your inventory to sort it.
- **Quick stack to nearby chests hotkey**: Press `G` while not in any menus to quick stack to nearby chests.
## Develop
Build-and-run on linux: 
```
dotnet build -c Debug
cd ~/.local/share/Steam/steamapps/common/Allumeria\ Playtest/
wine "Allumeria.exe"
```
Package for distribution:
```
dotnet build -c Release
```