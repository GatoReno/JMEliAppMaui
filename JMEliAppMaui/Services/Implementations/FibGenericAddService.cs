using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using Firebase.Database;
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

        public async Task<object> GetChilds(string concept)
        {
            if (fibClient != null)
            {
                var concepts = await fibClient.Child($"{concept}").OnceAsync<object>();
                return concepts;
            }

            return "";
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
 
    }
}

