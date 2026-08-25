using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Save.Interfaces;

namespace DeviGames.Atlas.Core.Save.Collections
{
    public sealed class SaveParticipantCollection
    {
        private readonly Dictionary<string, ISaveParticipant> _participants =
            new(StringComparer.Ordinal);

        public IReadOnlyCollection<ISaveParticipant> Participants =>
            _participants.Values;

        public void Add(ISaveParticipant participant)
        {
            if (participant == null)
            {
                throw new ArgumentNullException(nameof(participant));
            }

            if (string.IsNullOrWhiteSpace(participant.Key))
            {
                throw new ArgumentException(
                    "Save participant key cannot be empty.",
                    nameof(participant));
            }

            if (!_participants.TryAdd(participant.Key, participant))
            {
                throw new InvalidOperationException(
                    $"Save participant '{participant.Key}' is already registered.");
            }
        }
    }
}