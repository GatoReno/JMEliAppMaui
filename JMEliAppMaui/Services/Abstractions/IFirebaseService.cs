using System.Collections.ObjectModel;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Services.Abstractions
{
    /// <summary>
    /// Servicio Firebase unificado — CRUD genérico tipado.
    /// También implementa IFibAddGenericService para retrocompatibilidad.
    /// </summary>
    public interface IFirebaseService : IFibAddGenericService
    {
        // Typed CRUD
        Task<string> AddAsync<T>(T entity, string collection) where T : class;
        Task UpdateAsync<T>(T entity, string collection, string id) where T : class;
        Task DeleteAsync(string collection, string id);
        Task<T?> GetByIdAsync<T>(string collection, string id) where T : class;
        Task<ObservableCollection<T>> GetAllAsync<T>(string collection) where T : class, IHasId;
        Task<ObservableCollection<T>> GetWhereAsync<T>(string collection, Func<T, bool> predicate) where T : class, IHasId;
    }
}
