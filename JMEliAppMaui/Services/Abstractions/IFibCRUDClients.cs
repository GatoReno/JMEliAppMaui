using System;
using System.Collections.ObjectModel;
using JMEliAppMaui.Models;

namespace JMEliAppMaui.Services.Abstractions
{
	public interface IFibCRUDClients
	{
        Task<string> AddClient(ClientModel client);
        Task<ObservableCollection<ClientModel>> GetClients();
        Task DeleteClinet(string id);
        Task UpdateClient(ClientModel client);
    }
}

