using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels
{
	public class ContractsViewModel : BindableObject
	{
        #region props
        private bool _isLoading, _isEdit, _isAdd, _isShow;
        private string _contractName, _Id;

        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }
        public string ContractName { get => _contractName; set { _contractName = value; OnPropertyChanged(); } }
        public ObservableCollection<ContractTypeModel> ContractList { get; set; }
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsAdd { get => _isAdd; set { _isAdd = value; OnPropertyChanged(); } }
        public bool IsShow { get => _isShow; set { _isShow = value; OnPropertyChanged(); } }

        public ICommand DeleteContractCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand DetailsContractCommand { get; private set; }

        #endregion

        private IFibAddGenericService _fibAddGenericService;
        IFibContract _fibContractService;

        public ContractsViewModel(IFibAddGenericService fibAddGenericService, IFibContract fibContractService)
		{
            this._fibAddGenericService = fibAddGenericService;
            this._fibContractService = fibContractService;
            ContractList = new ObservableCollection<ContractTypeModel>();
            DeleteContractCommand = new Command(OnDeleteContractCommand);
            AddCommand = new Command(OnAddContractCommand);
            BackCommand = new Command(OnBackCommand);
            DetailsContractCommand = new Command<ContractTypeModel>(OnEditContractCommand);
            OnBackCommand();
        }

        private void OnEditContractCommand(ContractTypeModel model)
        {
            
        }

        private void OnBackCommand()
        {
            IsAdd = false;
            IsEdit = false;
            IsShow = true;
            GetChilds();
        }

        private async void GetChilds()
        {
            IsLoading = true;
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                ContractList.Clear();
                var contracts = await _fibContractService.GetContracts();
                if (contracts.Count()>0)
                {
                    foreach (var item in contracts)
                    {
                        ContractList.Add(item);
                    }
                }
            }
            IsLoading = false;
        }

        private async void OnAddContractCommand()
        {
            if (IsEdit)
            {
                return;
            }
            if (IsAdd)
            {
                IsLoading = true;
                if (!string.IsNullOrEmpty(ContractName))
                {
                    var newContract = new ContractTypeModel { Name = ContractName };

                    await _fibAddGenericService.AddChild(newContract, "ContractType");
                    OnBackCommand();
                    ContractName = string.Empty;
                }
               


                IsLoading = false;
            }
            else {
                IsAdd = true;
                IsEdit = false;
                IsShow = false;
            }
        }

        private void OnDeleteContractCommand(object obj)
        {

        }
    }
}

