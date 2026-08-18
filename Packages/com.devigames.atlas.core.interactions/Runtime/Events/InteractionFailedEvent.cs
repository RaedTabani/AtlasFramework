using DeviGames.Atlas.Core.Interaction.Models;

namespace DeviGames.Atlas.Core.Interaction.Events
{
    public readonly struct InteractionFailedEvent
    {
        public InteractionRequest Request { get; }

        public InteractionResult Result { get; }

        public InteractionFailedEvent(
            InteractionRequest request,
            InteractionResult result)
        {
            Request = request;
            Result = result;
        }
    }
}