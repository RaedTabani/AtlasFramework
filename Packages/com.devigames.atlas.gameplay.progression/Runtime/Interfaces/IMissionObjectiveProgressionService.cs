using UnityEngine;

using DeviGames.Atlas.Core.Objectives.Events;

namespace DeviGames.Atlas.Gameplay.Progression.Interfaces
{
    public interface IMissionObjectiveProgressionService
    {
        void OnObjectiveCompleted(ObjectiveCompletedEvent eventData);
    }
}
