using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Unity.Scenes.Interfaces;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeviGames.Atlas.Unity.Scenes.Services
{
    public sealed class UnitySceneService :
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

            AsyncOperation operation =
                SceneManager.LoadSceneAsync(
                    sceneName,
                    LoadSceneMode.Single);

            if (operation == null)
            {
                throw new InvalidOperationException(
                    $"Failed to load scene '{sceneName}'.");
            }

            while (!operation.isDone)
            {
                await Task.Yield();
            }
        }
    }
}