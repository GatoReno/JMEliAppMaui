

using JMEliAppMaui.Services.Abstractions;

#if IOS
using Photos;
using Foundation;
#endif

#if ANDROID
using Android.Content;
using AndroidX.Core.App;
using Android.Content.PM;
#endif

namespace JMEliAppMaui.Services.Implementations
{
    public class FileService : IFileService
    {
        public bool FileExists(string ContractModelUrl)
        {
#if ANDROID
            string filename = GetFileName(ContractModelUrl);
            if (!string.IsNullOrEmpty(filename))
            {
                bool fileFound = false;
                string internalPath = GetFilePath(filename);
                var sdCardsPaths = GetAndroidSdCardsPaths();
                //check for SD cards
                if (sdCardsPaths != null)
                {
                    foreach (var sdCardPath in sdCardsPaths)
                    {
                        string sdPath = Path.Combine(sdCardPath, "Download", filename);
                        if (File.Exists(sdPath))
                        {
                            return true;
                        }
                    }
                }
                //If not found in SD card check internal storage
                if (!fileFound)
                {
                    if (File.Exists(internalPath))
                    {
                        return true;
                    }
                }
            }
            return false;

#elif WINDOWS
            string filename = GetFileName(ContractModelUrl);
            if (!string.IsNullOrEmpty(filename))
            {
                string path = GetFilePath(filename);
                if (File.Exists(path))
                {
                    return true;
                }
            }
            return false;
#elif IOS || MACCATALYST
            //implementation here

            string filename = GetFileName(ContractModelUrl);
            if (!string.IsNullOrEmpty(filename))
            {
                bool fileFound = false;
                string internalPath = GetFilePath(filename);

                if (File.Exists(internalPath))
                {
                    return true;
                }
            }
            return false;
#endif
        }

        public string GetWebviewUrl(string ContractModelUrl)
        {
            string fileName = GetFileName(ContractModelUrl);

            if (!string.IsNullOrEmpty(fileName))
            {
#if WINDOWS
                string path = GetFilePath(fileName);
                if (File.Exists(path))
                {
                    return path;
                }
                else
                {
                    return string.Empty;
                }

#elif ANDROID
                string url = string.Empty;
                string internalPath = GetFilePath(fileName);
                //check for SD cards
                var sdCardsPaths = GetAndroidSdCardsPaths();
                bool fileFound = false;
                if (sdCardsPaths != null)
                {
                    foreach (var sdCardPath in sdCardsPaths)
                    {
                        string sdPath = Path.Combine(sdCardPath, "Download", fileName);
                        if (File.Exists(sdPath))
                        {
                            url = string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///" + sdPath));
                            fileFound = true;
                            continue;
                        }
                    }
                }
                //If not found in SD card check internal storage
                if (!fileFound)
                {
                    if (File.Exists(internalPath))
                    {
                        url = string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///" + internalPath));
                    }
                }
                return url;
#elif IOS || MACCATALYST
                //implementation here
                return GetFilePath(fileName);
#endif
            }
            else
            {
                return string.Empty;
            }
        }

        static string GetFileName(string ContractModelUrl)
        {
            string fileName = string.Empty;
            var result = ContractModelUrl.Split("?alt");
            if (result != null && result.Count() > 1)
            {
                fileName = result[0].Replace("https://firebasestorage.googleapis.com/v0/b/joanmiroapp.appspot.com/o/StudentContrat%2F", "").Trim();
            }
            return fileName;
        }
        static string GetFilePath(string filename)
        {
#if WINDOWS
            string mainDir = FileSystem.Current.AppDataDirectory;
            string newDir = mainDir.Replace(@"AppData\Local\Packages\com.etho.jmeliappmaui_9zz4h110yvjzm\LocalState", "").Trim();
            return Path.Combine(newDir, "Downloads", filename);
#elif ANDROID
            return Path.Combine("storage/emulated/0/Download", filename);
#elif IOS || MACCATALYST
            //implementation here
            // Get the path to the Downloads folder

            var downloadsPath = NSSearchPath.GetDirectories(NSSearchPathDirectory.DownloadsDirectory, NSSearchPathDomain.User, true).FirstOrDefault();
            var personal= Environment.GetFolderPath(Environment.SpecialFolder.Personal);


            // Construct the full path to the PDF file
            var pdfFileName = filename;
            var fulldownloadpath = Path.Combine(downloadsPath, pdfFileName);
             // Check if the file exists
            if (!File.Exists(fulldownloadpath))
            {
                Console.WriteLine($"PDF file '{pdfFileName}' not found at '{fulldownloadpath}'.");
                
            }


            var Personalpath = Path.Combine(personal, pdfFileName);
            // Check if the file exists
            if (!File.Exists(Personalpath))
            {
                Console.WriteLine($"PDF file '{pdfFileName}' not found at '{Personalpath}'.");

            }

            var icloudDriveDownloadpath = "/private/var/mobile/Library/Mobile Documents/com~apple~CloudDocs/Downloads/";





            var f1 = File.Exists(fulldownloadpath) || File.Exists(Path.Combine(icloudDriveDownloadpath,filename));

            //var fileUrl = new NSUrl(pdfFilePath, false);
            //var request = new NSUrlRequest(fileUrl);
            //await webView.LoadRequestAsync(request);
            return Path.Combine(fulldownloadpath, filename);


          
#endif
        }



      

        public  bool AndroidNeedsPermission()
        {
#if IOS

            try
            {
                var status = PHPhotoLibrary.AuthorizationStatus;
                if (status == PHAuthorizationStatus.Denied
                    || status == PHAuthorizationStatus.NotDetermined
                    || status == PHAuthorizationStatus.Restricted)
                {
                    // Permission is denied or restricted
                    return true;
                }
                // Permission is granted //or not determined 
                return false;


            }
            catch (Exception ex)
            {
                // Handle exception if any
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
#else
            var check = Platform.AppContext.CheckSelfPermission("android.permission.READ_EXTERNAL_STORAGE");
            if (check == Permission.Denied)
            {
                return true;
            }
            return false;
 


#endif
        }
        public void AndroidRequestPermision()
        {
#if ANDROID
            if (AndroidNeedsPermission())
            {
                ActivityCompat.RequestPermissions(Platform.CurrentActivity, new[] { "android.permission.READ_EXTERNAL_STORAGE" }, 0);
                return;
            }
#elif IOS
            PHPhotoLibrary.RequestAuthorization(status =>
            {
                if (status == PHAuthorizationStatus.Authorized)
                {
                    // Permission granted
                    Console.WriteLine("Photo library permission granted.");
                }
                else
                {
                    // Permission denied or restricted
                    Console.WriteLine("Photo library permission denied or restricted.");
                }
            });
#endif
        }
#if ANDROID
        List<string> GetAndroidSdCardsPaths()
        {
            var context = Platform.AppContext?.ApplicationContext;
            List<string> list = new();
            try
            {

                var storageManager = (Android.OS.Storage.StorageManager)context.GetSystemService(Context.StorageService);

                var volumeList = (Java.Lang.Object[])storageManager.Class.GetDeclaredMethod("getVolumeList").Invoke(storageManager);
                foreach (var storage in volumeList)
                {
                    Java.IO.File info = (Java.IO.File)storage.Class.GetDeclaredMethod("getPathFile").Invoke(storage);

                    if ((bool)storage.Class.GetDeclaredMethod("isEmulated").Invoke(storage) == false && info.TotalSpace > 0)
                    {
                        list.Add(info.Path);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("======================Exception caught!===========================");
            }
            return list;
        }
#endif
    }
}
