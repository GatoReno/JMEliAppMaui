using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(ContractModel), "ContractModel")]
    public class ContractViewerViewModel : BaseViewModel, IQueryAttributable
    {
        private ContractModel _contract;
        private string _fileUrl;
        private string _contractStatus = "";
        private string _contractType = "";

        public ContractModel Contract { get => _contract; set { _contract = value; OnPropertyChanged(); } }
        public string FileUrl { get => _fileUrl; set { _fileUrl = value; OnPropertyChanged(); } }
        public string ContractStatus { get => _contractStatus; set { _contractStatus = value; OnPropertyChanged(); } }
        public string ContractType { get => _contractType; set { _contractType = value; OnPropertyChanged(); } }

        public ICommand SaveCommand { get; }
        public ICommand ShareCommand { get; }

        private readonly IFileService _fileService;
        private string? _localFilePath;

        public ContractViewerViewModel(IFileService fileService)
        {
            _fileService = fileService;
            SaveCommand = new Command(OnSave);
            ShareCommand = new Command(OnShare);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(nameof(ContractModel)))
            {
                var contract = query[nameof(ContractModel)] as ContractModel;
                if (contract == null) return;

                Contract = contract;
                ContractStatus = contract.Status ?? "Sin estado";
                ContractType = contract.Type ?? "Contrato";

                // Write HTML to temp file for WebView
                if (!string.IsNullOrEmpty(contract.HtmlContent))
                {
                    var fileName = $"contract_{contract.Id ?? "temp"}.html";
                    _localFilePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                    await File.WriteAllTextAsync(_localFilePath, contract.HtmlContent);
                    FileUrl = _localFilePath;
                }
                else if (!string.IsNullOrEmpty(contract.Url))
                {
                    FileUrl = _fileService.GetWebviewUrl(contract.Url);
                    _localFilePath = contract.Url;
                }
            }
        }

        private async void OnSave()
        {
            if (string.IsNullOrEmpty(_localFilePath) || !File.Exists(_localFilePath)) return;

            try
            {
                // Save to app documents directory with proper name
                var docsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var safeName = $"{Contract?.Type ?? "Contrato"}_{Contract?.StudentName?.Replace(" ", "_") ?? "doc"}_{DateTime.Now:yyyyMMdd}.html";
                var destPath = Path.Combine(docsDir, safeName);

                File.Copy(_localFilePath, destPath, true);

                await Shell.Current.DisplayAlert("✅ Guardado", $"Contrato guardado en:\n{destPath}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
            }
        }

        private async void OnShare()
        {
            if (string.IsNullOrEmpty(_localFilePath) || !File.Exists(_localFilePath)) return;

            try
            {
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = $"Contrato - {Contract?.StudentName}",
                    File = new ShareFile(_localFilePath)
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo compartir: {ex.Message}", "OK");
            }
        }
    }
}
