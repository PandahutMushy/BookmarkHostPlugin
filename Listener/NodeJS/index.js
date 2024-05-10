const http = require('http');
const fs = require('fs');
const url = require('url');
const querystring = require('querystring');
const cfgFile = fs.readFileSync('ServerCfg.json', 'utf8');
const cfg = JSON.parse(cfgFile);

const ListenerPORT = 3000;

var server = http.createServer(function(request, response) {
	//[GET] Fetch server connection string
	if (request.method == 'GET') {
		var query = url.parse(request.url).query;		
		var queryServer = querystring.parse(query).server;		
		response.writeHead(200, { "Content-Type": "text/html" });
		response.write(cfg[queryServer]?.ConnString || '');	
		response.end();		
		return;
	}	
	//[POST] Set server connection string
	else if (request.method == 'POST') {
        var body = '';		
        request.on('data', function (data) { body += data; });		
        request.on('end', function () {
            try {
				var reqData = JSON.parse(body);
				var reqGSLT = reqData['GSLT'];
				var reqCS = reqData['ConnString'];
				var srvShortName = "";

				// Only make changes if a server exists by the given GSLT
				for (const key in cfg) {
					if (cfg[key]['GSLT'] === reqGSLT) {
						// Get the server name for this GSLT
						srvShortName = key;
						// Remove entry with the same GSLT
						delete cfg[key];
						// We don't want to break here; remove duplicate GSLTs
					}
				}
				
				if (typeof srvShortName === 'undefined')  return;
				
				cfg[srvShortName] = {GSLT : reqGSLT, ConnString : reqCS};
				const encodedJsonData = JSON.stringify(cfg, null, 2);
				fs.writeFileSync('ServerCfg.json', encodedJsonData, 'utf8');
				response.writeHead(200, {"Content-Type": "text/plain"});
				response.write(encodedJsonData ? 'OK' : '');	
				response.end();
				return;
            }
			catch (err) {
              response.writeHead(500, {"Content-Type": "text/plain"});
              response.write("Bad Post Data. Please ensure your data is proper JSON.\n");
              response.end();
              return;
            }
        });
    }
});
server.listen(ListenerPORT);
console.log("Unturned BookmarkHost server started!")