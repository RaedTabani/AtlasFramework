using System;

using DeviGames.Atlas.Core.Interaction.Services;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Playground.Debugger;
using DeviGames.Playground.Interaction;

using UnityEngine;

namespace DeviGames.Playground.Application
{
    public sealed class PlaygroundApplication :
        MonoBehaviour
    {
        [Header("Scene Components")]
        [SerializeField]
        private PlaygroundDebugger _debugger;

        [SerializeField]
        private PlaygroundSave _saveSystem;

        private void Start()
        {
            try
            {
                StartPlayground();
            }
            catch (Exception exception)
            {
                Debug.LogException(
                    exception);
            }
        }

        private void StartPlayground()
        {

            IUnlockService unlockService =
                Services.Resolve<IUnlockService>();

            SaveGameCoordinator saveGameCoordinator =
                Services.Resolve<SaveGameCoordinator>();


            _debugger.Initialize(
                unlockService);

            _saveSystem.Initialize(
                saveGameCoordinator);
        }
    }
}