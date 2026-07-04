using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Save.Interfaces;
using DeviGames.Atlas.Core.Save.Models;
using UnityEngine;

namespace DeviGames.Atlas.Core.Save.Storage
{
    public sealed class JsonFileSaveStorage : ISaveStorage
    {
        private readonly string _rootPath;

        public JsonFileSaveStorage(string rootPath)
        {
            _rootPath = rootPath;
            Directory.CreateDirectory(_rootPath);
        }

        public async Task SaveAsync<T>(string key, T data)
        {
            string path = GetPath(key);

            var envelope = new SaveEnvelope<T>(1, data);
            string json = JsonUtility.ToJson(envelope, true);

            await File.WriteAllTextAsync(path, json);
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            string path = GetPath(key);

            if (!File.Exists(path))
                return default;

            string json = await File.ReadAllTextAsync(path);
            var envelope = JsonUtility.FromJson<SaveEnvelope<T>>(json);

            return envelope.Data;
        }

        public Task<bool> ExistsAsync(string key)
        {
            string path = GetPath(key);
            return Task.FromResult(File.Exists(path));
        }

        public Task DeleteAsync(string key)
        {
            string path = GetPath(key);

            if (File.Exists(path))
                File.Delete(path);

            return Task.CompletedTask;
        }

        private string GetPath(string key)
        {
            return Path.Combine(_rootPath, $"{key}.json");
        }
    }
}