using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;

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