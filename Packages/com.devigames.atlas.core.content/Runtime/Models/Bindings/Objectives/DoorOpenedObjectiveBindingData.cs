using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class DoorOpenedObjectiveBindingData
    {
        public string ObjectiveId;
        public string DoorId;
        public int ProgressAmount = 1;
    }
}