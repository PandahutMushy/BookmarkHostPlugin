const http = require('http');
const fs = require('fs').promises;
const url = require('url');
const querystring = require('querystring');

const ListenerPORT = 3000;

async function readConfig() {
    try {
        const cfgFile = await fs.readFile('ServerCfg.json', 'utf8');
        return JSON.parse(cfgFile);
    } catch (err) {
        console.error("Error reading configuration:", err);
        throw new Error("Failed to load configuration");
    }
}

async function writeConfig(cfg) {
    try {
        await fs.writeFile('ServerCfg.json', JSON.stringify(cfg, null, 2), 'utf8');
    } catch (err) {
        console.error("Error writing configuration:", err);
        throw new Error("Failed to save configuration");
    }
}

const server = http.createServer(async (request, response) => {
    let cfg;

    try {
        cfg = await readConfig();
    } catch (error) {
        response.writeHead(500, { "Content-Type": "text/plain" });
        response.end(error.message);
        return;
    }

    if (request.method === 'GET') {
        const query = url.parse(request.url).query;
        const queryServer = querystring.parse(query).server;
        const connString = cfg[queryServer]?.ConnString || '';
        
        response.writeHead(200, { "Content-Type": "text/html" });
        response.end(connString);
        
    } else if (request.method === 'POST') {
        let body = '';

        request.on('data', chunk => body += chunk);
        
        request.on('end', async () => {
            try {
                const reqData = JSON.parse(body);
                const { GSLT: reqGSLT, ConnString: reqCS } = reqData;
                let srvShortName;

                for (const key in cfg) {
                    if (cfg[key].GSLT === reqGSLT) {
                        srvShortName = key;
                        delete cfg[key];
                        break;
                    }
                }

                if (!srvShortName) return response.end('Server not found');

                cfg[srvShortName] = { GSLT: reqGSLT, ConnString: reqCS };
                await writeConfig(cfg);

                response.writeHead(200, { "Content-Type": "text/plain" });
                response.end('OK');
            } catch (err) {
                response.writeHead(400, { "Content-Type": "text/plain" });
                response.end("Invalid data. Please ensure the data is in JSON format.");
            }
        });
    } else {
        response.writeHead(405, { "Content-Type": "text/plain" });
        response.end("Method not allowed");
    }
});

server.listen(ListenerPORT, () => {
    console.log("BookmarkHost server started on port", ListenerPORT);
});
