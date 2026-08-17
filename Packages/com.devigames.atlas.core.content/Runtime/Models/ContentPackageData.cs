using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class ContentPackageData
    {
        public int Version = 1;

        public string PackageId;

        public ObjectiveContentData[] Objectives =
            Array.Empty<ObjectiveContentData>();

        public MissionContentData[] Missions =
            Array.Empty<MissionContentData>();
        public ItemCollectedObjectiveBindingData[] ItemCollectedObjectiveBindings =
            Array.Empty<ItemCollectedObjectiveBindingData>();

        public AreaEnteredObjectiveBindingData[] AreaEnteredObjectiveBindings =
            Array.Empty<AreaEnteredObjectiveBindingData>();

        public DoorOpenedObjectiveBindingData[] DoorOpenedObjectiveBindings =
            Array.Empty<DoorOpenedObjectiveBindingData>();

        public TriggerContentData[] Triggers =
            Array.Empty<TriggerContentData>();
    }
}