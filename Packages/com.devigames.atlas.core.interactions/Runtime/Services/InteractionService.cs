using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Interaction.Events;
using DeviGames.Atlas.Core.Interaction.Models;

namespace DeviGames.Atlas.Core.Interaction.Services
{
    public sealed class InteractionService
    {
        public InteractionResult Process(
            InteractionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(
                    nameof(request));
            }

            var target =
                request.Target;

            var context =
                request.Context;

            if (!target.CanInteract(
                    context))
            {
                InteractionResult rejected =
                    InteractionResult.Failed(
                        "Interaction is not currently available.");

                EventBus.Publish(
                    new InteractionFailedEvent(
                        request,
                        rejected));

                return rejected;
            }

            EventBus.Publish(
                new InteractionStartedEvent(
                    request));

            InteractionResult result;

            try
            {
                result =
                    target.Interact(
                        context);
            }
            catch (Exception exception)
            {
                result =
                    InteractionResult.Failed(
                        exception.Message);
            }

            if (result.Succeeded)
            {
                EventBus.Publish(
                    new InteractionCompletedEvent(
                        request,
                        result));
            }
            else
            {
                EventBus.Publish(
                    new InteractionFailedEvent(
                        request,
                        result));
            }

            return result;
        }
    }
}