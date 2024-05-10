using Rocket.API;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Pandahut.Unturned.BookmarkHostPlugin
{
    public class BookmarkHostPluginConfiguration : IRocketPluginConfiguration
    {
        public bool PluginEnabled;
        public void LoadDefaults()
        {
            PluginEnabled = true;
        }
    }
}
