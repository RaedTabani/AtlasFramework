using System;
using System.Collections.Generic;
using UnityEngine;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;

namespace DeviGames.Atlas.Core.Progress.Services
{
    public sealed class ProgressSaveCoordinator: IInitializable, IShutdownable
    {
        private readonly MissionProgressService _progress;
        private readonly SaveService _save;

        public ProgressSaveCoordinator(
            MissionProgressService progress,
            SaveService save)
        {
            _progress = progress ?? throw new ArgumentNullException(nameof(progress));
            _save = save ?? throw new ArgumentNullException(nameof(save));
        }

        public void Initialize()
        {
            EventBus.Subscribe<MissionProgressChangedEvent>(
                OnProgressChanged);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<MissionProgressChangedEvent>(
                OnProgressChanged);
        }

        private async void OnProgressChanged(MissionProgressChangedEvent e)
        {
            try
            {
                await _save.SaveAsync(SaveKeys.Missions, _progress.CreateSnapshot());
            }
            catch(Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}