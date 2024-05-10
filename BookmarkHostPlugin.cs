using Rocket.Core.Plugins;
using Steamworks;
using SDG.Unturned;
using System;
using Logger = Rocket.Core.Logging.Logger;
using IPv4Address = Unturned.SystemEx.IPv4Address;
using SDG.Framework.IO.Serialization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace Pandahut.Unturned.BookmarkHostPlugin
{
    public class BookmarkHostPlugin : RocketPlugin<BookmarkHostPluginConfiguration>
    {
        public static BookmarkHostPlugin Instance;
        protected override void Load()
        {
            Instance = this;

            if (!string.IsNullOrEmpty(Provider.configData.Browser.BookmarkHost))
            {
                try
                {
                    string fakeIPConnectionString;
                    string bookmarkHostURL = Provider.configData.Browser.BookmarkHost;
                    string serverGSLT = Provider.configData.Browser.Login_Token;
                    bool bCancel = false;

                    if (!BookmarkHostPlugin.Instance.Configuration.Instance.PluginEnabled)
                    {
                        Logger.Log("BookmarkHost Plugin is disabled! Plugin will not load.");
                        return;
                    }

                    if (!Provider.configData.Server.Use_FakeIP)
                    {
                        Logger.Log("Error: Cannot update Bookmark Host. Please enable the \"Use_FakeIP\" setting in the server's Config.json.");
                        bCancel = true;
                    }

                    if (string.IsNullOrEmpty(bookmarkHostURL))
                    {
                        Logger.Log("Error: Cannot update Bookmark Host. Please set the \"BookmarkHost\" property in the server's Config.json.");
                        bCancel = true; ;
                    }

                    if (string.IsNullOrEmpty(bookmarkHostURL))
                    {
                        Logger.Log("Error: Cannot update Bookmark Host. Please set the \"Login_Token\" property in the server's Config.json.");
                        bCancel = true; ;
                    }

                    if (bCancel)
                        return;

                    //Server errors out on the line below. "System.InvalidOperationException: Steamworks is not initialized."
                    SteamGameServerNetworkingSockets.GetFakeIP(0, out var pInfo);

                    if (pInfo.m_eResult == EResult.k_EResultBusy)
                    {
                        Logger.Log("Could not get server's Fake IP: Response status is busy.");
                    }

                    else if (pInfo.m_eResult == EResult.k_EResultOK)
                    {
                        string fetchedFakeIP = new IPv4Address(pInfo.m_unIP).ToString();
                        string fetchedFakePort = pInfo.m_unPorts[0].ToString();
                        fakeIPConnectionString = $"{fetchedFakeIP}:{fetchedFakePort}";

                        FakeIPServer fisData = new FakeIPServer(fakeIPConnectionString, serverGSLT);

                        Logger.Log($"Updating Bookmark Host with the following data: ConnString: {fakeIPConnectionString}, GSLT: {fisData.GSLT}");
                        _ = UpdateBookmarkHost(bookmarkHostURL, fisData);
                    }

                    else
                    {
                        Logger.Log($"Error updating Bookmark Host. Fatal result while fetching Fake IP data: {pInfo.m_eResult}");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }
        }

        public static async Task UpdateBookmarkHost(string URL, FakeIPServer Data)
        {
            try
            {
                var PostResponse = await SendHttpAsync(URL, Data);

                Logger.Log("Got response from Bookmark Host:");
                Logger.Log(PostResponse.ToString());

            }
            catch (Exception e)
            {
                Logger.LogException(e, "Exception encountered while updating Bookmark Host.");
            }
        }
        public static async Task<string> SendHttpAsync<TString>(string URL, TString SerializableData, JsonSerializerOptions Options = null)
        {
            try
            {
                JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { AllowTrailingCommas = true, PropertyNamingPolicy = null };
                var DATA = JsonSerializer.Serialize(SerializableData, Options);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(URL));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 600;
                request.ReadWriteTimeout = 300;

                using (Stream webStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
                {
                    using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        await requestWriter.WriteAsync(DATA).ConfigureAwait(false);
                    }
                }

                WebResponse webResponse = await request.GetResponseAsync().ConfigureAwait(false);
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        return await responseReader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }
            }

            catch (Exception e)
            {
                Logger.LogException(e, $"Exception encountered while sending HTTP data to Bookmark. URL: {URL}.");
                throw;
            }
        }
    }
}