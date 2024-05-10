# BookmarkHostPlugin

## About
- This is a reference implementation of the BookmarkHost Plugin for Unturned servers.
- Its purpose is to update your Bookmark Host with your server's latest FakeIP connection information.
- When your Unturned server changes its IP address, clients who have bookmarked your server(s) in-game will still be able to connect to it.

## Setup:
- Add the BookmarkHost plugin's DLL file in your server's "Rocket\Plugins" directory.
- Set up your Bookmark Host. A reference implementation is available here:
- Enable the "Use_FakeIP" setting in your Unturned server's Config.json file.
- Enter your web listener's URL in the "BookmarkHost" property of your Unturned server's Config.json file.
- Enter your server's Steam Login Token (GSLT) in the "Login_Token" property of your Unturned server's Config.json file.

## Disclaimer:
- This project is purely instructional. It was designed to meet minimum operational requirements.
- Ideally, you will modify and expand on this implementation, remodeling it to your system's functional and security requirements.