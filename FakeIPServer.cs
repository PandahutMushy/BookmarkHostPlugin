using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandahut.Unturned.BookmarkHostPlugin
{
    public class FakeIPServer
    {
        public FakeIPServer()
        {
            ConnString = "";
            GSLT = "";
        }
        public FakeIPServer(string connString, string gSLT)
        {
            ConnString = connString;
            GSLT = gSLT;
        }

        public string ConnString { get; set; }
        public string GSLT { get; set; }
    }
}
