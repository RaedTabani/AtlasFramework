using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class AreaEnteredObjectiveBindingData
    {
        public string ObjectiveId;
        public string AreaId;
        public int ProgressAmount = 1;
    }
}