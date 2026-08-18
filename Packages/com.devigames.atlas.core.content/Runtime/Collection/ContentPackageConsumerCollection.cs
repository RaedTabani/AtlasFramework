using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Content.Interfaces;

namespace DeviGames.Atlas.Core.Content.Collections
{
    public sealed class ContentPackageConsumerCollection
    {
        private readonly List<IContentPackageConsumer>
            _consumers =
                new();

        public IReadOnlyList<IContentPackageConsumer>
            Consumers =>
                _consumers;

        public void Add(
            IContentPackageConsumer consumer)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException(
                    nameof(consumer));
            }

            if (_consumers.Contains(
                    consumer))
            {
                throw new InvalidOperationException(
                    "Content package consumer is already registered.");
            }

            _consumers.Add(
                consumer);

            _consumers.Sort(
                (left, right) =>
                    left.Order.CompareTo(
                        right.Order));
        }
    }
}