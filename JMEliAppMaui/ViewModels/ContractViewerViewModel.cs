using JMEliAppMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

#if ANDROID
using Android.Content;
#endif

namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(ContractModel), "ContractModel")]
    public class ContractViewerViewModel : BaseViewModel, IQueryAttributable
    {
        private ContractModel _contract;
        public ContractModel Contract
        {
            get => _contract; set { _contract = value; OnPropertyChanged(); }
        }
        private string _fileUrl;
        public string FileUrl
        {
            get => _fileUrl; set { _fileUrl = value; OnPropertyChanged(); }
        }
        private UrlWebViewSource _AndroidfileUrl;
        public UrlWebViewSource AndroidFileUrl
        {
            get => _AndroidfileUrl; set { _AndroidfileUrl = value; OnPropertyChanged(); }
        }
        public ContractViewerViewModel() 
        {

        }

        public List<string> GetAndroidSdCardsPaths()
        {
#if ANDROID
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
#endif
            return null;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            string mainDir = FileSystem.Current.AppDataDirectory;
            if (query.ContainsKey(nameof(ContractModel)))
            {
                string fileName= string.Empty;
                ContractModel contract = query[nameof(ContractModel)] as ContractModel;
                if (contract != null)
                {
                    Contract = contract;
                    var result = Contract.Url?.Split("?alt");
                    if(result !=null && result.Count() > 1)
                    {
                        fileName = result[0].Replace("https://firebasestorage.googleapis.com/v0/b/joanmiroapp.appspot.com/o/StudentContrat%2F","").Trim();
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        Contract.Url = fileName;
                        string newDir = mainDir.Replace(@"AppData\Local\Packages\com.etho.jmeliappmaui_9zz4h110yvjzm\LocalState", "").Trim();
                        string path = Path.Combine(newDir, "Downloads", fileName);
                        FileUrl = path;


#if ANDROID
                        string newDir = mainDir.Replace("com.etho.jmeliappmaui/files", "").Trim();
                        string internalPath = Path.Combine("storage/emulated/0/Download", fileName);
                        //check for SD cards
                        var sdCardsPaths = GetAndroidSdCardsPaths();
                        bool fileFound = false;
                        if(sdCardsPaths != null)
                        {
                            foreach(var sdCardPath in sdCardsPaths)
                            {
                                string sdPath = Path.Combine(sdCardPath, "Download", fileName);
                                if (File.Exists(sdPath))
                                {
                                    FileUrl = $"file:///android_asset/pdfjs/web/viewer.html?file=file:///" + sdPath;
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

                                Android.Net.Uri? uri = Android.Net.Uri.FromFile(new Java.IO.File(internalPath));
                                string pdfFilePath = string.Format("file:///android_asset/web/viewer.html?file={0}", uri);
                                AndroidFileUrl = new UrlWebViewSource { Url = pdfFilePath };

                            }
                        }
#endif
                    }
                }
                OnPropertyChanged(nameof(ContractModel));
            }
        }
    }
}
