using DeviGames.Atlas.Core.Interaction.Models;

namespace DeviGames.Atlas.Core.Interaction.Events
{
    public readonly struct InteractionCompletedEvent
    {
        public InteractionRequest Request { get; }

        public InteractionResult Result { get; }

        public InteractionCompletedEvent(
            InteractionRequest request,
            InteractionResult result)
        {
            Request = request;
            Result = result;
        }
    }
}