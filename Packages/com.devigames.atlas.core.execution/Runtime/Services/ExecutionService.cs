using System;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Execution.Models;
using DeviGames.Atlas.Core.Execution.Systems;

namespace DeviGames.Atlas.Core.Execution.Services
{
    public sealed class ExecutionService :
        IExecutionService
    {
        private readonly ISystemCollection _systems;

        public ExecutionService(
            ISystemCollection systems)
        {
            _systems =
                systems
                ?? throw new ArgumentNullException(
                    nameof(systems));
        }

        public void Update(
            ExecutionContext context)
        {
            var systems = _systems.Systems;

            for (int index = 0;
                 index < systems.Count;
                 index++)
            {
                if (systems[index] is IUpdatable updatable)
                {
                    updatable.Update(context);
                }
            }
        }

        public void FixedUpdate(
            ExecutionContext context)
        {
            var systems = _systems.Systems;

            for (int index = 0;
                 index < systems.Count;
                 index++)
            {
                if (systems[index] is IFixedUpdatable fixedUpdatable)
                {
                    fixedUpdatable.FixedUpdate(context);
                }
            }
        }

        public void LateUpdate(
            ExecutionContext context)
        {
            var systems = _systems.Systems;

            for (int index = 0;
                 index < systems.Count;
                 index++)
            {
                if (systems[index] is ILateUpdatable lateUpdatable)
                {
                    lateUpdatable.LateUpdate(context);
                }
            }
        }
    }
}