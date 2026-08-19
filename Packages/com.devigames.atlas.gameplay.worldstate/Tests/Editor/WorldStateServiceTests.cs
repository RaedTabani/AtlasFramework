using UnityEngine;
using NUnit.Framework;
using DeviGames.Atlas.Gameplay.WorldState.Services;

namespace DeviGames.Atlas.Gameplay.WorldState.Tests
{
    public class WorldStateServiceTests
    {
        [Test]
        public void Set_NewState_StoresValue()
        {
            var service =
                new WorldStateService();

            bool changed =
                service.Set(
                    "door.exit.opened",
                    true);

            Assert.That(
                changed,
                Is.True);

            Assert.That(
                service.Get(
                    "door.exit.opened"),
                Is.True);
        }
        [Test]
        public void Get_MissingState_ReturnsFalse()
        {
            var service =
                new WorldStateService();

            Assert.That(
                service.Get(
                    "unknown"),
                Is.False);
        }

        [Test]
        public void Contains_SetState_ReturnsTrue()
        {
            var service =
                new WorldStateService();

            service.Set(
                "door.exit.opened",
                false);

            Assert.That(
                service.Contains(
                    "door.exit.opened"),
                Is.True);
        }
    }
}
