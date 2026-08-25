using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Save.Collections;
using DeviGames.Atlas.Core.Save.Interfaces;

namespace DeviGames.Atlas.Core.Save.Services
{
    public sealed class SaveGameCoordinator
    {
        private readonly SaveParticipantCollection _participants;

        public SaveGameCoordinator(SaveParticipantCollection participants)
        {
            _participants = participants ?? throw new ArgumentNullException(nameof(participants));
        }

        public async Task SaveAsync()
        {
            foreach (ISaveParticipant participant in _participants.Participants)
            {
                await participant.SaveAsync();
            }
        }

        public async Task LoadAsync()
        {
            foreach (ISaveParticipant participant in _participants.Participants)
            {
                await participant.LoadAsync();
            }
        }
    }
}