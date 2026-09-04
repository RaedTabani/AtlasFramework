using System;
using System.Threading.Tasks;

namespace DeviGames.Atlas.Unity.Scenes.Interfaces
{
    public interface IContentDownloadService
    {
        Task<long> GetDownloadSizeAsync(
            string contentKey);

        Task DownloadAsync(
            string contentKey,
            IProgress<float> progress = null);
    }
}