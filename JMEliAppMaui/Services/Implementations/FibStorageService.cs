using System;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.Services.Implementations
{
	public class FibStorageService : IFibStorageService
    {
		
        public async Task<string> AddImageFibStorge(string id, string concept, Stream stream)
        {
            var storage = FibStoreInstance.GetInstance();
            try
            {
                var url = await storage.Child(concept).Child($"{concept}-{id}-{DateTime.Now.ToString("ddMMyyyy")}.png").PutAsync(stream);
                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" {ex.Message} {ex.Data}");
               await  App.Current.MainPage.DisplayAlert("Error",$"{ex.Message} , try later.","ok");
            }
            return string.Empty;
        }
    }
}

