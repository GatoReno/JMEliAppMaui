using System;
using System.Windows.Input;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels
{
	public class ClientsViewModel : BindableObject
    {
        #region props
        private bool _searchOrPlus;
        public bool IsSearch { get => _searchOrPlus; set { _searchOrPlus = value; OnPropertyChanged(); }  }
        private bool _orPlus;
        public bool OrPlus { get => _orPlus; set { _orPlus = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; private set; }
        public ICommand AddBackCommand { get; private set; }
        #endregion

        #region add client props
        private string _fullname, _scholarity,_ocupation,_email,_phone,_office,_relationship,_work,_address;

        public string FullName { get => _fullname; set { _fullname = value; OnPropertyChanged(); } }
        public string Scholarity { get => _scholarity; set { _scholarity= value; OnPropertyChanged(); } }
        public string Ocupation { get => _ocupation; set { _ocupation = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email= value; OnPropertyChanged(); } }
        public string Phone { get => _phone; set { _phone= value; OnPropertyChanged(); } }
        public string Office { get => _office; set { _office= value; OnPropertyChanged(); } }
        public string Relationship { get => _relationship; set {_relationship= value; OnPropertyChanged(); } }
        public string Work { get => _work; set { _work = value; OnPropertyChanged(); } }
        public string Address { get => _address; set { _address= value; OnPropertyChanged(); } }
        #endregion

        private IFibCRUDClients _fibCRUDClients;
        public ClientsViewModel(IFibCRUDClients fibCRUDClients)
		{
			this._fibCRUDClients = fibCRUDClients;
            IsSearch = true;
            OrPlus = !IsSearch;
            AddCommand = new Command(OnAddCommand);
            AddBackCommand = new Command(OnAddBackCommand);

        }

        private void OnAddBackCommand(object obj)
        {
            IsSearch = true;
            OrPlus = false;
        }

        private void OnAddCommand()
        {
            IsSearch = false;
            OrPlus = true;
        }
    }
}

