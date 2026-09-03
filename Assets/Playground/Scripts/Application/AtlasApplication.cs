using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Services;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.Progress.Bootstrap;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Services;
using DeviGames.Atlas.Unity.Scenes.Interfaces;
using DeviGames.Atlas.Unity.Scenes.Services;
using DeviGames.Playground.Bootstrap;

using UnityEngine;

namespace DeviGames.Atlas.Unity.Application
{
    public sealed class AtlasApplication :
        MonoBehaviour
    {
        private const string MainMenuSceneName =
            "MainMenu";

        public MissionLaunchService MissionLaunchService { get; private set; }

        private static AtlasApplication _instance;
        public static AtlasApplication Instance => _instance;

        private BootstrapService _bootstrapService;
        private ISceneService _applicationSceneService;
        private ISceneService _missionSceneService;

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
                _applicationSceneService =
                    new UnitySceneService();

                _missionSceneService =
                    new AddressableSceneService();

                await BootstrapAsync();

                CreateUnityServices();

                EnterMainMenu();

                await _applicationSceneService.LoadAsync(
                    MainMenuSceneName);
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

        private void CreateUnityServices()
        {
            MissionFlowCoordinator missionFlowCoordinator =
                Services.Resolve<MissionFlowCoordinator>();

            IMissionCollection missionCollection =
                Services.Resolve<IMissionCollection>();

            MissionLaunchService =
                new MissionLaunchService(
                    missionFlowCoordinator,
                    missionCollection,
                    _missionSceneService);
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

        public async Task ReturnToMainMenuAsync()
        {
            await _applicationSceneService.LoadAsync(
                MainMenuSceneName);
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