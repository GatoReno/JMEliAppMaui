using JMEliAppMaui.ViewModels;
#if IOS
using Foundation;
#endif

namespace JMEliAppMaui.Views;

public partial class ContractViewerPage : ContentPage
{
    ContractViewerViewModel vm;
    public ContractViewerPage(ContractViewerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = vm =viewModel;

	}

	  void Config()
	{

#if IOS
        var fileUrl = new NSUrl(vm.FileUrl);
        var request = new NSUrlRequest(fileUrl);
       // Viewer.Source = fileUrl;
#endif

    }
}