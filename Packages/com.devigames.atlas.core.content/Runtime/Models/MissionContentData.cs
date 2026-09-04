using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class MissionContentData
    {
        public string Id;

        public string DisplayName;

        public string Description;

        public string[] ObjectiveIds =
            Array.Empty<string>();
        
        public string SceneKey;
        public string ContentKey;

        public string IntroSequenceId;
        public string OutroSequenceId;
    }
}