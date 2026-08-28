using DeviGames.Atlas.Core.GameFlow.Models;

namespace DeviGames.Atlas.Core.GameFlow.Events
{
    public readonly struct GameFlowStateChangedEvent
    {
        public GameFlowState PreviousState { get; }

        public GameFlowState CurrentState { get; }

        public GameFlowStateChangedEvent(
            GameFlowState previousState,
            GameFlowState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }
    }
}