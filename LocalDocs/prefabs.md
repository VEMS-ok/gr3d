# Prefabs
## Player
The Player prefab is a modified version of the `GenericPlayer` prefab in the getReal3D package:
- Replaces the `GenericWandManager` with `CustomWandManager` which exposes events that can be subscribed to and also can interact with any component that has `Interactable` attached to them.
- Replaces the `GenericGrabbingWand` with `CustomGrabbingWand`which exposes events that can be subscribed to.
- Has the `WandPitchAxis` setup, which uses the pitch axis to pitch the wand up and down. You may need to configure the pitch axis in `getReal3D` > `Input Manager` for this to work.
- Has the `ColorBehaviour` setup for all wands, which changes the color of the wand to yellow when the wand button is pressed.
- Has the `getRealCombinedInput` configured to work with `getRealRemoteInput` and the default `getReal3DPlayerInput` that is bundled with the `getReal3d` pacakge. (components under the game object `Player`>`InputManager`).
- Has the the `MessageClient` setup in  `Player`>`InputManager` > `MessageInput`. For local testing, setup the `testServerAddress`, `testPort` and `testProtocol`. When built, they will use preconfigured values for the vems servers. See docuemntation of `MessageClient` for more information.
- Removes the example menu and components related to that.
