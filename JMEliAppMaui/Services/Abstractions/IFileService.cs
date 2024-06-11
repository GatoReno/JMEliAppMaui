
using JMEliAppMaui.Models;

namespace JMEliAppMaui.Services.Abstractions
{
    public interface IFileService
    {
        bool FileExists(string ContractModelUrl);
        string GetWebviewUrl(string ContractModelUrl);
        void AndroidRequestPermision();
        bool AndroidNeedsPermission();
        public Task<string> CreatePdfAsync(ContractModel contract, Stream sign);
        public Task<string> UpdatePdfAsync(ContractModel contract, Stream sign);
    }
}
