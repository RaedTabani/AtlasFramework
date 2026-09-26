using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace DeviGames.Atlas.Unity.Application.UI
{
    public sealed class LoadingTransitionController :
        MonoBehaviour
    {
        [SerializeField]
        private GameObject _panel;

        [SerializeField]
        private TMP_Text _statusText;

        [SerializeField]
        private Slider _progressSlider;

        public bool IsVisible =>
            _panel != null &&
            _panel.activeSelf;

        private void Awake()
        {
            if (_panel == null)
            {
                throw new InvalidOperationException(
                    "Loading transition panel is not assigned.");
            }

            if (_statusText == null)
            {
                throw new InvalidOperationException(
                    "Loading transition status text is not assigned.");
            }

            if (_progressSlider == null)
            {
                throw new InvalidOperationException(
                    "Loading transition progress slider is not assigned.");
            }

            Hide();
        }

        public void Show(
            string status)
        {
            _panel.SetActive(
                true);

            _statusText.text =
                status ?? string.Empty;

            _progressSlider.gameObject.SetActive(
                false);
        }

        public void ShowProgress(
            string status,
            float progress)
        {
            _panel.SetActive(
                true);

            _statusText.text =
                status ?? string.Empty;

            _progressSlider.gameObject.SetActive(
                true);

            _progressSlider.value =
                Mathf.Clamp01(
                    progress);
        }

        public void SetStatus(
            string status)
        {
            _statusText.text =
                status ?? string.Empty;
        }

        public void SetProgress(
            float progress)
        {
            _progressSlider.gameObject.SetActive(
                true);

            _progressSlider.value =
                Mathf.Clamp01(
                    progress);
        }

        public void Hide()
        {
            _panel.SetActive(
                false);

            _statusText.text =
                string.Empty;

            _progressSlider.value =
                0f;

            _progressSlider.gameObject.SetActive(
                false);
        }
    }
}