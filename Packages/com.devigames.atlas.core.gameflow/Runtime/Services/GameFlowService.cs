using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;

namespace DeviGames.Atlas.Core.GameFlow.Services
{
    public sealed class GameFlowService :
        IGameFlowService
    {
        public GameFlowState State { get; private set; }

        public GameFlowService()
        {
            State = GameFlowState.Boot;
        }

        public bool EnterMainMenu()
        {
            if (State != GameFlowState.Boot &&
                State != GameFlowState.MissionResults)
            {
                return false;
            }

            return TransitionTo(
                GameFlowState.MainMenu);
        }

        public bool BeginMissionIntro()
        {
            if (State != GameFlowState.MainMenu)
            {
                return false;
            }

            return TransitionTo(
                GameFlowState.MissionIntro);
        }

        public bool BeginGameplay()
        {
            if (State != GameFlowState.MissionIntro)
            {
                return false;
            }

            return TransitionTo(
                GameFlowState.Gameplay);
        }

        public bool BeginMissionOutro()
        {
            if (State != GameFlowState.Gameplay)
            {
                return false;
            }

            return TransitionTo(
                GameFlowState.MissionOutro);
        }

        public bool BeginMissionResults()
        {
            if (State != GameFlowState.MissionOutro)
            {
                return false;
            }

            return TransitionTo(
                GameFlowState.MissionResults);
        }

        private bool TransitionTo(
            GameFlowState state)
        {
            if (State == state)
            {
                return false;
            }

            GameFlowState previousState =
                State;

            State =
                state;

            EventBus.Publish(
                new GameFlowStateChangedEvent(
                    previousState,
                    State));

            return true;
        }
    }
}