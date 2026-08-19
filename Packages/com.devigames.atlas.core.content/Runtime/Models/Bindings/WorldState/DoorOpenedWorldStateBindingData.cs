using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class DoorOpenedWorldStateBindingData
    {
        public string DoorId = "";
        public string StateKey = "";
        public bool Value = true;
    }
}