# Unturned BookmarkHost Web Listener (PHP)


## About
- This is a reference implementation of the BookmarkHost web listener for Unturned servers.
- Its purpose is to fetch and serve the latest connection information for your Unturned server(s).
- This is useful for Unturned servers using Unturned's FakeIP feature.
- When your Unturned server changes its IP address, clients who have bookmarked your server(s) in-game will still be able to connect to it.
- The listener can be configured to work with multiple Unturned servers.
- **Important**: Be sure to install a BookmarkHost plugin on your server, to update this web listener. A reference implementation can be found in this project's [root](https://github.com/PandahutMushy/BookmarkHostPlugin/tree/master) folder.
<br/>

## Setup
- This web listener must be hosted on a **PHP** web server.
- On first use, update the web listener's configuration file `ServerCfg.json` to include a short name and the GSLT (Steam Login Token) for your server(s). 
- Before saving, you may wish to run the contents of this file for proper Json formatting. A free, online Json validator can be found here: https://jsonlint.com/. 
<br/>

## Usage
- Each server's root property is defined by its short/friendly name. Example: `server1`. Child properties include the server's Steam Login Token (`GSLT`) and connection string (`ConnString`).
- Each server's `ConnString` property will contain its `FakeIP:Port` connection string. This field is updated automatically by the BookmarkHost Plugin. For now, you can leave the default values in place.
- The web listener can be queried via a GET request using the "?server=" query string. Example: `http://your-domain.here/?server=<shortname>`.
- Perform a test query to your web listener. Use your web browser to navigate to your web listener's URL and add the desired query string.
- You should see a blank webpage containing only your Unturned server's connection string. Example: `192.168.0.1:54321`.
- Once successfully tested, enter the above URL in the `BookmarkHost` property of your Unturned server's `Config.json` file. Example: `BookmarkHost" = "http://mydomain.com/?server=server1`.
- You Unturned server(s) will use the above URL to update the web listener with its connection information every time the the server restarts or the plugin is loaded
- The Unturned Bookmarks list will query the above URL every time a player bookmarks your server or joins it using its bookmark.
<br/>

## Disclaimers
- This project is purely instructional. It was designed to meet minimum operational requirements.
- Ideally, you will modify and expand on this implementation, remodeling it to your system's functional and security requirements.