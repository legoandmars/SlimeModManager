using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using static ModAssistant.Http;

namespace ModAssistant.API
{
    public class Utils
    {
        public static readonly string BeatSaberPath = App.BeatSaberInstallDirectory;

        public static void SetMessage(string message)
        {
            if (App.OCIWindow != "No")
            {
                if (App.window == null)
                {
                    if (App.OCIWindow == "No") OneClickStatus.Instance = null;
                    if (OneClickStatus.Instance == null) return;

                    OneClickStatus.Instance.MainText = message;
                }
                else
                {
                    MainWindow.Instance.MainText = message;
                }
            }
        }

        public static async Task DownloadAsset(string link, string folder, bool showNotifcation, string fileName = null)
        {
            await DownloadAsset(link, folder, fileName, null, showNotifcation);
        }

        public static async Task DownloadAsset(string link, string folder, string fileName = null, string displayName = null)
        {
            await DownloadAsset(link, folder, fileName, displayName, true);
        }

        public static async Task DownloadAsset(string link, string folder, string fileName, string displayName, bool showNotification, bool beatsaver = false)
        {
            if (string.IsNullOrEmpty(BeatSaberPath))
            {
                ModAssistant.Utils.SendNotify((string)Application.Current.FindResource("OneClick:InstallDirNotFound"));
            }
            try
            {
                Directory.CreateDirectory(Path.Combine(BeatSaberPath, folder));
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = WebUtility.UrlDecode(Path.Combine(BeatSaberPath, folder, new Uri(link).Segments.Last()));
                }
                else
                {
                    fileName = WebUtility.UrlDecode(Path.Combine(BeatSaberPath, folder, fileName));
                }
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = Path.GetFileNameWithoutExtension(fileName);
                }

                if (beatsaver) await BeatSaver.Download(link, fileName);
                else await ModAssistant.Utils.Download(link, fileName);

                if (showNotification)
                {
                    SetMessage(string.Format((string)Application.Current.FindResource("OneClick:InstalledAsset"), displayName));
                }
            }
            catch
            {
                SetMessage((string)Application.Current.FindResource("OneClick:AssetInstallFailed"));
                App.CloseWindowOnFinish = false;
            }
        }

        public static List<Mod> GetModDependencies(Mod mod, Mod[] ModsList)
        {
            var depStrings = mod.LatestVersion.dependencies;
            var foundDeps = new List<Mod>();

            for (int i = 0; i < depStrings.Length; i++)
            {
                for (int j = 0; j < ModsList.Length; j++)
                {
                    if (ModsList[j].MatchesDependencyString(depStrings[i])) // might break when versions start changing, fix
                    {
                        foundDeps.Add(ModsList[j]);
                        foundDeps.AddRange(GetModDependencies(ModsList[j], ModsList));
                        break;
                    }
                }
            }

            //if (foundDeps.Count != depStrings.Length) throw new Exception("Depenencies couldn't be met");
            return foundDeps;
        }

        public static async Task DownloadModWithDependencies(Uri uri)
        {
            try
            {
                var splitURI = uri.ToString().Split('/');

                var version = splitURI[splitURI.Length - 2];
                var name = splitURI[splitURI.Length - 3];
                var author = splitURI[splitURI.Length - 4];
                var url = splitURI[splitURI.Length - 5];

                if (url.ToLower() != "nasb.thunderstore.io") throw new Exception("Non-NASB URL"); // remove?
                var modAPIURL = $"https://{url}/api/v1/package/";

                var resp = await HttpClient.GetAsync(modAPIURL);
                var body = await resp.Content.ReadAsStringAsync();
                var ModsList = JsonSerializer.Deserialize<Mod[]>(body);

                var mod = ModsList.Where(e => e.name == name && e.owner == author).FirstOrDefault();

                var modsToGet = GetModDependencies(mod, ModsList);
                modsToGet.Add(mod);

                var modHandler = new Pages.Mods();

                foreach (var modToGet in modsToGet.Distinct())
                {
                    await DownloadMod(modToGet, modHandler);
                }
                    
                App.CloseWindowOnFinish = true;
            }
            catch (Exception e)
            {
                SetMessage(e.ToString());
                SetMessage((string)Application.Current.FindResource("OneClick:AssetInstallFailed"));
                App.CloseWindowOnFinish = false;
            }
        }

        //public static async Task DownloadMod(string link, string folder, string fileName, string displayName, bool showNotification, bool beatsaver = false)
        public static async Task DownloadMod(Mod mod, Pages.Mods modHandler)
        {
            if (string.IsNullOrEmpty(BeatSaberPath))
            {
                ModAssistant.Utils.SendNotify((string)Application.Current.FindResource("OneClick:InstallDirNotFound"));
            }
            try
            {
                await modHandler.InstallMod(mod, Path.Combine(App.BeatSaberInstallDirectory)); // what

                SetMessage(string.Format((string)Application.Current.FindResource("OneClick:InstalledAsset"), mod.name.Replace('_', ' ')));
                /*
                Directory.CreateDirectory(Path.Combine(BeatSaberPath, folder));
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = WebUtility.UrlDecode(Path.Combine(BeatSaberPath, folder, new Uri(link).Segments.Last()));
                }
                else
                {
                    fileName = WebUtility.UrlDecode(Path.Combine(BeatSaberPath, folder, fileName));
                }
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = Path.GetFileNameWithoutExtension(fileName);
                }

                if (beatsaver) await BeatSaver.Download(link, fileName);
                else await ModAssistant.Utils.Download(link, fileName);

                if (showNotification)
                {
                    SetMessage(string.Format((string)Application.Current.FindResource("OneClick:InstalledAsset"), displayName));
                }*/
            }
            catch(Exception e)
            {
                SetMessage((string)Application.Current.FindResource("OneClick:AssetInstallFailed"));
                App.CloseWindowOnFinish = false;
            }
        }
    }
}
