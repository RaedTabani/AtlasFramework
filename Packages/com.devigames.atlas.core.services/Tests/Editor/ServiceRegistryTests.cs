using System;
using NUnit.Framework;

namespace DeviGames.Atlas.Core.Services.Tests
{
    public class ServiceRegistryTests
    {
        private ServiceRegistry _registry;

        [SetUp]
        public void Setup()
        {
            _registry = new ServiceRegistry();
        }

        [Test]
        public void Register_ThenResolve_Should_Return_Registered_Service()
        {
            var service = new TestService();

            _registry.Register(service);

            TestService resolved = _registry.Resolve<TestService>();

            Assert.AreSame(service, resolved);
        }

        [Test]
        public void TryResolve_When_Service_Exists_Should_Return_True()
        {
            var service = new TestService();

            _registry.Register(service);

            bool result = _registry.TryResolve(out TestService resolved);

            Assert.IsTrue(result);
            Assert.AreSame(service, resolved);
        }

        [Test]
        public void TryResolve_When_Service_Missing_Should_Return_False()
        {
            bool result = _registry.TryResolve(out TestService resolved);

            Assert.IsFalse(result);
            Assert.IsNull(resolved);
        }

        [Test]
        public void Resolve_When_Service_Missing_Should_Throw()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _registry.Resolve<TestService>();
            });
        }

        [Test]
        public void Register_Duplicate_Service_Should_Throw()
        {
            _registry.Register(new TestService());

            Assert.Throws<InvalidOperationException>(() =>
            {
                _registry.Register(new TestService());
            });
        }

        [Test]
        public void Unregister_Should_Remove_Service()
        {
            _registry.Register(new TestService());

            _registry.Unregister<TestService>();

            Assert.IsFalse(_registry.IsRegistered<TestService>());
        }

        [Test]
        public void Clear_Should_Remove_All_Services()
        {
            _registry.Register(new TestService());
            _registry.Register<ITestInterface>(new InterfaceTestService());

            _registry.Reset();

            Assert.IsFalse(_registry.IsRegistered<TestService>());
            Assert.IsFalse(_registry.IsRegistered<ITestInterface>());
        }

        [Test]
        public void Register_By_Interface_Should_Resolve_Interface()
        {
            ITestInterface service = new InterfaceTestService();

            _registry.Register<ITestInterface>(service);

            ITestInterface resolved = _registry.Resolve<ITestInterface>();

            Assert.AreSame(service, resolved);
        }

        private sealed class TestService { }

        private interface ITestInterface { }

        private sealed class InterfaceTestService : ITestInterface { }
    }
}