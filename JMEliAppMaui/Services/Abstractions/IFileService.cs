
namespace JMEliAppMaui.Services.Abstractions
{
    public interface IFileService
    {
        bool FileExists(string ContractModelUrl);
        string GetWebviewUrl(string ContractModelUrl);
        void AndroidRequestPermision();
        bool AndroidNeedsPermission();
    }
}
