using System;
using JMEliAppMaui.Models;

namespace JMEliAppMaui.ViewModels
{
	[QueryProperty(nameof(Client), "ClientModel")]
	public class ClientDetailsViewModel : BindableObject
	{

        private ClientModel _clientUser;
        public ClientModel Client { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }

        public ClientDetailsViewModel()
		{
		}
	}
}

