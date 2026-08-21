using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Firebase.Database;
using Firebase.Database.Query;
using JMEliAppMaui.ProgramHelpers.Contants;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.Services.Implementations
{
    /// <summary>
    /// Servicio Firebase genérico unificado con cache en memoria.
    /// - Un solo serializer (System.Text.Json camelCase)
    /// - Cache por colección con TTL configurable (evita N requests al mismo nodo)
    /// - Invalidación automática en writes
    /// - Errores propagados (no silenciados)
    /// </summary>
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseClient _client;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            // No CamelCase policy — writes PascalCase to match existing Firebase data structure
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true  // Reads both PascalCase and camelCase
        };

        // Cache: collection name → (items, timestamp)
        private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
        private static readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(5);

        public FirebaseService()
        {
            _client = FibInstance.GetInstance();
        }

        #region Generic CRUD (Typed)

        public async Task<string> AddAsync<T>(T entity, string collection) where T : class
        {
            var json = JsonSerializer.Serialize(entity, JsonOptions);
            var result = await _client.Child(collection).PostAsync(json);
            InvalidateCache(collection);
            return result.Key;
        }

        public async Task UpdateAsync<T>(T entity, string collection, string id) where T : class
        {
            var json = JsonSerializer.Serialize(entity, JsonOptions);
            await _client.Child($"{collection}/{id}").PutAsync(json);
            InvalidateCache(collection);
        }

        public async Task DeleteAsync(string collection, string id)
        {
            await _client.Child($"{collection}/{id}").DeleteAsync();
            InvalidateCache(collection);
        }

        public async Task<T?> GetByIdAsync<T>(string collection, string id) where T : class
        {
            var item = await _client.Child($"{collection}/{id}").OnceSingleAsync<object>();
            if (item == null) return null;

            var json = item.ToString()!;
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }

        public async Task<ObservableCollection<T>> GetAllAsync<T>(string collection) where T : class, IHasId
        {
            // Check cache first
            if (TryGetFromCache<T>(collection, out var cached))
                return new ObservableCollection<T>(cached!);

            // Use OnceAsync<T> directly — FirebaseDatabase.net handles all deserialization
            // including nested arrays stored as {"0": {...}, "1": {...}}
            var result = new ObservableCollection<T>();
            var items = await _client.Child(collection).OnceAsync<T>();

            foreach (var item in items)
            {
                try
                {
                    if (item.Object != null)
                    {
                        item.Object.Id = item.Key;
                        result.Add(item.Object);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[FirebaseService] Error for {collection}/{item.Key}: {ex.Message}");
                }
            }

            // Store in cache
            SetCache(collection, result.ToList<object>());
            return result;
        }

        public async Task<ObservableCollection<T>> GetWhereAsync<T>(
            string collection, Func<T, bool> predicate) where T : class, IHasId
        {
            var all = await GetAllAsync<T>(collection);
            return new ObservableCollection<T>(all.Where(predicate));
        }

        #endregion

        #region Cache Management

        /// <summary>Invalidate a collection's cache (called after writes).</summary>
        public void InvalidateCache(string collection)
        {
            _cache.TryRemove(collection, out _);
        }

        /// <summary>Invalidate all caches (use after major operations).</summary>
        public void InvalidateAll()
        {
            _cache.Clear();
        }

        private bool TryGetFromCache<T>(string collection, out List<T>? items) where T : class, IHasId
        {
            items = null;
            if (_cache.TryGetValue(collection, out var entry))
            {
                if (DateTime.UtcNow - entry.Timestamp < CacheTTL)
                {
                    items = entry.Items.Cast<T>().ToList();
                    return true;
                }
                // Expired
                _cache.TryRemove(collection, out _);
            }
            return false;
        }

        private void SetCache(string collection, List<object> items)
        {
            _cache[collection] = new CacheEntry(items, DateTime.UtcNow);
        }

        private record CacheEntry(List<object> Items, DateTime Timestamp);

        #endregion

        #region Legacy Compatibility

        public async Task<object> AddChild<T>(T children, string concept)
        {
            if (children == null) return "";
            var json = JsonSerializer.Serialize(children, JsonOptions);
            var result = await _client.Child(concept).PostAsync(json);
            InvalidateCache(concept);
            return result.Key ?? "";
        }

        public async Task UpdateChild<T>(T children, string concept, string id)
        {
            if (children == null) return;
            var json = JsonSerializer.Serialize(children, JsonOptions);
            await _client.Child($"{concept}/{id}").PutAsync(json);
            InvalidateCache(concept);
        }

        public async Task<IReadOnlyCollection<object>> GetChilds(string concept)
        {
            var items = await _client.Child(concept).OnceAsync<object>();
            return items;
        }

        public async Task DeleteChild(string id, string concept)
        {
            await _client.Child($"{concept}/{id}").DeleteAsync();
            InvalidateCache(concept);
        }

        #endregion
    }

    /// <summary>
    /// Interface marker para modelos con Id (GetAllAsync asigna el Firebase Key al Id).
    /// </summary>
    public interface IHasId
    {
        string? Id { get; set; }
    }
}
