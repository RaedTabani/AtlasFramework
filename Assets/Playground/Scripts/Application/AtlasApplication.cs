using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Services;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Progress.Bootstrap;
using DeviGames.Playground.Bootstrap;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeviGames.Atlas.Unity.Application
{
    public sealed class AtlasApplication :
        MonoBehaviour
    {
        private const string MainMenuSceneName =
            "MainMenu";

        private static AtlasApplication _instance;

        private BootstrapService _bootstrapService;

        private async void Awake()
        {
            if (_instance != null &&
                _instance != this)
            {
                Destroy(
                    gameObject);

                return;
            }

            _instance =
                this;

            DontDestroyOnLoad(
                gameObject);

            try
            {
                await BootstrapAsync();

                EnterMainMenu();

                await LoadMainMenuAsync();
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);
            }
        }

        private async Task BootstrapAsync()
        {
            _bootstrapService =
                new BootstrapService();

            _bootstrapService
                .AddStep(
                    new RegisterPlaygroundServicesStep())
                .AddStep(
                    new LoadGameStep())
                .AddStep(
                    new LoadPlaygroundContentStep());

            await _bootstrapService.RunAsync();
        }

        private void EnterMainMenu()
        {
            IGameFlowService gameFlowService =
                Services.Resolve<IGameFlowService>();

            if (!gameFlowService.EnterMainMenu())
            {
                throw new InvalidOperationException(
                    "Game Flow failed to enter Main Menu.");
            }
        }

        private async Task LoadMainMenuAsync()
        {
            AsyncOperation operation =
                SceneManager.LoadSceneAsync(
                    MainMenuSceneName,
                    LoadSceneMode.Single);

            if (operation == null)
            {
                throw new InvalidOperationException(
                    $"Failed to load scene '{MainMenuSceneName}'.");
            }

            while (!operation.isDone)
            {
                await Task.Yield();
            }
        }

        private void OnApplicationQuit()
        {
            if (Services.IsInitialized)
            {
                Services.Shutdown();
            }
        }
    }
}