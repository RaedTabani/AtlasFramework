using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Save.Interfaces
{
    public interface ISaveStorage
    {
        Task SaveAsync<T>(string key, T data);
        Task<T> LoadAsync<T>(string key);
        Task<bool> ExistsAsync(string key);
        Task DeleteAsync(string key);
    }
}