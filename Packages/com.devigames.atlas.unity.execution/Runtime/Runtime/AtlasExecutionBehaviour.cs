using System;
using UnityEngine;

using DeviGames.Atlas.Core.Execution.Models;
using DeviGames.Atlas.Core.Execution.Services;
using DeviGames.Atlas.Core.Execution.Interfaces;

namespace DeviGames.Atlas.Unity.Execution.Runtime
{
    /// <summary>
    /// Bridges Unity's player-loop callbacks into Atlas execution.
    ///
    /// This behaviour knows only about Core.Execution.
    /// It does not know which systems are registered.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AtlasExecutionBehaviour :
        MonoBehaviour
    {
        private IExecutionService _executionService;

        private ulong _updateFrame;
        private ulong _fixedUpdateFrame;
        private ulong _lateUpdateFrame;

        private bool _isInitialized;

        public bool IsInitialized =>
            _isInitialized;

        public void Initialize(
            IExecutionService executionService)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException(
                    $"{nameof(AtlasExecutionBehaviour)} " +
                    "has already been initialized.");
            }

            _executionService =
                executionService
                ?? throw new ArgumentNullException(
                    nameof(executionService));

            _isInitialized = true;
        }

        private void Update()
        {
            EnsureInitialized();

            var context =
                new ExecutionContext(
                    Time.deltaTime,
                    _updateFrame);

            _executionService.Update(
                context);

            _updateFrame++;
        }

        private void FixedUpdate()
        {
            EnsureInitialized();

            var context =
                new ExecutionContext(
                    Time.fixedDeltaTime,
                    _fixedUpdateFrame);

            _executionService.FixedUpdate(
                context);

            _fixedUpdateFrame++;
        }

        private void LateUpdate()
        {
            EnsureInitialized();

            var context =
                new ExecutionContext(
                    Time.deltaTime,
                    _lateUpdateFrame);

            _executionService.LateUpdate(
                context);

            _lateUpdateFrame++;
        }

        private void EnsureInitialized()
        {
            if (!_isInitialized ||
                _executionService == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(AtlasExecutionBehaviour)} " +
                    "has not been initialized.");
            }
        }
    }
}