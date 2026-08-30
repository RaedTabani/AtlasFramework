using System;
using System.Collections.Generic;

using DeviGames.Atlas.Unity.Scenes.Interfaces;
using DeviGames.Atlas.Unity.Scenes.Models;

namespace DeviGames.Atlas.Unity.Scenes.Services
{
    public sealed class MissionSceneResolver :
        IMissionSceneResolver
    {
        private readonly Dictionary<string, string> _scenes =
            new(StringComparer.Ordinal);

        public MissionSceneResolver(
            IEnumerable<MissionSceneDefinition> definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException(
                    nameof(definitions));
            }

            foreach (MissionSceneDefinition definition in definitions)
            {
                if (definition == null ||
                    string.IsNullOrWhiteSpace(definition.MissionId) ||
                    string.IsNullOrWhiteSpace(definition.SceneName))
                {
                    continue;
                }

                _scenes[definition.MissionId] =
                    definition.SceneName;
            }
        }

        public bool TryGetSceneName(
            string missionId,
            out string sceneName)
        {
            if (string.IsNullOrWhiteSpace(missionId))
            {
                sceneName = null;

                return false;
            }

            return _scenes.TryGetValue(
                missionId,
                out sceneName);
        }
    }
}