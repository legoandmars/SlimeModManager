using System.Collections.Generic;
using ModAssistant.Pages;
using System;
using System.Linq;
using System.Collections;

namespace ModAssistant
{
    /*public class Mod
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
    }*/

    public class ModVersion
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string version_number { get; set; }
        public string[] dependencies { get; set; }
        public string download_url { get; set; }
        public int downloads { get; set; }
        public DateTime date_created { get; set; }
        public string website_url { get; set; }
        public bool is_active { get; set; }
        public string uuid4 { get; set; }
        public int file_size { get; set; }
    }

    public class Mod
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public string owner { get; set; }
        public string package_url { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public string uuid4 { get; set; }
        public int rating_score { get; set; }
        public bool is_pinned { get; set; }
        public bool is_deprecated { get; set; }
        public bool has_nsfw_content { get; set; }
        public string[] categories { get; set; }
        public ModVersion[] versions { get; set; }
        public Mods.ModListItem ListItem;
        public List<string> downloadedFilePaths = new List<string>();
        public List<Mod> Dependents = new List<Mod>();

        public ModVersion LatestVersion
        {
            get
            {
                try
                {
                    var versionsList = versions.ToList();
                    var maxVersion = versionsList.Max(e => new Version(e.version_number));
                    var latestVersion = versionsList.First(e => new Version(e.version_number) == maxVersion);

                    return latestVersion;
                }
                catch
                {
                    return versions[0];
                }
            }
        }

        /*public string DependencyString
        {
            get
            {
                return $"{owner}-{name}-{LatestVersion.version_number}";
            }
        }*/

        public bool MatchesDependencyString(string dependencyString)
        {
            try
            {
                var ownerAndName = dependencyString.Substring(0, dependencyString.LastIndexOf("-"));
                var version = new Version(dependencyString.Substring(dependencyString.LastIndexOf("-") + 1));
                if (new Version(LatestVersion.version_number) >= version && ownerAndName.Trim() == $"{owner}-{name}") return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
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
