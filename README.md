# getReal3D Extensions

This repo contains the extensions to the getReal3D for the VEMS.

## Installation
- Install the getReal3D unity pacakge. You'll have to request it from the UBCO Studios. Currently the getReal3D application only supports the following version of Unity.
  - ~~2019.2.11f1~~ (Currently this version doesn't work)
  - 2018.4.12f1
  - 2017.4.34f1
- Download the `VEMS_gr3d.unitypackage` package from the [releases on github](https://github.com/VEMS-ok/gr3d/releases) and add it to the project.
- Following instructions on the documentation provided with the getReal3D unity package to setup unity.

## Extensions in this project
### Prefabs
#### Player
The Player prefab is a modified version of the `GenericPlayer` prefab in the getReal3D package:
- Replaces the `GenericWandManager` with `CustomWandManager` which exposes events that can be subscribed to.
- Replaces the `GenericGrabbingWand` with `CustomGrabbingWand`which exposes events that can be subscribed to.
- Has the `WandPitchAxis` setup. You may need to configure the pitch axis in `getReal3D` > `Input Manager` for this to work.
- Has the `ColorBehaviour` setup for all wands, which changes the color of the wand to yellow when the wand button is pressed.
- Removes the example menu and components related to that.
### Scripts
#### `ColorBehaviour`
#### `CustomGrabbingWand`
#### `CustomWandManager`
#### `Interactables`
#### `WandPitchAxis`
#### `getRealMessageHandler`
