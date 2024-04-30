using System;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JMEliAppMaui.ViewModels
{
	public class BaseViewModel : BindableObject
	{
        #region props
        private bool _isLoading, _isEdit, _isAdd, _isShow, _DataFormVisibility, _IsLoadingRequierements;
        private string _levelName, _Id;

        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        public bool IsLoadingRequierements { get => _IsLoadingRequierements; set { _IsLoadingRequierements = value; OnPropertyChanged(); } }

        public bool DataFormVisibility { get => _DataFormVisibility; set { _DataFormVisibility = value; OnPropertyChanged(); } }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsAdd { get => _isAdd; set { _isAdd = value; OnPropertyChanged(); } }
        public bool IsShow { get => _isShow; set { _isShow = value; OnPropertyChanged(); } }

        public string Title { get => _levelName; set { _levelName = value; OnPropertyChanged(); } }

 

        public  IFibAddGenericService _fibAddGenericService;
 
        public ObservableCollection<StatusModel> StatusList { get; set; }
        public StatusModel SelectedStatus
        { get; set; }
        public ICommand DeleteCommand { get;   set; }
        public ICommand AddCommand { get;   set; }
        public ICommand BackCommand { get;   set; }
        public ICommand DetailsCommand { get;   set; }
        public ICommand EditCommand { get;   set; }
        public ICommand AppearingCommand { get; set; }

        #endregion
    }
}

