using System;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Services;
using DeviGames.Atlas.Core.Interaction.Services;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Progress.Bootstrap;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Playground.Bootstrap;
using DeviGames.Playground.Interaction;
using DeviGames.Playground.Trigger;
using DeviGames.Playground.Debugger;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;
using DeviGames.Atlas.Gameplay.Objectives.Services;
using UnityEngine;

namespace DeviGames.Playground.Application
{
    public sealed class PlaygroundApplication : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField]
        private MissionDefinition _mission;

        [Header("Scene Components")]
        [SerializeField]
        private InteractionPlaygroundController _interactionController;
        [SerializeField]
        private TriggerPlaygroundController _triggerController;
        [SerializeField]
        private PlayerInteractionController _playerInteractionController;
        [SerializeField]
        private PlaygroundDebugger _debugger;

        private BootstrapService _bootstrapService;
        

        private async void Start()
        {
            try
            {
                await BootstrapAsync();
                StartPlayground();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private async Task BootstrapAsync()
        {
            _bootstrapService = new BootstrapService();

            _bootstrapService.AddStep(new RegisterPlaygroundServicesStep())
            .AddStep(new LoadMissionProgressStep()) 
            .AddStep(new LoadWorldStateStep())
            .AddStep(new LoadUnlocksStep())
            .AddStep(new LoadPlaygroundContentStep());

            await _bootstrapService.RunAsync();
        }

        private void StartPlayground()
        {
            InteractionService interactionService =
                Services.Resolve<InteractionService>();

            MissionService missionService =
                Services.Resolve<MissionService>();

            MissionProgressService progressService =
                Services.Resolve<MissionProgressService>();

            IInventoryService inventoryService =
                Services.Resolve<IInventoryService>();
            
            IUnlockService unlockService = Services.Resolve<IUnlockService>();

            //_interactionController.Initialize(interactionService);
            //_triggerController.Initialize(inventoryService);
            _playerInteractionController.Initialize(interactionService);
            _debugger.Initialize(unlockService);

            //missionService.StartMission(_mission);
        }

        private void OnDestroy()
        {
            if (Services.IsInitialized)
            {
                Services.Shutdown();
            }
        }
    }
}