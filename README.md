# getReal3D Extensions

This repo contains the extensions to the getReal3D for the VEMS. The project itself is a playground for developing the extensions. 

## Installation
- Install the getReal3D unity pacakge. You'll have to request it from the UBCO Studios. Currently the getReal3D application only supports the following version of Unity.
  - ~~2019.2.11f1~~ (Currently this version doesn't work)
  - 2018.4.12f1
  - 2017.4.34f1
- Download the `VEMS_gr3d.unitypackage` package from the [releases on github](https://github.com/VEMS-ok/gr3d/releases) and add it to the project.
- Following instructions on the documentation provided with the getReal3D unity package to setup unity.

## Documentation
- Documentation of the extensions are available at [https://vems-ok.github.io/gr3d/](https://vems-ok.github.io/gr3d/).
- When the appliction is built, it connects to the server `vemslab.ok.ubc.ca` on port 80. During development, please do not use this server, as it may interfere with anyone using the VEMS space. Note that, when testing from editor, it would not allow connecting to this server.

## Examples
- A sample react application that acts as a remote input can be found in https://github.com/VEMS-ok/sample-react-remote-input
  This generates messages which are recieved by the `getRealRemoteInput`.
  A react app is hosted in https://vems-ok.github.io/sample-react-remote-input/

## Contributions
- See [CONTRIBUTING.md](CONTRIBUTING.md)
