using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Progression.Services;

using UnityEngine;

namespace DeviGames.Playground.MissionFlow
{
    public class GamePlayGateController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameplayRoot;

        private IGameFlowService _gameFlowService;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            try
            {
                _gameFlowService =Services.Resolve<IGameFlowService>();
                
                EventBus.Subscribe<GameFlowStateChangedEvent>(OnGameFlowStateChanged);
            }
            catch(Exception exception)
            {
                Debug.LogException(exception);
            }
            
        }

        void OnDestroy()
        {
            EventBus.Unsubscribe<GameFlowStateChangedEvent>(OnGameFlowStateChanged);
        }
        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnGameFlowStateChanged(GameFlowStateChangedEvent eventData)
        {
            Refresh();
        }

        private void Refresh()
        {
            if(_gameplayRoot == null)
                return;
            _gameplayRoot.SetActive(_gameFlowService.State == GameFlowState.Gameplay);
        }
    }

}