using System;

using NUnit.Framework;

using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Missions.Installation;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Missions.Tests
{
    public sealed class MissionInstallerTests
    {
        [Test]
        public void Install_RegistersMissionPackage()
        {
            var services =
                new ServiceRegistry();

            var context =
                new AtlasInstallationContext(
                    services);

            new MissionInstaller().Install(
                context);

            Assert.That(
                services.Resolve<IMissionCollection>(),
                Is.Not.Null);

            Assert.That(
                services.Resolve<IMissionFactory>(),
                Is.Not.Null);

            Assert.That(
                services.Resolve<MissionService>(),
                Is.Not.Null);
        }

        [Test]
        public void Install_Twice_Throws()
        {
            var services =
                new ServiceRegistry();

            var context =
                new AtlasInstallationContext(
                    services);

            var installer =
                new MissionInstaller();

            installer.Install(
                context);

            Assert.Throws<
                InvalidOperationException>(
                    () =>
                        installer.Install(
                            context));
        }
    }
}