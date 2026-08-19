using System;

namespace DeviGames.Atlas.Gameplay.WorldState.Models
{
    public sealed class DoorOpenedWorldStateBinding
    {
        public string DoorId { get; }

        public string StateKey { get; }

        public bool Value { get; }

        public DoorOpenedWorldStateBinding(
            string doorId,
            string stateKey,
            bool value = true)
        {
            if (string.IsNullOrWhiteSpace(
                    doorId))
            {
                throw new ArgumentException(
                    "Door ID cannot be empty.",
                    nameof(doorId));
            }

            if (string.IsNullOrWhiteSpace(
                    stateKey))
            {
                throw new ArgumentException(
                    "World state key cannot be empty.",
                    nameof(stateKey));
            }

            DoorId = doorId;

            StateKey = stateKey;

            Value = value;
        }
    }
}