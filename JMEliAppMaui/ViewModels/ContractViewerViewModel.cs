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
                if (contract != null && !string.IsNullOrEmpty(contract.Url))
                {
                    Contract = contract;
                    FileUrl = _fileService.GetWebviewUrl(Contract.Url);
                }
                OnPropertyChanged(nameof(ContractModel));
            }
        }
    }
}
