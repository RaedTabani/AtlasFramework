using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Sequence.Collections;
using DeviGames.Atlas.Core.Sequence.Events;
using DeviGames.Atlas.Core.Sequence.Factories;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Services;
using DeviGames.Atlas.Gameplay.Progression.Services;
using DeviGames.Playground.Sequence;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace DeviGames.Playground
{
    public sealed class MissionIntroController :
        MonoBehaviour
    {
        [SerializeField]
        private GameObject _panel;

        [SerializeField]
        private TMP_Text _text;

        [SerializeField]
        private Button _continueButton;

        private MissionFlowCoordinator _missionFlowCoordinator;
        private IGameFlowService _gameFlowService;
        private SequenceDefinitionCollection _sequenceDefinitions;

        private SequencePlayer _sequencePlayer;

        private const string IntroSequenceId =
            "sequence.mission.escape.intro";

        private void Awake()
        {
            if (_panel == null)
            {
                throw new InvalidOperationException(
                    "Mission Intro panel is not assigned.");
            }

            if (_text == null)
            {
                throw new InvalidOperationException(
                    "Mission Intro text is not assigned.");
            }

            if (_continueButton == null)
            {
                throw new InvalidOperationException(
                    "Mission Intro continue button is not assigned.");
            }

            _missionFlowCoordinator =
                Services.Resolve<MissionFlowCoordinator>();

            _gameFlowService =
                Services.Resolve<IGameFlowService>();

            _sequenceDefinitions =
                Services.Resolve<SequenceDefinitionCollection>();

            _continueButton.onClick.AddListener(
                ContinueSequence);

            EventBus.Subscribe<GameFlowStateChangedEvent>(
                OnGameFlowStateChanged);

            EventBus.Subscribe<SequenceCompletedEvent>(
                OnSequenceCompleted);

            Refresh();
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(
                ContinueSequence);

            EventBus.Unsubscribe<GameFlowStateChangedEvent>(
                OnGameFlowStateChanged);

            EventBus.Unsubscribe<SequenceCompletedEvent>(
                OnSequenceCompleted);
        }

        private void OnGameFlowStateChanged(
            GameFlowStateChangedEvent eventData)
        {
            Refresh();
        }

        private void Refresh()
        {
            bool isIntro =
                _gameFlowService.State ==
                GameFlowState.MissionIntro;

            _panel.SetActive(
                isIntro);

            if (isIntro &&
                (_sequencePlayer == null ||
                 !_sequencePlayer.IsPlaying))
            {
                PlayIntroSequence();
            }
        }

        private void PlayIntroSequence()
        {
            var presenter =
                new UnitySequenceTextPresenter(
                    _text);

            var registry =
                new SequenceStepFactoryRegistry();

            registry.Register(
                new ShowTextStepFactory(
                    presenter));

            registry.Register(
                new WaitForContinueStepFactory());

            var factory =
                new SequenceFactory(
                    registry);

            SequenceRuntime sequence =
                factory.Create(
                    _sequenceDefinitions.Get(
                        IntroSequenceId));

            _sequencePlayer =
                new SequencePlayer();

            _sequencePlayer.Play(
                sequence);
        }

        private void ContinueSequence()
        {
            _sequencePlayer?.Continue();
        }

        private void OnSequenceCompleted(
            SequenceCompletedEvent eventData)
        {
            if (!string.Equals(
                    eventData.SequenceId,
                    IntroSequenceId,
                    StringComparison.Ordinal))
            {
                return;
            }

            _missionFlowCoordinator.CompleteIntro();
        }
    }
}