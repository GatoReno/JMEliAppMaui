using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

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

        private readonly IFileService _fileService;

        public ContractViewerViewModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(nameof(ContractModel)))
            {
                ContractModel contract = query[nameof(ContractModel)] as ContractModel;
                if (contract == null) return;

                Contract = contract;

                // If contract has HtmlContent (new system), write to temp file
                if (!string.IsNullOrEmpty(contract.HtmlContent))
                {
                    var fileName = $"contract_{contract.Id ?? "temp"}.html";
                    var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                    await File.WriteAllTextAsync(filePath, contract.HtmlContent);
                    FileUrl = filePath;
                }
                // If contract has a URL (legacy/Firebase Storage), use file service
                else if (!string.IsNullOrEmpty(contract.Url))
                {
                    FileUrl = _fileService.GetWebviewUrl(contract.Url);
                }
            }
        }
    }
}
