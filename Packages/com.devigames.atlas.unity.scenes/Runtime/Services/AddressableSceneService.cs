using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Unity.Scenes.Interfaces;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace DeviGames.Atlas.Unity.Scenes.Services
{
    public sealed class AddressableSceneService :
        ISceneService
    {
        public string ActiveSceneName =>
            SceneManager.GetActiveScene().name;

        public async Task LoadAsync(
            string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                throw new ArgumentException(
                    "Scene name cannot be null or empty.",
                    nameof(sceneName));
            }

            AsyncOperationHandle<SceneInstance> operation =
                Addressables.LoadSceneAsync(
                    sceneName,
                    LoadSceneMode.Single);

            await operation.Task;

            if (operation.Status !=
                AsyncOperationStatus.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to load Addressable scene '{sceneName}'.",
                    operation.OperationException);
            }
        }
    }
}