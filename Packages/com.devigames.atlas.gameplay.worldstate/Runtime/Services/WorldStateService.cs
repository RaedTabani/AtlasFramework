using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Gameplay.WorldState.Models;
using DeviGames.Atlas.Gameplay.WorldState.Events;
using DeviGames.Atlas.Gameplay.WorldState.Interfaces;

namespace DeviGames.Atlas.Gameplay.WorldState.Services
{
    public sealed class WorldStateService :
        IWorldStateService
    {
        private readonly Dictionary<string, bool> _states = new(StringComparer.Ordinal);

        public bool Get(string key)
        {
            ValidateKey(key);

            return
                _states.TryGetValue(key,out bool value) && value;
        }

        public bool Contains(
            string key)
        {
            ValidateKey(
                key);

            return _states.ContainsKey(
                key);
        }

        public bool Set(
            string key,
            bool value)
        {
            ValidateKey(
                key);

            bool previousValue =
                Get(
                    key);

            if (_states.ContainsKey(
                    key) &&
                previousValue == value)
            {
                return false;
            }

            _states[key] =
                value;

            EventBus.Publish(
                new WorldStateChangedEvent(
                    key,
                    previousValue,
                    value));

            return true;
        }

        private static void ValidateKey(
            string key)
        {
            if (string.IsNullOrWhiteSpace(
                    key))
            {
                throw new ArgumentException(
                    "World state key cannot be empty.",
                    nameof(key));
            }
        }

        public WorldStateData CreateSnapshot()
        {
            var data =
                new WorldStateData();

            foreach (
                KeyValuePair<string, bool> pair
                in _states)
            {
                data.Entries.Add(
                    new WorldStateEntryData
                    {
                        Key =
                            pair.Key,

                        Value =
                            pair.Value
                    });
            }

            return data;
        }

        public void Load(
            WorldStateData data)
        {
            _states.Clear();

            if (data == null ||
                data.Entries == null)
            {
                return;
            }

            foreach (
                WorldStateEntryData entry
                in data.Entries)
            {
                if (entry == null ||
                    string.IsNullOrWhiteSpace(
                        entry.Key))
                {
                    continue;
                }

                _states[entry.Key] =
                    entry.Value;
            }
        }
    }
}