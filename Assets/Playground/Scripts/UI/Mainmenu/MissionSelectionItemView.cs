using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace DeviGames.Playground.MainMenu
{
    public sealed class MissionSelectionItemView :
        MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _titleText;

        [SerializeField]
        private TMP_Text _statusText;

        [SerializeField]
        private Button _playButton;

        private string _missionId;
        private Action<string> _onPlay;

        public void Bind(
            MissionSelectionItem item,
            Action<string> onPlay)
        {
            _missionId =
                item.MissionId;

            _onPlay =
                onPlay;

            _titleText.text =
                item.Title;

            _playButton.interactable =
                item.IsUnlocked;

            _statusText.text =
                GetStatusText(
                    item);
        }

        private void Awake()
        {
            _playButton.onClick.AddListener(
                HandlePlayClicked);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(
                HandlePlayClicked);
        }

        private void HandlePlayClicked()
        {
            _onPlay?.Invoke(
                _missionId);
        }

        private static string GetStatusText(
            MissionSelectionItem item)
        {
            if (!item.IsUnlocked)
            {
                return "Locked";
            }

            if (item.IsCompleted)
            {
                return "Completed";
            }

            return "Available";
        }
    }
}