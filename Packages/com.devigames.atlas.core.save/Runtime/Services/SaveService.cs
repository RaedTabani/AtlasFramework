using System;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Save.Events;
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

        public async Task SaveAsync<T>(string key, T data)
        {
            ValidateKey(key);

            try
            {
                await _storage.SaveAsync(key, data);
                EventBus.Publish(new SaveCompletedEvent(key));
            }
            catch (Exception ex)
            {
                EventBus.Publish(new SaveFailedEvent(key, ex));
                throw;
            }
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            ValidateKey(key);

            try
            {
                T result = await _storage.LoadAsync<T>(key);
                EventBus.Publish(new LoadCompletedEvent(key));
                return result;
            }
            catch (Exception ex)
            {
                EventBus.Publish(new LoadFailedEvent(key, ex));
                throw;
            }
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