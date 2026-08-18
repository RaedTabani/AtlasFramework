using System;
using DeviGames.Atlas.Core.Interaction.Interfaces;

namespace DeviGames.Atlas.Core.Interaction.Models
{
    public sealed class InteractionRequest
    {
        public IInteractable Target { get; }

        public InteractionContext Context { get; }

        public InteractionRequest(
            IInteractable target,
            InteractionContext context)
        {
            Target =
                target
                ?? throw new ArgumentNullException(
                    nameof(target));

            Context =
                context;
        }
    }
}