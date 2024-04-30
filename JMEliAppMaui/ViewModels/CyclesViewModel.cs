using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels
{
	public class CyclesViewModel : BindableObject
    {

        #region props
        private bool _isLoading, _isEdit, _isAdd, _isShow, _AddVisibility, _backVisibility;
        private string _CycleName, _Title,_Id;
        public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        public string CycleName { get => _CycleName; set { _CycleName = value; OnPropertyChanged(); } }


        public bool BackVisibility
        {
            get => _backVisibility; set
            {
                _backVisibility = value;
            }
        }
        public bool AddVisibility
        {
            get => _AddVisibility; set
            {
                _AddVisibility = value;
            }
        }
        
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsAdd { get => _isAdd; set { _isAdd = value; OnPropertyChanged(); } }
        public bool IsShow { get => _isShow; set { _isShow = value; OnPropertyChanged(); } }

        DateTime _InitDate, _EndDate;
        public DateTime InitDate { get => _InitDate; set { _InitDate = value; OnPropertyChanged(); } }
        public DateTime EndDate { get => _EndDate; set { _EndDate = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand EditCycleCommand { get; private set; }
        //
        public ICommand DeleteCycleCommand { get; private set; }

        public ObservableCollection<CycleModel> Cycles { get; set; }
        public CycleModel SelectedCycle { get; set; }

        private IFibAddGenericService _fibAddGenericService;
        IFibCyclesService _fibCyclesService1;
        #endregion

        public CyclesViewModel(IFibAddGenericService fibAddGenericService, IFibCyclesService fibCyclesService)
		{
            this._fibAddGenericService = fibAddGenericService;
            this._fibCyclesService1 = fibCyclesService;
            Cycles = new ObservableCollection<CycleModel>();
            OnBackCommand();
            SelectedCycle = new CycleModel();
            AddCommand = new Command(OnAddCommand);
            BackCommand = new Command(OnBackCommand);
            EditCycleCommand = new Command<CycleModel>(OnEditCycleCommand);
            DeleteCycleCommand = new Command(OnDeleteCycleCommand);
            BackVisibility = true;
            Title = "Cycles";
            InitDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        private async void OnDeleteCycleCommand(object obj)
        {
            IsLoading = true;
            var deletePromot = await App.Current.MainPage.DisplayAlert("Warning", $"Are you sure about delete {SelectedCycle.Name} ? ", "Ok","Cancel");

            if (deletePromot)
            {
                await _fibAddGenericService.DeleteChild(SelectedCycle.Id, "Cycles");
                OnBackCommand();
            }

            IsLoading = false;
        }

        private async void OnEditCycleCommand(CycleModel model)
        {
            IsLoading = true;
            AddVisibility = false;
            if (IsEdit)
            {
                SelectedCycle.Name = CycleName;
                SelectedCycle.EndDate = EndDate.ToString("");
                SelectedCycle.Id = Id;
                SelectedCycle.InitDate = InitDate.ToString("");
                await _fibAddGenericService.UpdateChild(SelectedCycle, "Cycles",SelectedCycle.Id);
                OnBackCommand();
            }
            else
            {
                CycleName = model.Name;
                SelectedCycle.Name = CycleName;
                SelectedCycle.EndDate = EndDate.ToString("");
                SelectedCycle.Id = model.Id;
                SelectedCycle.InitDate = InitDate.ToString("");


                IsAdd = false;
                IsEdit = true;
                IsShow = false;
            }
            IsLoading = false;


        }

        private void OnBackCommand()
        {
            IsAdd = false;
            IsEdit = false;
            IsShow = true;
            AddVisibility = true;
            GetChilds();
        }

        private async void OnAddCommand()
        {

            
            if (IsEdit)
            {
                return;
            }
            if (IsAdd)
            {
                if (EndDate == DateTime.Today || string.IsNullOrEmpty(CycleName))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "A name and end date not to be today to add a Cycle;", "ok");
                }
                else
                {
                    IsLoading = true;
                    var model = new CycleModel { Name = CycleName };

                    var id = await _fibAddGenericService.AddChild(model, "Cycles");

                    if (!string.IsNullOrEmpty(id.ToString()))
                    {
                        SelectedCycle.Name = CycleName;
                        SelectedCycle.InitDate = InitDate.ToString("");
                        SelectedCycle.EndDate = EndDate.ToString("");
                        SelectedCycle.Id = Id = id.ToString();
                        await _fibAddGenericService.UpdateChild(SelectedCycle, "Cycles", id.ToString());
                    }


                    IsShow = true;
                    IsAdd = false;
                    Cycles.Add(model);
                    IsLoading = false;
                    OnBackCommand();
                }
            }
            else
            {
                IsAdd = true;
                IsEdit = false;
                IsShow = false;
                CycleName = "";
            }
           
        }

        private async  void GetChilds()
        {


            IsLoading = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "need internet to procede, check your conectivity", "ok");

                return;
            }
            Cycles.Clear();
            var clients = await _fibCyclesService1.GetCycles();
            foreach (var item in clients)
            {
                Cycles.Add(item);
            }

            IsLoading = false;

        }
    }
}

