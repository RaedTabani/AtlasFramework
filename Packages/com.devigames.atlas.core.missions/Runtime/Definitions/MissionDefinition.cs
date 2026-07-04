using UnityEngine;

namespace DeviGames.Atlas.Core.Missions.Definitions
{
    [CreateAssetMenu(fileName = "MissionDefinition", menuName = "DeviGames/Atlas/Missions/Mission Definition")]
    public sealed class MissionDefinition : ScriptableObject
    {

        #if UNITY_EDITOR
        public void Editor_InitializeForTests(
            string missionId,
            string displayName,
            string description,
            string sceneName,
            int unlockOrder,
            int difficulty,
            int version,
            bool downloadable)
        {
            _missionId = missionId;
            _displayName = displayName;
            _description = description;
            _sceneName = sceneName;
            _unlockOrder = unlockOrder;
            _difficulty = difficulty;
            _version = version;
            _downloadable = downloadable;
        }
        #endif
        
        [Header("Identity")]
        [SerializeField] private string _missionId;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea(3, 5)] private string _description;

        [Header("Setup")]
        [SerializeField] private string _sceneName;
        [SerializeField] private Sprite _thumbnail;

        [Header("Progression & Balancing")]
        [SerializeField] private int _unlockOrder;
        [SerializeField] private int _difficulty;

        [Header("Metadata")]
        [SerializeField] private int _version = 1;
        [SerializeField] private bool _downloadable;

        // --- Read-Only Properties ---
        public string MissionId => _missionId;
        public string DisplayName => _displayName;
        public string Description => _description;
        public string SceneName => _sceneName;
        public Sprite Thumbnail => _thumbnail;
        public int UnlockOrder => _unlockOrder;
        public int Difficulty => _difficulty;
        public int Version => _version;
        public bool Downloadable => _downloadable;
    }
}