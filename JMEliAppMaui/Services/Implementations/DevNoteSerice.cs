 using System.Collections.ObjectModel;
using System.Text.Json;
using Firebase.Database; 
using JMEliAppMaui.Models; 
using JMEliAppMaui.Services.Abstractions;
 
namespace JMEliAppMaui.Services.Implementations
{
    public class FibService : IDevNotesService
    {
        FirebaseClient client = new FirebaseClient(ProgramHelpers.Contants.FibConstants.fibRef);
		 
        public async Task<string> AddNoteDev(Note4DevModel note)
        {
            if (client != null)
            {
                try
                {
                    var noteResult = await client.Child("note4Dev").PostAsync(JsonSerializer.Serialize(note));
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

        public async Task DeleteNoteDev(string id)
        {
            if (client != null)
            {
                try
                {
                    await client.Child("note4Dev" + "/" + id).DeleteAsync();
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

                if (client != null)
                {
                    var notes = await client.Child("note4Dev").OnceAsync<object>();
                    foreach (var item in notes)
                    {
                        var note = Newtonsoft.Json.JsonConvert.DeserializeObject<Note4DevModel>(item.Object.ToString());
                        note?.Date.ToString("dd-MM-yyyy");
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
    }
}

