using System;

using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;

namespace DeviGames.Atlas.Gameplay.Progression.Services
{
    public sealed class MissionAvailabilityService : IMissionAvailabilityService
    {
        private readonly IMissionCollection _missionCollection;
        private readonly IUnlockService _unlockService;

        public MissionAvailabilityService(
            IMissionCollection missionCollection,
            IUnlockService unlockService)
        {
            _missionCollection = missionCollection ?? throw new ArgumentNullException(nameof(missionCollection));
            _unlockService = unlockService ?? throw new ArgumentNullException(nameof(unlockService));
        }

        public bool IsAvailable(string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
            {
                return false;
            }

            if (!_missionCollection.TryGet(missionId, out _))
            {
                return false;
            }

            return _unlockService.IsUnlocked(missionId);
        }
    }
}