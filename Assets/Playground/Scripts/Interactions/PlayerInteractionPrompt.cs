using System;

using DeviGames.Atlas.Gameplay.Player.Interaction;

using TMPro;

using UnityEngine;

namespace DeviGames.Playground.Interaction
{
    public sealed class PlayerInteractionPrompt :
        MonoBehaviour
    {
        [SerializeField]
        private PlayerInteractionController
            _interactionController;

        [SerializeField]
        private GameObject _prompt;

        [SerializeField]
        private TMP_Text _promptText;

        private void Awake()
        {
            if (_interactionController == null)
            {
                throw new InvalidOperationException(
                    "Player Interaction Controller is not assigned.");
            }

            if (_prompt == null)
            {
                throw new InvalidOperationException(
                    "Interaction Prompt is not assigned.");
            }

            if (_promptText == null)
            {
                throw new InvalidOperationException(
                    "Interaction Prompt Text is not assigned.");
            }

            _prompt.SetActive(
                false);
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            bool showPrompt =
                _interactionController.HasTarget;

            _prompt.SetActive(
                showPrompt);

            if (!showPrompt)
            {
                return;
            }

            _promptText.text =
                "E  Interact";
        }
    }
}