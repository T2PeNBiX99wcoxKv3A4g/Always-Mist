# Always Mist

A mod let you enter [the mist area](https://hollowknightsilksong.wiki.fextralife.com/The+Mist) again

## What does this mod do?
* The mod will reopen [the mist area](https://hollowknightsilksong.wiki.fextralife.com/The+Mist) even you finish the area.
* Random the maze value make the mist more different.
* Have true always mist option can be turned on, then enter any room always enter the mist first.

## Installation
1. Download [BepInEx](https://github.com/BepInEx/BepInEx) and [install](https://docs.bepinex.dev/articles/user_guide/installation/index.html).
2. Download [BepinEx-Utils](https://github.com/T2PeNBiX99wcoxKv3A4g/BepinEx-Utils/releases/latest).
3. Extract all the .dll file to `game folder/BepInEx/plugins`
4. Launch game

## Configuration
The mod configuration file name is `io.github.ykysnk.AlwaysMist.cfg` inside `game folder/BepInEx/config`,
If you are not using any mod manager, you can manually change the value, also if you installed [BepinEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager)
you can change any values in game instead.

**The `TrueAlwaysMist` config will ignore `ResetMazeSaveData`, `RandomNeededCorrectDoors`, `RestBenchInMist` config values.
Also turn on the config, enter any doors or any room will enter the mist area first!**

## Known issue
* The mist area is only one side, after pass through the mist area you won't back to the mist last hall.
I'm lazy, So I won't fix this.
* When player pass through the mist area, enter again will exit immediately, until player death.
The can be easily fixed by turn on `ResetMazeSaveData` config, also turn on `TrueAlwaysMist` config will not have the issue.
* You can turn on `TrueAlwaysMist` config then start new game, but will be very painful if you don't have any abilities.
So recommend install some abilities unlocked mod.