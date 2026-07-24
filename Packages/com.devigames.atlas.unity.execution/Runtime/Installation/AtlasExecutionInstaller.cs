using System;
using UnityEngine;

using DeviGames.Atlas.Core.Execution.Services;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Unity.Execution.Runtime;

namespace DeviGames.Atlas.Unity.Execution.Installation
{
    /// <summary>
    /// Installs the Unity bridge that drives Core.Execution.
    /// </summary>
    public sealed class AtlasExecutionInstaller
    {
        private const string ExecutionObjectName =
            "[Atlas Execution]";

        public AtlasExecutionBehaviour Install(
            IExecutionService executionService)
        {
            if (executionService == null)
            {
                throw new ArgumentNullException(
                    nameof(executionService));
            }

            AtlasExecutionBehaviour existingBehaviour =
                UnityEngine.Object.FindFirstObjectByType<
                    AtlasExecutionBehaviour>();

            if (existingBehaviour != null)
            {
                throw new InvalidOperationException(
                    $"{nameof(AtlasExecutionBehaviour)} " +
                    "is already installed in the active application.");
            }

            var gameObject =
                new GameObject(
                    ExecutionObjectName);

            UnityEngine.Object.DontDestroyOnLoad(
                gameObject);

            AtlasExecutionBehaviour behaviour =
                gameObject.AddComponent<
                    AtlasExecutionBehaviour>();

            behaviour.Initialize(
                executionService);

            return behaviour;
        }
    }
}