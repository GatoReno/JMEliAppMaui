using CommunityToolkit.Mvvm.ComponentModel;

#if ANDROID
using Android.Content;
using AndroidX.Core.App;
using Android.Content.PM;
#endif

namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(FileUrl), "FileUrl")]
    public partial class PdfViewerViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        string fileUrl;

        public PdfViewerViewModel()
        {

        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(nameof(FileUrl)))
            {
                string url = query[nameof(FileUrl)] as string;
                if (!string.IsNullOrEmpty(url))
                {
#if ANDROID
                    var check = Platform.AppContext.CheckSelfPermission("android.permission.READ_EXTERNAL_STORAGE");
                    if (check == Permission.Denied)
                    {
                        Console.WriteLine("======================Needs permission============================");
                        ActivityCompat.RequestPermissions(Platform.CurrentActivity, new[] { "android.permission.READ_EXTERNAL_STORAGE" }, 0);
                        return;
                    }
#endif
                    FileUrl = url;
                }
            }
        }
    }
}
