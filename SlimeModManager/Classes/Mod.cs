using System.Collections.Generic;
using ModAssistant.Pages;

namespace ModAssistant
{
    public class Mod
    {
        public string name;
        public string author;
        public string description;
        public string version;
        public string git_path;
        public string group;
        public string download_url;
        public string[] dependencies;
        public List<string> downloadedFilePaths = new List<string>();
        public Mods.ModListItem ListItem;
        public List<Mod> Dependents = new List<Mod>();
    }
    /*
    public class Mod
    {
        public string name;
        public string version;
        public string gameVersion;
        public string _id;
        public string status;
        public string authorId;
        public string uploadedDate;
        public string updatedDate;
        public Author author;
        public string description;
        public string link;
        public string category;
        public DownloadLink[] downloads;
        public bool required;
        public Dependency[] dependencies;
        public List<Mod> Dependents = new List<Mod>();
        public Mods.ModListItem ListItem;

        public class Author
        {
            public string _id;
            public string username;
            public string lastLogin;
        }

        public class DownloadLink
        {
            public string type;
            public string url;
            public FileHashes[] hashMd5;
        }

        public class FileHashes
        {
            public string hash;
            public string file;
        }

        public class Dependency
        {
            public string name;
            public string _id;
            public Mod Mod;
        }
    }*/
}