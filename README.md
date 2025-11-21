# Qol Tweaks
Mod for [Allumeria](https://store.steampowered.com/app/3516590/Allumeria/), using the [Ignitron](https://github.com/danilwhale/ignitron) mod loader.
## Installation
Depends on Allumeria and Ignitron.

Download and unzip the zip file from the mod's [Releases](https://github.com/AlurienFlame/qol-tweaks/releases), and place the resulting folder in the `mods` folder of your Allumeria install.
## Features
- **Craft all**: Press `Shift+LMB` to craft as many as possible of a recipe.
- **Show block names**: When looking at a block or entity, its name will be displayed at the top of the screen. This can be disabled in the settings.
### Hotkeys
- **Sort hotkey**: Press `Mouse3` while in your inventory to sort it.
- **Quick stack to nearby chests hotkey**: Press `G` while not in any menus to quick stack to nearby chests.
- **Quick torch hotkey**: Press `F` to place or throw a torch directly from your hotbar. Will prioritize using placeable torches, but resort to using throwables if you're not looking at a block, or if you're out of placeable torches.
- **Quick heal hotkey**: Press `H` to use a health potion from your hotbar or inventory.
- **Quick buff hotkey**: Press `B` to use every buff potion in your hotbar or inventory. Does not use healing potions or poison potions. Does use food that provides a buff.
- **Recall hotkey**: Press `Y` to use a potion of returning from your inventory.

All hotkeys can be rebound.
Using items from the inventory favors items in the hotbar, starting from the left, then the inventory, starting from the top left.
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
Note that the resources folder won't properly copy over on Windows. If you want to contribute to the project on windows, either get into the habit of manually copying `res`, or update the build settings in `QolTweaks.csproj` to work differently depending on operating system.

### Decompiling the Game
The way I have my solution set up right now, it'll look for a folder called `GameDecompiled` containing the Allumeria source code. Either delete that line of `QolTweaks.slnx`, or give it what it's looking for. Here's one approach:
```bash
dotnet tool install -g ilspycmd
ilspycmd -p --nested-directories -o GameDecompiled "$ALLUMERIA_GAME_DIR/Allumeria.dll"
```