using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Unity.Scenes.Interfaces;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DeviGames.Atlas.Unity.Scenes.Services
{
    public sealed class AddressableContentDownloadService :
        IContentDownloadService
    {
        public async Task<long> GetDownloadSizeAsync(
            string contentKey)
        {
            if (string.IsNullOrWhiteSpace(contentKey))
            {
                throw new ArgumentException(
                    "Content key cannot be null or empty.",
                    nameof(contentKey));
            }

            AsyncOperationHandle<long> operation =
                Addressables.GetDownloadSizeAsync(
                    contentKey);

            try
            {
                await operation.Task;

                if (operation.Status !=
                    AsyncOperationStatus.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to get download size for content '{contentKey}'.",
                        operation.OperationException);
                }

                return operation.Result;
            }
            finally
            {
                Addressables.Release(
                    operation);
            }
        }

        public async Task DownloadAsync(
            string contentKey,
            IProgress<float> progress = null)
        {
            if (string.IsNullOrWhiteSpace(contentKey))
            {
                throw new ArgumentException(
                    "Content key cannot be null or empty.",
                    nameof(contentKey));
            }

            AsyncOperationHandle operation =
                Addressables.DownloadDependenciesAsync(
                    contentKey,
                    false);

            try
            {
                while (!operation.IsDone)
                {
                    DownloadStatus status =
                        operation.GetDownloadStatus();

                    progress?.Report(
                        status.Percent);

                    await Task.Yield();
                }

                if (operation.Status !=
                    AsyncOperationStatus.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to download content '{contentKey}'.",
                        operation.OperationException);
                }

                progress?.Report(
                    1f);
            }
            finally
            {
                Addressables.Release(
                    operation);
            }
        }
    }
}