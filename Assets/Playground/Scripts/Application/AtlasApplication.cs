using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Services;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.Progress.Bootstrap;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Progression.Services;
using DeviGames.Atlas.Unity.Scenes.Interfaces;
using DeviGames.Atlas.Unity.Scenes.Models;
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

        [Header("Mission Scenes")]
        [SerializeField]
        private MissionSceneDefinition[] _missionScenes;

        private BootstrapService _bootstrapService;
        private ISceneService _sceneService;

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
                _sceneService =
                    new UnitySceneService();

                await BootstrapAsync();

                CreateUnityServices();

                EnterMainMenu();

                await _sceneService.LoadAsync(
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
            var missionSceneResolver =
                new MissionSceneResolver(
                    _missionScenes);

            MissionFlowCoordinator missionFlowCoordinator =
                Services.Resolve<MissionFlowCoordinator>();

            MissionLaunchService =
                new MissionLaunchService(
                    missionFlowCoordinator,
                    missionSceneResolver,
                    _sceneService);
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
            await _sceneService.LoadAsync(
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