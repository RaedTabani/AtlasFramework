using System;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Save.Interfaces;

namespace DeviGames.Atlas.Core.Save.Services
{
    public sealed class SaveService
    {
        private readonly ISaveStorage _storage;

        public SaveService(ISaveStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task SaveAsync<T>(string key, T data)
        {
            ValidateKey(key);
            return _storage.SaveAsync(key, data);
        }

        public Task<T> LoadAsync<T>(string key)
        {
            ValidateKey(key);
            return _storage.LoadAsync<T>(key);
        }

        public Task<bool> ExistsAsync(string key)
        {
            ValidateKey(key);
            return _storage.ExistsAsync(key);
        }

        public Task DeleteAsync(string key)
        {
            ValidateKey(key);
            return _storage.DeleteAsync(key);
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Save key cannot be null, empty, or whitespace.", nameof(key));
        }
    }
}