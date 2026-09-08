using DeviGames.Atlas.Gameplay.Progression.Models;

namespace DeviGames.Atlas.Gameplay.Progression.Interfaces
{
    public interface IMissionResultService
    {
        MissionResult CurrentResult { get; }

        bool HasResult { get; }

        bool TryGetResult(
            out MissionResult result);

        void Clear();
    }
}