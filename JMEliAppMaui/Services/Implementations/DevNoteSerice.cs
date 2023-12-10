using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using Firebase.Database; 
using JMEliAppMaui.Models;
using JMEliAppMaui.ProgramHelpers;
using JMEliAppMaui.Services.Abstractions;
using Newtonsoft.Json.Linq;
 
namespace JMEliAppMaui.Services.Implementations
{
    public class FibService : IDevNotesService
    {
        FirebaseClient client = new FirebaseClient(ProgramHelpers.Contants.FibConstants.fibRef);
		 
        public async Task AddNoteDev(Note4DevModel note)
        {
            if (client != null)
            {
                try
                {
                    await client.Child("note4Dev").PostAsync(JsonSerializer.Serialize(note));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                }
            }
         }

       

        public async Task<ObservableCollection<Note4DevModel>> GetNotes()
        {
            ObservableCollection<Note4DevModel> ResultList = new ObservableCollection<Note4DevModel>();
            try
            {

                var notes =  await client.Child("note4Dev").OnceAsync<object>();
                foreach (var item in notes)
                {
                    // product = Newtonsoft.Json.JsonConvert.DeserializeObject<Note4DevModel>(item.Object.ToString());
                    var note = Newtonsoft.Json.JsonConvert.DeserializeObject<Note4DevModel>(item.Object.ToString());
                    note.Date.ToString("dd-MMMM-yyyy");
                    note.Id = item.Key.ToString();
                    ResultList.Add(note);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($" {ex.Message} {ex.Data}");
                
            }

            return ResultList;
        }
    }
}

