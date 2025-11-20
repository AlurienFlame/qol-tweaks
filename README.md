# Qol Tweaks
Mod for [Allumeria](https://store.steampowered.com/app/3516590/Allumeria/), using the [Ignitron](https://github.com/danilwhale/ignitron) mod loader.
## Installation
Download and unzip the zip file from the mod's [Releases](https://github.com/AlurienFlame/qol-tweaks/releases), and place the resulting folder in the `mods` folder of your Allumeria install. You should have created a `mods` folder when installing Ignitron.
## Features
- **Craft All**: Press `Shift+LMB` to craft as many as possible of a recipe.
### Hotkeys
- **Sort hotkey**: Press `Mouse3` while in your inventory to sort it.
- **Quick stack to nearby chests hotkey**: Press `G` while not in any menus to quick stack to nearby chests.
- **Quick torch hotkey**: Press `F` to place or throw a torch directly from your hotbar. Favors items further to the left. Will prioritize using placeable torches, but resort to using throwables if you're not looking at a block, or if you're out of placeable torches.
- **Quick heal hotkey**: Press `H` to use a health potion from your hotbar or inventory. Favors items in the hotbar, starting from the left, then the inventory, starting from the top left.
- **Quick buff hotkey**: Press `B` to use every buff potion in your inventory. Same priority as quick heal. Does not use healing potions or poison potions. Does use food.
All hotkeys can be rebound.
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