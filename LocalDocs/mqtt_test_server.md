# Setting up local mqtt server for testing

When developing your game, if you are using `MessageClient` or `getRealRemoteInput`, you would use a local mqtt broker (server). When built, it would use the server setup at VEMS. You can use any broker. As an example, we show how to set up and use a [mosquitto broker](https://mosquitto.org) with the [`Player`](https://vems-ok.github.io/gr3d/md__local_docs_prefabs.html#autotoc_md6) prefab bundled with the gr3d extensions.

1. Install the mosquitto broker. Instructions for download/installation can be found here: https://mosquitto.org/download/
2. Open a terminal and run the following:
```sh
mosquitto -v -p 80
```

The above would run the server only on the local machine on port 80 (`-p 80`) in verbose mode (`-v`). 

If you are on **Windows OS**, you'd want to either add the install location to the `PATH` variable or change directory of the terminal (`cd`) to the location where you have the broker installed. For example:
```sh
cd 'C:\Program Files\mosquitto\'
.\mosquitto.exe -v -p 80
```

3. In unity, ensure you have the `MessageClient` as an active component in the scene. When using the `Player` prefab, it's in  `Player`>`InputManager` > `MessageInput`. Set the following:
   - `Test Server Address`: `127.0.0.1` 
   - `Test Port`: `80`
   - `Test Protocol`: `mqtt`
   

You should be able to play and test the server. 

Note that, the sample [react application](https://vems-ok.github.io/sample-react-remote-input/) will still work with the server configured to be only on the local machine. As the application entirely runs on the client, if you open the app on a broswer in the same machine the broker is running on, you can provide the following details and connect to the broker:
- `Set address`: `127.0.0.1`
- `Set port`: `80`

If you want any device connected to the same network as you to be able to connect to the broker, with the mosquitto broker, you have to have a configuration file. An example configuration file would be as follows:
```conf
listener 80
```

To have the broker run with this config file:
```conf
mosquitto -v -c <config file>
```
where `<config file>` would be the path to the configuration file.
