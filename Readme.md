# Unturned BookmarkHost Server Plugin


## About
- This is a reference implementation of the BookmarkHost Plugin for Unturned servers. It currently supports [Rocket](https://github.com/SmartlyDressedGames/Legally-Distinct-Missile), but you're welcome to create another implementation of it!
- This project is free to use and open source.
- Its purpose is to update your Bookmark Host web listener with your server's latest FakeIP connection information.
- When your Unturned server changes its IP address, clients who have bookmarked your server(s) in-game will still be able to connect to it.

## Setup
- Add the BookmarkHost plugin's DLL file to your server's `Rocket\Plugins` directory.
- Set up the web listener for your Bookmark Host. A reference implementation is available in the [/Listener](https://github.com/PandahutMushy/BookmarkHostPlugin/tree/master/Listener) folder located in this project's root directory. 
- Enable the `Use_FakeIP` setting in your Unturned server's `Config.json` file.
- Enter your web listener's URL in the `BookmarkHost` property of your Unturned server's `Config.json` file.
- Enter your server's Steam Login Token (GSLT) in the `Login_Token` property of your Unturned server's `Config.json` file.

<br/>

# Unturned BookmarkHost Web Listener

## About
- **Important**: Project files are located in the [/Listener](https://github.com/PandahutMushy/BookmarkHostPlugin/tree/master/Listener) folder of this project's root directory. 
- This is a reference implementation of the BookmarkHost web listener for Unturned servers.
- Its purpose is to fetch and serve the latest connection information for your Unturned server(s).
- This is useful for Unturned servers using Unturned's FakeIP feature.
- When your Unturned server changes its IP address, clients who have bookmarked your server(s) in-game will still be able to connect to it.
- The listener can be configured to work with multiple Unturned servers.
- **Important**: Be sure to install a BookmarkHost plugin on your server, to update this web listener. A reference implementation can be found in this project's [root](https://github.com/PandahutMushy/BookmarkHostPlugin/tree/master) folder.

## Option #1: Setup (NodeJS)
- This web listener must be hosted on a **NodeJS** web server.
- To run locally, you must have NodeJS installed. Browse to its extracted folder and run the command `node index.js`.
- The server runs on Port 3000 by default. You can change this by altering the `ListenerPORT` line in the `index.js` file.
- On first use, update the web listener's configuration file `ServerCfg.json` to include a short name and the GSLT (Steam Login Token) for your server(s). Before saving, you might wish to run the contents of this file for proper Json formatting. A free, online Json validator can be found here: https://jsonlint.com/. 

## Option #2: Setup (PHP)
- **Important**: Be sure to install a BookmarkHost plugin on your server, to update this web listener. A reference implementation can be found in this project's [root](https://github.com/PandahutMushy/BookmarkHostPlugin/tree/master) folder.
- This web listener must be hosted on a **PHP** web server.
- On first use, update the web listener's configuration file `ServerCfg.json` to include a short name and the GSLT (Steam Login Token) for your server(s). Before saving, you might wish to run the contents of this file for proper Json formatting. A free, online Json validator can be found here: https://jsonlint.com/. 

## Usage
- Each server's root property is defined by its short/friendly name. For example, `server1`. Child properties include the server's Steam Login Token (`GSLT`) and connection string (`ConnString`).
- Each server's `ConnString` property will contain its `FakeIP:Port` connection string. This field is updated automatically by the BookmarkHost Plugin. For now, you can leave the default values in place.
- The web listener can be queried via a GET request using the "?server=" query string. Example: "http://your-domain.here/?server=<shortname>".
- Perform a test query to your web listener. Use your web browser to navigate to your web listener's URL and add the desired query string.
- Once successfully tested, enter the above URL in the `BookmarkHost` property of your Unturned server's `Config.json` file. For example, `BookmarkHost" = "http://mydomain.com/?server=server1`.
- You Unturned server(s) will use the above URL to update the web listener with its connection information every time the the server restarts or the plugin is loaded
- The Unturned Bookmarks list will query the above URL every time a player bookmarks your server or joins it using its bookmark.

<br/>

## Disclaimers
- This project is purely instructional. It was designed to meet minimum operational requirements.
- Ideally, you will modify and expand on this implementation, remodeling it to your system's functional and security requirements.