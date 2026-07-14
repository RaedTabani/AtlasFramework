using System.Collections.Generic;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Lifecycle;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using NUnit.Framework;

namespace DeviGames.Atlas.Core.Services.Tests
{
    public sealed class ServiceLifecycleTests
    {
        [Test]
        public async Task InitializeAsync_Should_Initialize_In_Registration_Order()
        {
            var order = new List<string>();
            var registry = new ServiceRegistry();

            registry.Register(new FirstLifecycleService(order));
            registry.Register(new SecondLifecycleService(order));

            await registry.InitializeAsync();

            CollectionAssert.AreEqual(
                new[]
                {
                    "Initialize:A",
                    "Initialize:B"
                },
                order);
        }

        [Test]
        public async Task Shutdown_Should_Run_In_Reverse_Order()
        {
            var order = new List<string>();
            var registry = new ServiceRegistry();

            registry.Register(new FirstLifecycleService(order));
            registry.Register(new SecondLifecycleService(order));

            await registry.InitializeAsync();

            order.Clear();

            registry.Shutdown();

            CollectionAssert.AreEqual(
                new[]
                {
                    "Shutdown:B",
                    "Shutdown:A"
                },
                order);
        }

        [Test]
        public async Task InitializeAsync_CalledTwice_Should_Not_InitializeTwice()
        {
            var order = new List<string>();
            var registry = new ServiceRegistry();

            registry.Register(new FirstLifecycleService(order));

            await registry.InitializeAsync();
            await registry.InitializeAsync();

            CollectionAssert.AreEqual(
                new[]
                {
                    "Initialize:A"
                },
                order);
        }

        [Test]
        public async Task Register_After_Initialization_Should_Throw()
        {
            var registry = new ServiceRegistry();

            registry.Register(
                new FirstLifecycleService(
                    new List<string>()));

            await registry.InitializeAsync();

            Assert.Throws<System.InvalidOperationException>(() =>
            {
                registry.Register(
                    new SecondLifecycleService(
                        new List<string>()));
            });
        }

        private sealed class FirstLifecycleService :
            IInitializable,
            IShutdownable
        {
            private readonly List<string> _order;

            public FirstLifecycleService(List<string> order)
            {
                _order = order;
            }

            public void Initialize()
            {
                _order.Add("Initialize:A");
            }

            public void Shutdown()
            {
                _order.Add("Shutdown:A");
            }
        }

        private sealed class SecondLifecycleService :
            IInitializable,
            IShutdownable
        {
            private readonly List<string> _order;

            public SecondLifecycleService(List<string> order)
            {
                _order = order;
            }

            public void Initialize()
            {
                _order.Add("Initialize:B");
            }

            public void Shutdown()
            {
                _order.Add("Shutdown:B");
            }
        }
    }
}