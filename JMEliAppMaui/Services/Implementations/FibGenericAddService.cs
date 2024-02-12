using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using Firebase.Database;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

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

    #region interfaces
    public interface IFibLevelsService
    {
        Task<ObservableCollection<StudentLevelsModel>> GetLevels();
    }

    public interface IFibStatusService
    {
        Task<ObservableCollection<StatusModel>> GetStatus();
    }

    public interface IFibCyclesService
    {
        Task<ObservableCollection<CycleModel>> GetCycles();
    }

    public interface IFibContract
    {
        Task<ObservableCollection<ContractTypeModel>> GetContracts();
        Task<ObservableCollection<ContractModel>> GetContractsClient(string id);
        Task<ObservableCollection<ContractModel>> GetContractsStudent(string id);

    }
    #endregion

    public class FibContractService : IFibContract
    {
        public async Task<ObservableCollection<ContractTypeModel>> GetContracts()
        {
            FirebaseClient fibClient = FibInstance.GetInstance();


            ObservableCollection<ContractTypeModel> ResultList = new ObservableCollection<ContractTypeModel>();
            try
            {

                if (fibClient != null)
                {
                    var contracts = await fibClient.Child("ContractType").OnceAsync<object>();
                    foreach (var item in contracts)
                    {
                        var contract = Newtonsoft.Json.JsonConvert.DeserializeObject<ContractTypeModel>(item.Object.ToString());

                        contract.Id = item.Key.ToString();
                        ResultList.Add(contract);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($" {ex.Message} {ex.Data}");

            }

            return ResultList;
        }

        public async Task<ObservableCollection<ContractModel>> GetContractsClient(string id)
        {
            FirebaseClient fibClient = FibInstance.GetInstance();
            ObservableCollection<ContractModel> ResultList = new ObservableCollection<ContractModel>();
            try
            {

                if (fibClient != null)
                {
                    var contracts = await fibClient.Child("Contract").OnceAsync<object>();
                    foreach (var item in contracts)
                    {
                        var contract = Newtonsoft.Json.JsonConvert.DeserializeObject<ContractModel>(item.Object.ToString());

                        if (contract.ClientId == id)
                        {
                            ResultList.Add(contract);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($" {ex.Message} {ex.Data}");

            }

            return ResultList;
        }

        public async Task<ObservableCollection<ContractModel>> GetContractsStudent(string id)
        {
            FirebaseClient fibClient = FibInstance.GetInstance();
            ObservableCollection<ContractModel> ResultList = new ObservableCollection<ContractModel>();
            try
            {

                if (fibClient != null)
                {
                    var contracts = await fibClient.Child("Contract").OnceAsync<object>();
                    foreach (var item in contracts)
                    {
                        var contract = Newtonsoft.Json.JsonConvert.DeserializeObject<ContractModel>(item.Object.ToString());

                        if (contract.StudentId == id)
                        {
                            ResultList.Add(contract);
                        }

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

    public class FibCyclesService : IFibCyclesService
    {
        public async Task<ObservableCollection<CycleModel>> GetCycles()
        {
            FirebaseClient fibClient = FibInstance.GetInstance();


            ObservableCollection<CycleModel> ResultList = new ObservableCollection<CycleModel>();
            try
            {

                if (fibClient != null)
                {
                    var notes = await fibClient.Child("Cycles").OnceAsync<object>();
                    foreach (var item in notes)
                    {
                        var note = Newtonsoft.Json.JsonConvert.DeserializeObject<CycleModel>(item.Object.ToString());

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




