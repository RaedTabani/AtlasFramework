using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Rewards.Interfaces;

namespace DeviGames.Atlas.Core.Rewards.Registry
{
    public sealed class RewardHandlerRegistry
    {
        private readonly Dictionary<string,IRewardHandler>  _handlers = new(StringComparer.Ordinal);

        public void Register(
            IRewardHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(
                    nameof(handler));
            }

            if (string.IsNullOrWhiteSpace(
                    handler.Type))
            {
                throw new ArgumentException(
                    "Reward handler must have a type.",
                    nameof(handler));
            }

            if (!_handlers.TryAdd(
                    handler.Type,
                    handler))
            {
                throw new InvalidOperationException(
                    $"Reward handler '{handler.Type}' " +
                    "is already registered.");
            }
        }

        public IRewardHandler Resolve(
            string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(
                    "Reward type cannot be empty.",
                    nameof(type));
            }

            if (!_handlers.TryGetValue(
                    type,
                    out IRewardHandler handler))
            {
                throw new InvalidOperationException(
                    $"No reward handler is registered " +
                    $"for '{type}'.");
            }

            return handler;
        }
    }
}