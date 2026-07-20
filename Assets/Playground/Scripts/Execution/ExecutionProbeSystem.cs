using DeviGames.Atlas.Core.Execution.Models;
using DeviGames.Atlas.Core.Execution.Interfaces;
using UnityEngine;

namespace DeviGames.Playground.Execution
{    public sealed class ExecutionProbeSystem :
        IUpdatable,
        IFixedUpdatable,
        ILateUpdatable
    {
        public void Update(
            ExecutionContext context)
        {
            Debug.Log(
                $"Update {context.Frame}: {context.DeltaTime}");
        }

        public void FixedUpdate(
            ExecutionContext context)
        {
            Debug.Log(
                $"Fixed {context.Frame}: {context.DeltaTime}");
        }

        public void LateUpdate(
            ExecutionContext context)
        {
            Debug.Log(
                $"Late {context.Frame}: {context.DeltaTime}");
        }
    }
}