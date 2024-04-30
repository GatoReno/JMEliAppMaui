using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels
{
	public class StatusPageViewModel : BindableObject 
    {
        #region props
        private bool  _isLoading,_isEdit,_isAdd,_isShow;
        private string _levelName, _Id, _selectedColor;
        private string _SelectedDeepColor, _SelectedStatusDescripsion, _StatusDescription,_statusName, _SelectedStatusName;

        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        public string SelectedStatusDescripsion
        {
            get => _SelectedStatusDescripsion; set
            {
                _SelectedStatusDescripsion = value; OnPropertyChanged();
            }
        }
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsAdd { get => _isAdd; set { _isAdd = value; OnPropertyChanged(); } }
        public bool IsShow { get => _isShow; set { _isShow = value; OnPropertyChanged(); } }

        public string Title { get => _levelName; set { _levelName = value; OnPropertyChanged(); } }
        
        public string SelectedStatusName { get => _SelectedStatusName; set { _SelectedStatusName = value; OnPropertyChanged(); } }
        public string StatusName { get => _statusName; set { _statusName = value; OnPropertyChanged(); } }
        public string StatusDescription { get => _StatusDescription; set { _StatusDescription = value; OnPropertyChanged(); } }

        public string SelectedColor { get => _selectedColor; set { _selectedColor = value; OnPropertyChanged(); } }
        public string SelectedDeepColor { get => _SelectedDeepColor; set { _SelectedDeepColor = value; OnPropertyChanged(); } }

        private IFibAddGenericService _fibAddGenericService;
        IFibStatusService _fibStatusService1;

        public ObservableCollection<StatusModel> StatusList { get; set; }
        public StatusModel SelectedStatus
        { get; set; }
        public ICommand DeleteStatusCommand { get; private set; }
        public ICommand AddStatusCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand DetailsStatusCommand { get; private set; }
        public ICommand SetSelectedColorCommand { get; private set; }

        #endregion
        public StatusPageViewModel(IFibAddGenericService fibAddGenericService, IFibStatusService fibStatusService)
		{
            this._fibStatusService1 = fibStatusService;
            this._fibAddGenericService = fibAddGenericService;
            StatusList = new ObservableCollection<StatusModel>();
              DeleteStatusCommand = new Command(OnDeleteStatusCommand);
              AddStatusCommand = new Command(OnAddStatusCommand);
              BackCommand = new Command(OnBackCommand);
              DetailsStatusCommand = new Command<StatusModel>(OnEditStatusCommand);
             SetSelectedColorCommand = new Command<string>(OnSetSelectedColorCommand);
            IsShow = true;
            IsEdit = false;
            StatusList = new ObservableCollection<StatusModel>();
            IsAdd = false;
            SelectedStatus = new StatusModel();
            Title = "Status";
            SelectedColor = "Purple";
            SelectedDeepColor = "MediumPurple";
            GetChilds();
        }

        private async void GetChilds()
        {
           StatusList.Clear();
           var statusList = await _fibStatusService1.GetStatus();
            if (statusList.Any())
            {
                foreach (var item in statusList)
                {
                    StatusList.Add(item);
                }
            }
        }

        private async void OnEditStatusCommand(StatusModel obj)
        {
            if (IsEdit)
            {
                IsLoading = true;
                obj = new StatusModel { Id = Id, Name = SelectedStatusName,
                    Descripsion = SelectedStatusDescripsion, Color = SelectedColor };
                await _fibAddGenericService.UpdateChild(obj, "Status", Id);
                OnBackCommand(null);
            }
            if (obj!=null)
            {
                SelectedStatusName = obj.Name;
                SelectedStatusDescripsion = obj.Descripsion;
                //SelectedColor with OnSetSelectedColorCommand
                OnSetSelectedColorCommand(obj.Color);
                Id = obj.Id;
                IsEdit = true;
                IsAdd = false;
                IsShow = false;

            }
            IsLoading = false;


        }

        public void OnSetSelectedColorCommand(string obj)
        {
            SelectedColor = obj;
            switch (obj)
            {
                case "IndianRed":
                    SelectedColor = "IndianRed";
                    SelectedDeepColor = "DarkRed";
                    break;
                case "Green":
                    SelectedDeepColor = "DarkGreen";
                    break;
                case "Gold":
                    SelectedDeepColor = "DarkGoldenrod";
                    break;
                default:
                    break;
            }
        }

        public void OnBackCommand(object obj)
        {
            IsAdd = false;
            IsEdit = false;
            IsShow = true;
            GetChilds();
        }

        
        private async void OnDeleteStatusCommand(object obj)
        {
            IsLoading = true;

            var request = await App.Current.MainPage.DisplayAlert("", "Are you sure you want to delete this Status", "ok", "cancel");
            if (request)
            {
                await _fibAddGenericService.DeleteChild(Id, "Status");
            }
            OnBackCommand(null);
            IsLoading = false;
        }

        public async void OnAddStatusCommand(object obj)
        {
            IsLoading = true;

            if (IsAdd)
            {
                if (string.IsNullOrEmpty(StatusDescription) ||
                     SelectedColor == "Purple"
                    || string.IsNullOrEmpty(StatusName))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "name , color and description needed", "ok");

                }
                else
                {

                    var modelStatus = new StatusModel()
                    {
                        Color = SelectedColor,
                        Name = StatusName,
                        Descripsion = StatusDescription
                    };

                    try
                    {
                        var id = await _fibAddGenericService.AddChild(modelStatus, "Status");

                        if (!string.IsNullOrEmpty(id.ToString()))
                        {
                            modelStatus.Id = id.ToString();
                            await _fibAddGenericService.UpdateChild(modelStatus, "Status", id.ToString());

                            await App.Current.MainPage.DisplayAlert("Success", $"Status {StatusName} added", "ok");
                            OnBackCommand(null);
                        }
                    
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message} {ex.Data}");
                    }
                }
            }
            else {
                IsAdd = true;
                IsEdit = false;
                IsShow = false;
            }

            IsLoading = false;
        }
    }
}

