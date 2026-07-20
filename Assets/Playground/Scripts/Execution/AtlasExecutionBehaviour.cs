using DeviGames.Atlas.Core.Execution.Models;
using DeviGames.Atlas.Core.Execution.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Execution.Interfaces;
using UnityEngine;

namespace DeviGames.Playground.Execution
{
    public sealed class AtlasExecutionBehaviour :
        MonoBehaviour
    {
        private IExecutionService _executionService;

        private ulong _updateFrame;
        private ulong _fixedFrame;
        private ulong _lateFrame;

        private void Start()
        {
            _executionService =
                Services.Resolve<IExecutionService>();
        }

        private void Update()
        {
            if (_executionService == null)
                return;

            var context =
                new ExecutionContext(
                    Time.deltaTime,
                    _updateFrame++);

            _executionService.Update(context);
        }

        private void FixedUpdate()
        {
            if (_executionService == null)
                return;

            var context =
                new ExecutionContext(
                    Time.fixedDeltaTime,
                    _fixedFrame++);

            _executionService.FixedUpdate(context);
        }

        private void LateUpdate()
        {
            if (_executionService == null)
                return;

            var context =
                new ExecutionContext(
                    Time.deltaTime,
                    _lateFrame++);

            _executionService.LateUpdate(context);
        }
    }
}