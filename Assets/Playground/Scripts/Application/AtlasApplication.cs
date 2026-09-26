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
using DeviGames.Atlas.Unity.Application.UI;
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
        public LoadingTransitionController LoadingTransition => _loadingTransition;

        private static AtlasApplication _instance;
        public static AtlasApplication Instance => _instance;

        [SerializeField]
        private LoadingTransitionController _loadingTransition;

        private BootstrapService _bootstrapService;
        private ISceneService _applicationSceneService;
        private ISceneService _missionSceneService;
        private IContentDownloadService _contentDownloadService;
        

        private async void Awake()
        {
            if (_instance != null &&
                _instance != this)
            {
                Destroy(
                    gameObject);

                return;
            }

            if (_loadingTransition == null)
            {
                throw new InvalidOperationException(
                    "Loading transition controller is not assigned.");
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

                _contentDownloadService = new AddressableContentDownloadService();

                await BootstrapAsync();

                CreateUnityServices();

                EnterMainMenu();

                _loadingTransition.Show("Loading...");

                try
                {
                    await _applicationSceneService.LoadAsync(
                        MainMenuSceneName);
                }
                finally
                {
                    _loadingTransition.Hide();
                }

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
                    _contentDownloadService,
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
            _loadingTransition.Show(
                "Loading...");

            try
            {
                await _applicationSceneService.LoadAsync(
                    MainMenuSceneName);
            }
            finally
            {
                _loadingTransition.Hide();
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