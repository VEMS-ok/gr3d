# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v0.0.3] - 2023-04-19
### Added
- Documentation on:
  - Setting up local servers
  - trackd gamepad mapping
- Github action to generate unitypackage with only Utils

### Changed
- Prevent connecting to VEMS server from editor
- Moved MQTTnet plugin to `VEMS.Utils`

## [v0.0.2] - 2023-02-23
### Added
- `MessageClient` - Handling mqtt messages
- `WandManager` interacts with `Interactable`
- `getRealCombinedInput` combining multiple `PlayerInputs`
- Setting up documentation generating with doxygen

### Changed
- renamed `getRealMessageHandler` to `getRealRemoteInput`
- Moved the mqtt logic to a seperate class (`MessageClient`)

### Fixed
- Variables shadowed in extended classes (These don't need to be extended?)

## [v0.0.1] - 2023-02-09
### Added
- New/modifed MonoBehaviours:
  - `ColorBehaviour`
  - `CustomGrabbingWand`
  - `CustomWandManager`
  - `WandPitchAxis`

- Expreimental implementations:
  - `Interactables`
  - `getRealMessageHandler`
  
- Created new `Player` prefab, with irrelevant components removed and new behaviours added.
