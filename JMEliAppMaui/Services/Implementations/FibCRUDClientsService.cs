using System;
using System.Collections.ObjectModel;
using Firebase.Database;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.Services.Implementations
{
	public class FibCRUDClientsService : IFibCRUDClients
    {
        FirebaseClient client = FibInstance.GetInstance();

        public FibCRUDClientsService()
		{
		}

        public Task<string> AddClient(ClientModel client)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClinet(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ClientModel>> GetClients()
        {
            throw new NotImplementedException();
        }

        public Task UpdateClient(ClientModel client)
        {
            throw new NotImplementedException();
        }
    }
}

