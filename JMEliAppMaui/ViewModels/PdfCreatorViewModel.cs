using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Popups;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Views;
using System.Diagnostics.Contracts;

namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(ContractModel), "ContractModel")]
    public partial class PdfCreatorViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IFileService _fileService;
        private readonly IMyPopupService _popupService;
        [ObservableProperty]
        string fileUrl;
        [ObservableProperty]
        ContractModel contract;
        public PdfCreatorViewModel(IFileService fileService,
                                   IMyPopupService popupService)
        {
            _fileService = fileService;
            _popupService = popupService;
        }

        [RelayCommand]
        public async Task Preview()
        {
            FileUrl = await _fileService.CreatePdfAsync(Contract,null);
            await Shell.Current.GoToAsync(nameof(PdfViewerPage), true,
            new Dictionary<string, object>
                 {
                    {"FileUrl", FileUrl }
                 });

        }
        [RelayCommand]
        public async Task Sign()
        {
            Popup signPopup = new SignPopup();
            var result = await _popupService.ShowPopupAsync(signPopup);
            if (result is Stream sign && result is not null)
            {
                FileUrl = await _fileService.CreatePdfAsync(Contract,sign);
                await Shell.Current.GoToAsync(nameof(PdfViewerPage), true,
               new Dictionary<string, object>
                {
                    {"FileUrl", FileUrl }
                });
            }
        }

        [RelayCommand]
        public async Task Update()
        {
            Popup signPopup = new SignPopup();
            var result = await _popupService.ShowPopupAsync(signPopup);
            if (result is Stream sign && result is not null)
            {
                FileUrl = await _fileService.UpdatePdfAsync(Contract, sign);
                await Shell.Current.GoToAsync(nameof(PdfViewerPage), true,
               new Dictionary<string, object>
                {
                    {"FileUrl", FileUrl }
                });
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(nameof(ContractModel)))
            {
                ContractModel contract = query[nameof(ContractModel)] as ContractModel;
                if (contract != null && !string.IsNullOrEmpty(contract.Url))
                {
                    Contract = contract;
                }
                OnPropertyChanged(nameof(ContractModel));
            }
        }
    }
}
