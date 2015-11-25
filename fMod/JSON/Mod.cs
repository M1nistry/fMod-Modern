using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fMod.JSON
{
    public class Mod
    {
        public int id { get; set; }
        public string url { get; set; }
        public List<string> categories { get; set; }
        public string author { get; set; }
        public string contact { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Release> releases { get; set; }
    }

    public class File
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mirror { get; set; }
        public string url { get; set; }
    }

    public class Release
    {
        public int id { get; set; }
        public string version { get; set; }
        public string released_at { get; set; }
        public List<object> game_versions { get; set; }
        public List<object> dependencies { get; set; }
        public List<File> files { get; set; }
    }
}
