using System;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction.Events;
using DeviGames.Atlas.Core.Interaction.Requests;
using DeviGames.Atlas.Core.Interaction.Results;

namespace DeviGames.Atlas.Core.Interaction.Services
{
    public sealed class InteractionService
    {
        public InteractionResult Process(InteractionRequest request)
        {
            if (request.Target == null)
            {
                return InteractionResult.Failed(
                    "Target is null.");
            }

            EventBus.Publish(new InteractionStartedEvent(request));

            InteractionResult result;

            try
            {
                result = request.Target.Interact(request);
            }
            catch (Exception exception)
            {
                result = InteractionResult.Failed(exception.Message);
            }

            if (result.Success)
            {
                EventBus.Publish(new InteractionCompletedEvent(request,result));
            }
            else
            {
                EventBus.Publish(
                    new InteractionFailedEvent(request,result));
            }

            return result;
        }
    }
}