
using JMEliAppMaui.Models;

namespace JMEliAppMaui.Services.Abstractions
{
    public interface IFileService
    {
        bool FileExists(string ContractModelUrl);
        string GetWebviewUrl(string url);
        void RequestPermision();
        bool NeedsPermission();
        public Task<string> CreatePdfAsync(ContractModel contract, Stream sign);
        public Task<string> UpdatePdfAsync(ContractModel contract, Stream sign);
        public Task<string> SaveAndView(string fileName, MemoryStream stream);
        public Task SaveAndView(string fileName, MemoryStream stream, OpenOption context, string contentType = "application/pdf");
    }
}
