using UnityEngine;

namespace DeviGames.Atlas.Core.Objectives.Definitions
{
    [CreateAssetMenu(
        fileName = "Objective",
        menuName = "DeviGames/Atlas/Objectives/Objective Definition")]
    public sealed class ObjectiveDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _id = "";

        [SerializeField] private string _title = "";

        [TextArea(2, 5)]
        [SerializeField] private string _description = "";

        [Header("Progress")]
        [SerializeField] private int _targetValue = 1;
        [Header("Trigger")]
        [SerializeField] private string _triggerType = "";
        [SerializeField] private string _triggerTargetId = "";

        public string Id => _id;
        public string Title => _title;
        public string Description => _description;
        public int TargetValue => _targetValue;
        public string TriggerType => _triggerType;
        public string TriggerTargetId => _triggerTargetId;

        #if UNITY_EDITOR
        public void Editor_InitializeForTests(
            string id,
            string title,
            string description,
            int targetValue,
            string triggerType = "",
            string triggerTargetId = "")
        {
            _id = id;
            _title = title;
            _description = description;
            _targetValue = targetValue;
            _triggerType = triggerType;
            _triggerTargetId = triggerTargetId;
        }
        #endif
    }
}