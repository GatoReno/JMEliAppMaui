using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using Firebase.Database;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using System.Linq;
namespace JMEliAppMaui.Services.Implementations
{
    public class FibCRUDClientsService : IFibCRUDClients
    {
        FirebaseClient fibClient = FibInstance.GetInstance();

        public FibCRUDClientsService()
        {
        }

        public async Task<string> AddClient(ClientModel client)
        {
            if (client != null)
            {

                try
                {
                    var noteResult = await fibClient.Child("Clients").PostAsync(JsonSerializer.Serialize(client));
                    if (noteResult.Key != null)
                    {
                        return noteResult.Key;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                }
            }

            return "";
        }

        public async Task DeleteClinet(string id)
        {
            if (fibClient != null)
            {
                try
                {
                    await fibClient.Child("Clients" + "/" + id).DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                }
            }
        }

        public async Task<ObservableCollection<ClientModel>> GetClients()
        {
            ObservableCollection<ClientModel> ResultList = new ObservableCollection<ClientModel>();
            try
            {

                if (fibClient != null)
                {
                    var notes = await fibClient.Child("Clients").OnceAsync<object>();
                    foreach (var item in notes)
                    {
                        var note = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientModel>(item.Object.ToString());
                        
                        note.Id = item.Key.ToString();
                        ResultList.Add(note);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($" {ex.Message} {ex.Data}");

            }

            return ResultList;
        }

        public async void UpdateClient(ClientModel client)
        {
            if (client != null)
            {

                try
                {
                  await fibClient.Child("Clients/"+client.Id).PutAsync(JsonSerializer.Serialize(client));
 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
 
                }
            }
              
        }
    }
}

