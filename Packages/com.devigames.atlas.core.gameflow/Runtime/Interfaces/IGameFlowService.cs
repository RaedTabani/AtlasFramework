using DeviGames.Atlas.Core.GameFlow.Models;

namespace DeviGames.Atlas.Core.GameFlow.Interfaces
{
    public interface IGameFlowService
    {
        GameFlowState State { get; }

        bool EnterMainMenu();

        bool BeginMissionIntro();

        bool BeginGameplay();

        bool BeginMissionOutro();

        bool BeginMissionResults();
    }
}