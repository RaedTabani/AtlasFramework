using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Sequence.Events;
using DeviGames.Atlas.Core.Sequence.Factories;
using DeviGames.Atlas.Core.Sequence.Collections;
using DeviGames.Atlas.Core.Sequence.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Progression.Services;
using DeviGames.Playground.Sequence;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace DeviGames.Playground.MissionFlow
{
    public sealed class MissionOutroController :
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
        private IMissionCollection _missionCollection;
        private SequenceDefinitionCollection _sequenceDefinitions;

        private SequencePlayer _sequencePlayer;

        private string _activeOutroSequenceId =
            string.Empty;

        private void Awake()
        {
            if (_panel == null)
            {
                throw new InvalidOperationException(
                    "Mission Outro panel is not assigned.");
            }

            if (_text == null)
            {
                throw new InvalidOperationException(
                    "Mission Outro text is not assigned.");
            }

            if (_continueButton == null)
            {
                throw new InvalidOperationException(
                    "Mission Outro continue button is not assigned.");
            }

            _missionFlowCoordinator =
                Services.Resolve<MissionFlowCoordinator>();

            _gameFlowService =
                Services.Resolve<IGameFlowService>();

            _missionCollection =
                Services.Resolve<IMissionCollection>();

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
            bool isOutro =
                _gameFlowService.State ==
                GameFlowState.MissionOutro;

            _panel.SetActive(
                isOutro);

            if (isOutro &&
                (_sequencePlayer == null ||
                 !_sequencePlayer.IsPlaying))
            {
                PlayOutroSequence();
            }
        }

        private void PlayOutroSequence()
        {
            if (!_missionFlowCoordinator.HasMission)
            {
                throw new InvalidOperationException(
                    "No mission is available for the mission outro.");
            }

            if (!_missionCollection.TryGet(
                    _missionFlowCoordinator.MissionId,
                    out MissionRuntime mission))
            {
                throw new InvalidOperationException(
                    $"Mission '{_missionFlowCoordinator.MissionId}' could not be found.");
            }

            if (string.IsNullOrWhiteSpace(
                    mission.OutroSequenceId))
            {
                throw new InvalidOperationException(
                    $"Mission '{mission.Id}' does not define an outro sequence.");
            }

            _activeOutroSequenceId =
                mission.OutroSequenceId;

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
                        _activeOutroSequenceId));

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
                    _activeOutroSequenceId,
                    StringComparison.Ordinal))
            {
                return;
            }

            _missionFlowCoordinator.CompleteOutro();
        }
    }
}