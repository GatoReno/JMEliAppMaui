using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using Firebase.Database;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.Services.Implementations
{
    public class FibGenericAddService : IFibAddGenericService<object>
    {
        FirebaseClient fibClient = FibInstance.GetInstance();


        public async Task<object> AddChild(object children, string concept)
        {

            if (children != null)
            {

                try
                {
                    var noteResult = await fibClient.Child($"{concept}").PostAsync(JsonSerializer.Serialize(children));
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

        public async Task<IReadOnlyCollection<object>> GetChilds(string concept)
        {
            if (fibClient != null)
            {
                var concepts = await fibClient.Child($"{concept}").OnceAsync<object>();
                return concepts;
            }

            return null;
        }



        public async Task UpdateChild(object children, string concept, string id)
        {

            if (children != null)
            {

                try
                {

                    await fibClient.Child($"{concept}/" + id).PutAsync(JsonSerializer.Serialize(children));


                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                }
            }

        }



        public async Task DeleteChild(string id, string concept)
        {
            if (fibClient != null)
            {
                try
                {
                    await fibClient.Child($"{concept}" + "/" + id).DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                }
            }
        }

    }

    public interface IFibLevelsService
    {
        Task<ObservableCollection<StudentLevelsModel>> GetLevels();
    }

    public interface IFibStatusService
    {
        Task<ObservableCollection<StatusModel>> GetStatus();
    }


    public class FibStatusService : IFibStatusService
    {
        FirebaseClient fibClient = FibInstance.GetInstance();

        public async Task<ObservableCollection<StatusModel>> GetStatus()
        {
            ObservableCollection<StatusModel> ResultList = new ObservableCollection<StatusModel>();
            try
            {

                if (fibClient != null)
                {
                    var notes = await fibClient.Child("Status").OnceAsync<object>();
                    foreach (var item in notes)
                    {
                        var note = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusModel>(item.Object.ToString());

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

        public class FibLevelsService : IFibLevelsService
        {
            FirebaseClient fibClient = FibInstance.GetInstance();

            public async Task<ObservableCollection<StudentLevelsModel>> GetLevels()
            {
                ObservableCollection<StudentLevelsModel> ResultList = new ObservableCollection<StudentLevelsModel>();
                try
                {

                    if (fibClient != null)
                    {
                        var notes = await fibClient.Child("Levels").OnceAsync<object>();
                        foreach (var item in notes)
                        {
                            var note = Newtonsoft.Json.JsonConvert.DeserializeObject<StudentLevelsModel>(item.Object.ToString());

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
}


