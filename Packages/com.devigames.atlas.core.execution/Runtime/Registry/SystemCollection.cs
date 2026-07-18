using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Execution.Interfaces;

namespace DeviGames.Atlas.Core.Execution.Systems
{
    public sealed class SystemCollection :ISystemCollection
    {
        private readonly List<object> _systems =
            new();

        public IReadOnlyList<object> Systems =>
            _systems;

        public int Count =>
            _systems.Count;

        public void Add(
            object system)
        {
            if (system == null)
            {
                throw new ArgumentNullException(
                    nameof(system));
            }

            if (!IsRuntimeSystem(system))
            {
                throw new ArgumentException(
                    "A runtime system must implement at least one execution contract.",
                    nameof(system));
            }

            if (Contains(system))
                return;

            _systems.Add(system);
        }

        public bool Remove(
            object system)
        {
            if (system == null)
                return false;

            int index =
                IndexOfReference(system);

            if (index < 0)
                return false;

            _systems.RemoveAt(index);

            return true;
        }

        public bool Contains(
            object system)
        {
            return system != null &&
                   IndexOfReference(system) >= 0;
        }

        public void Clear()
        {
            _systems.Clear();
        }

        private int IndexOfReference(
            object system)
        {
            for (int index = 0;
                 index < _systems.Count;
                 index++)
            {
                if (ReferenceEquals(
                        _systems[index],
                        system))
                {
                    return index;
                }
            }

            return -1;
        }

        private static bool IsRuntimeSystem(
            object system)
        {
            return system is IUpdatable ||
                   system is IFixedUpdatable ||
                   system is ILateUpdatable;
        }
    }
}