<?php
error_reporting(E_ERROR | E_PARSE);
$method = $_SERVER['REQUEST_METHOD'];
$jsonRqDataPOST = file_get_contents('php://input');
$queryRequest = $_GET;
$cfgFile = file_get_contents('ServerCfg.json');
$cfg = json_decode($cfgFile, true);	

//[GET] Fetch server connection string
if ($method == "GET" && isset($queryRequest['server']) && $queryRequest['server'] != "") {	
	$queryServer = $queryRequest['server'];
	echo (isset($cfg[$queryServer])) ? $cfg[$queryServer]['ConnString'] : "";
}

//[POST] Set server connection string
else if ($method == "POST" && isset($jsonRqDataPOST)) {
	$reqData = json_decode($jsonRqDataPOST, true);
	if ($reqData != null && $reqData['GSLT'] != "" && $reqData['ConnString'] != "") {
		$reqGSLT = $reqData['GSLT'];
		$reqCS = $reqData['ConnString'];
		
		//Only make changes if a server exists by the given GSLT
		foreach ($cfg as $key => $value) {						
			if ($value['GSLT'] == $reqGSLT) {
				//Get the server name for this GSLT
				$srvShortName = $key;
				//Remove entry with the same GSLT
				unset($cfg[$key]);
				//We don't want to break here; remove duplicate GSLTs
			}			
		}
				
		if (!isset ($srvShortName)) return;		
		
		$cfg[$srvShortName]['GSLT'] = $reqGSLT;
		$cfg[$srvShortName]['ConnString'] = $reqCS;
		$encodedJsonData = json_encode($cfg, JSON_PRETTY_PRINT);
		$cfgFileWrite = fopen("ServerCfg.json", "w") or die("Unable to open file!");
		fwrite($cfgFileWrite, $encodedJsonData);
		fclose($cfgFileWrite);		
		echo "OK";
	}
}
?>