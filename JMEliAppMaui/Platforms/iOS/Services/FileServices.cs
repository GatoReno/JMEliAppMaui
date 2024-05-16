using Foundation;
using JMEliAppMaui.Services.Abstractions;
using QuickLook;
using UIKit;

namespace JMEliAppMaui.Services;

public class FileServices : IGetAsyncFileService
{
    public async Task SaveAndView(string fileName, MemoryStream stream,  OpenOption context, string contentType = "application/pdf")
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string filePath = Path.Combine(path, fileName);

        FileStream fileStream = File.Open(filePath, FileMode.Create);
        stream.Position = 0;

        await stream.CopyToAsync(fileStream);
        await fileStream.FlushAsync();
        fileStream.Close();

        UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
        while (currentController.PresentedViewController != null)
            currentController = currentController.PresentedViewController;

        UIView currentView = currentController.View;
        QLPreviewController qLPreview = new QLPreviewController();
        QLPreviewItem item = new QLPreviewItemBundle(fileName, filePath);
        qLPreview.DataSource = new PreviewControllerDS(item);
        currentController.PresentViewController(qLPreview, true, null);
    }
}

public class PreviewControllerDS : QLPreviewControllerDataSource
{
    private QLPreviewItem _item;
    public PreviewControllerDS(QLPreviewItem item)
    {
        _item = item;
    }


    public override nint PreviewItemCount(QLPreviewController controller)
    {
        return 1;
    }

    public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
    {
        return _item;
    }
}


public class QLPreviewItemBundle : QLPreviewItem
{
    private string _fileName, _filePath;
    public QLPreviewItemBundle(string fileName, string filePath)
    {
        _fileName = fileName;
        _filePath = filePath;
    }

    public override string PreviewItemTitle => _fileName;
    public override NSUrl PreviewItemUrl
    {
        get
        {
            var documents = NSBundle.MainBundle.BundlePath;
            var lib = Path.Combine(documents, _filePath);
            var url = NSUrl.FromFilename(lib);
            return url;
        }
    }
}

public class QLPreviewItemFileSystem : QLPreviewItem
{
    string _fileName, _filePath;
    public QLPreviewItemFileSystem(string fileName, string filePath)
    {
        _fileName = fileName;
        _filePath = filePath;
    }

    public override string PreviewItemTitle => _fileName;

    public override NSUrl PreviewItemUrl => NSUrl.FromString(_filePath);

}