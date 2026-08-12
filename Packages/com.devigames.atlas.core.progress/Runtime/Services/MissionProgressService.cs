using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Progress.Events;

namespace DeviGames.Atlas.Core.Progress.Services
{
    public sealed class MissionProgressService :
        IInitializable,
        IShutdownable
    {
        private readonly HashSet<string>
            _completedMissionIds =
                new(StringComparer.Ordinal);

        public IReadOnlyCollection<string>
            CompletedMissionIds =>
                _completedMissionIds;
        
        public int CompletedMissionCount =>
            _completedMissionIds.Count;

        public void Initialize()
        {
            EventBus.Subscribe<MissionCompletedEvent>(
                OnMissionCompleted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<MissionCompletedEvent>(
                OnMissionCompleted);
        }

        public bool IsCompleted(
            string missionId)
        {
            if (string.IsNullOrWhiteSpace(
                    missionId))
            {
                return false;
            }

            return _completedMissionIds.Contains(
                missionId);
        }

        public bool MarkCompleted(
            string missionId)
        {
            return MarkCompletedInternal(
                missionId,
                publishEvent: true);
        }

        public void Restore(
            IEnumerable<string> completedMissionIds)
        {
            if (completedMissionIds == null)
            {
                throw new ArgumentNullException(
                    nameof(completedMissionIds));
            }

            _completedMissionIds.Clear();

            foreach (string missionId
                     in completedMissionIds)
            {
                MarkCompletedInternal(
                    missionId,
                    publishEvent: false);
            }
        }

        public void Clear()
        {
            _completedMissionIds.Clear();
        }

        private bool MarkCompletedInternal(
            string missionId,
            bool publishEvent)
        {
            if (string.IsNullOrWhiteSpace(
                    missionId))
            {
                return false;
            }

            if (!_completedMissionIds.Add(
                    missionId))
            {
                return false;
            }

            if (publishEvent)
            {
                EventBus.Publish(
                    new MissionProgressChangedEvent(
                        missionId,
                        completed: true));
            }

            return true;
        }

        private void OnMissionCompleted(
            MissionCompletedEvent eventData)
        {
            MarkCompleted(
                eventData.MissionId);
        }
    }
}