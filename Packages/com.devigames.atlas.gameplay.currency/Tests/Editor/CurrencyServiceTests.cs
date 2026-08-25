using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Gameplay.Currency.Events;
using DeviGames.Atlas.Gameplay.Currency.Services;

namespace DeviGames.Atlas.Gameplay.Currency.Tests
{
    public sealed class CurrencyServiceTests
    {
        private CurrencyService _service;

        [SetUp]
        public void SetUp()
        {
            _service =
                new CurrencyService();
        }

        [Test]
        public void GetBalance_UnknownCurrency_ReturnsZero()
        {
            Assert.That(
                _service.GetBalance("coins"),
                Is.Zero);
        }

        [Test]
        public void Add_PositiveAmount_IncreasesBalance()
        {
            bool result =
                _service.Add(
                    "coins",
                    100);

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.GetBalance("coins"),
                Is.EqualTo(100));
        }

        [Test]
        public void Add_InvalidAmount_ReturnsFalse()
        {
            bool result =
                _service.Add(
                    "coins",
                    0);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _service.GetBalance("coins"),
                Is.Zero);
        }

        [Test]
        public void Spend_EnoughBalance_DecreasesBalance()
        {
            _service.Add(
                "coins",
                100);

            bool result =
                _service.Spend(
                    "coins",
                    30);

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.GetBalance("coins"),
                Is.EqualTo(70));
        }

        [Test]
        public void Spend_InsufficientBalance_ReturnsFalse()
        {
            _service.Add(
                "coins",
                20);

            bool result =
                _service.Spend(
                    "coins",
                    30);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _service.GetBalance("coins"),
                Is.EqualTo(20));
        }

        [Test]
        public void Add_PublishesCurrencyChangedEvent()
        {
            CurrencyChangedEvent? received =
                null;

            void Handler(
                CurrencyChangedEvent eventData)
            {
                received =
                    eventData;
            }

            EventBus.Subscribe<
                CurrencyChangedEvent>(
                    Handler);

            try
            {
                _service.Add(
                    "coins",
                    50);

                Assert.That(
                    received.HasValue,
                    Is.True);

                Assert.That(
                    received.Value.CurrencyId,
                    Is.EqualTo("coins"));

                Assert.That(
                    received.Value.PreviousBalance,
                    Is.Zero);

                Assert.That(
                    received.Value.CurrentBalance,
                    Is.EqualTo(50));

                Assert.That(
                    received.Value.Delta,
                    Is.EqualTo(50));
            }
            finally
            {
                EventBus.Unsubscribe<
                    CurrencyChangedEvent>(
                        Handler);
            }
        }

        [Test]
        public void Spend_PublishesNegativeDelta()
        {
            _service.Add(
                "coins",
                100);

            CurrencyChangedEvent? received =
                null;

            void Handler(
                CurrencyChangedEvent eventData)
            {
                received =
                    eventData;
            }

            EventBus.Subscribe<
                CurrencyChangedEvent>(
                    Handler);

            try
            {
                _service.Spend(
                    "coins",
                    25);

                Assert.That(
                    received.HasValue,
                    Is.True);

                Assert.That(
                    received.Value.PreviousBalance,
                    Is.EqualTo(100));

                Assert.That(
                    received.Value.CurrentBalance,
                    Is.EqualTo(75));

                Assert.That(
                    received.Value.Delta,
                    Is.EqualTo(-25));
            }
            finally
            {
                EventBus.Unsubscribe<
                    CurrencyChangedEvent>(
                        Handler);
            }
        }

        [Test]
        public void Spend_InsufficientBalance_DoesNotPublishEvent()
        {
            int eventCount = 0;

            void Handler(
                CurrencyChangedEvent eventData)
            {
                eventCount++;
            }

            EventBus.Subscribe<
                CurrencyChangedEvent>(
                    Handler);

            try
            {
                _service.Spend(
                    "coins",
                    10);

                Assert.That(
                    eventCount,
                    Is.Zero);
            }
            finally
            {
                EventBus.Unsubscribe<
                    CurrencyChangedEvent>(
                        Handler);
            }
        }
    }
}