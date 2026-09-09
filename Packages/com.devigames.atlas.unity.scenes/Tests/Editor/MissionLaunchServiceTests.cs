using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Services;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;

using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Models;
using DeviGames.Atlas.Gameplay.Progression.Services;

using DeviGames.Atlas.Unity.Scenes.Interfaces;
using DeviGames.Atlas.Unity.Scenes.Models;
using DeviGames.Atlas.Unity.Scenes.Services;

namespace DeviGames.Atlas.Unity.Scenes.Tests
{
    public sealed class MissionLaunchServiceTests
    {
        private MissionCollection _missionCollection;
        private FakeMissionSessionService _sessionService;
        private FakeMissionResultService _resultService;
        private IGameFlowService _gameFlowService;
        private MissionFlowCoordinator _missionFlowCoordinator;
        private FakeContentDownloadService _contentDownloadService;
        private FakeSceneService _sceneService;
        private MissionLaunchService _launchService;

        [SetUp]
        public void SetUp()
        {
            _missionCollection =
                new MissionCollection();

            _sessionService =
                new FakeMissionSessionService();

            _resultService =
                new FakeMissionResultService();

            _gameFlowService =
                new GameFlowService();

            _missionFlowCoordinator =
                new MissionFlowCoordinator(
                    _sessionService,
                    _gameFlowService,
                    _resultService);

            _missionFlowCoordinator.Initialize();

            _gameFlowService.EnterMainMenu();

            _contentDownloadService =
                new FakeContentDownloadService();

            _sceneService =
                new FakeSceneService();

            _launchService =
                new MissionLaunchService(
                    _missionFlowCoordinator,
                    _missionCollection,
                    _contentDownloadService,
                    _sceneService);
        }

        [TearDown]
        public void TearDown()
        {
            _missionFlowCoordinator.Shutdown();
        }

        [Test]
        public async Task LaunchAsync_UnknownMission_ReturnsMissionNotFound()
        {
            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.unknown");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.MissionNotFound));

            Assert.That(
                _sessionService.StartCallCount,
                Is.Zero);

            Assert.That(
                _sceneService.LoadCallCount,
                Is.Zero);
        }

        [Test]
        public async Task LaunchAsync_MissingSceneKey_ReturnsMissingSceneKey()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "",
                contentKey: "content.test");

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.MissingSceneKey));

            Assert.That(
                _contentDownloadService.GetDownloadSizeCallCount,
                Is.Zero);

            Assert.That(
                _sessionService.StartCallCount,
                Is.Zero);

            Assert.That(
                _sceneService.LoadCallCount,
                Is.Zero);
        }

        [Test]
        public async Task LaunchAsync_MissingContentKey_ReturnsMissingContentKey()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "");

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.MissingContentKey));

            Assert.That(
                _contentDownloadService.GetDownloadSizeCallCount,
                Is.Zero);

            Assert.That(
                _sessionService.StartCallCount,
                Is.Zero);

            Assert.That(
                _sceneService.LoadCallCount,
                Is.Zero);
        }

        [Test]
        public async Task LaunchAsync_NoDownloadRequired_StartsMissionAndLoadsScene()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _contentDownloadService.DownloadSize =
                0;

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.Success));

            Assert.That(
                _contentDownloadService.GetDownloadSizeCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _contentDownloadService.DownloadCallCount,
                Is.Zero);

            Assert.That(
                _sessionService.StartCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sceneService.LoadCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sceneService.LastSceneName,
                Is.EqualTo(
                    "scene.test"));
        }

        [Test]
        public async Task LaunchAsync_DownloadRequired_DownloadsContent()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _contentDownloadService.DownloadSize =
                1024;

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.Success));

            Assert.That(
                _contentDownloadService.GetDownloadSizeCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _contentDownloadService.DownloadCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _contentDownloadService.LastDownloadedContentKey,
                Is.EqualTo(
                    "content.test"));

            Assert.That(
                _sessionService.StartCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sceneService.LoadCallCount,
                Is.EqualTo(
                    1));
        }

        [Test]
        public async Task LaunchAsync_GetDownloadSizeFails_ReturnsContentDownloadFailed()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _contentDownloadService.ThrowOnGetDownloadSize =
                true;

            LogAssert.Expect(
                LogType.Exception,
                new Regex(
                    "InvalidOperationException: Download size failed."));

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.ContentDownloadFailed));

            Assert.That(
                _sessionService.StartCallCount,
                Is.Zero);

            Assert.That(
                _sceneService.LoadCallCount,
                Is.Zero);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public async Task LaunchAsync_DownloadFails_ReturnsContentDownloadFailed()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _contentDownloadService.DownloadSize =
                1024;

            _contentDownloadService.ThrowOnDownload =
                true;

            LogAssert.Expect(
                LogType.Exception,
                new Regex(
                    "InvalidOperationException: Download failed."));

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.ContentDownloadFailed));

            Assert.That(
                _contentDownloadService.DownloadCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sessionService.StartCallCount,
                Is.Zero);

            Assert.That(
                _sceneService.LoadCallCount,
                Is.Zero);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public async Task LaunchAsync_SceneLoadFails_ReturnsSceneLoadFailed()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _sceneService.ThrowOnLoad =
                true;

            LogAssert.Expect(
                LogType.Exception,
                new Regex(
                    "InvalidOperationException: Scene loading failed."));

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.SceneLoadFailed));
        }

        [Test]
        public async Task LaunchAsync_SceneLoadFails_RollsBackMissionLaunch()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _sceneService.ThrowOnLoad =
                true;

            LogAssert.Expect(
                LogType.Exception,
                new Regex(
                    "InvalidOperationException: Scene loading failed."));

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.SceneLoadFailed));

            Assert.That(
                _sessionService.ExitCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sessionService.HasActiveSession,
                Is.False);

            Assert.That(
                _missionFlowCoordinator.HasMission,
                Is.False);

            Assert.That(
                _missionFlowCoordinator.MissionId,
                Is.Empty);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }
        
        [Test]
        public async Task LaunchAsync_MissionStartRejected_ReturnsMissionStartRejected()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _sessionService.AllowStart =
                false;

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.MissionStartRejected));

            Assert.That(
                _sessionService.StartCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sceneService.LoadCallCount,
                Is.Zero);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        
        [Test]
        public async Task LaunchAsync_Success_LeavesFlowInMissionIntro()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.Success));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionIntro));

            Assert.That(
                _missionFlowCoordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));

            Assert.That(
                _sessionService.HasActiveSession,
                Is.True);

            Assert.That(
                _sessionService.ActiveMissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public async Task LaunchAsync_Success_ClearsPreviousMissionResult()
        {
            RegisterMission(
                missionId: "mission.test",
                sceneKey: "scene.test",
                contentKey: "content.test");

            _resultService.SetResult(
                "mission.previous");

            MissionLaunchResult result =
                await _launchService.LaunchAsync(
                    "mission.test");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionLaunchResult.Success));

            Assert.That(
                _resultService.HasResult,
                Is.False);

            Assert.That(
                _resultService.ClearCallCount,
                Is.EqualTo(
                    1));
        }

        private void RegisterMission(
            string missionId,
            string sceneKey,
            string contentKey)
        {
            var missionDefinition =
                new MissionDefinition(
                    id: missionId,
                    displayName: missionId,
                    description: "",
                    objectiveIds: new[]
                    {
                        "objective.test"
                    },
                    sceneKey: sceneKey,
                    contentKey: contentKey);

            var missionRuntime =
                new MissionRuntime(
                    missionDefinition);

            _missionCollection.Add(
                missionRuntime);
        }

        private sealed class FakeContentDownloadService :
            IContentDownloadService
        {
            public long DownloadSize { get; set; }

            public bool ThrowOnGetDownloadSize { get; set; }
            public bool ThrowOnDownload { get; set; }

            public int GetDownloadSizeCallCount { get; private set; }
            public int DownloadCallCount { get; private set; }

            public string LastDownloadedContentKey { get; private set; } =
                string.Empty;

            public Task<long> GetDownloadSizeAsync(
                string contentKey)
            {
                GetDownloadSizeCallCount++;

                if (ThrowOnGetDownloadSize)
                {
                    throw new InvalidOperationException(
                        "Download size failed.");
                }

                return Task.FromResult(
                    DownloadSize);
            }

            public Task DownloadAsync(
                string contentKey,
                IProgress<float> progress = null)
            {
                DownloadCallCount++;

                LastDownloadedContentKey =
                    contentKey;

                if (ThrowOnDownload)
                {
                    throw new InvalidOperationException(
                        "Download failed.");
                }

                return Task.CompletedTask;
            }
        }

        private sealed class FakeSceneService :
            ISceneService
        {
            public string ActiveSceneName { get; private set; } =
                string.Empty;

            public bool ThrowOnLoad { get; set; }

            public int LoadCallCount { get; private set; }

            public string LastSceneName { get; private set; } =
                string.Empty;

            public Task LoadAsync(
                string sceneName)
            {
                LoadCallCount++;

                LastSceneName =
                    sceneName;

                if (ThrowOnLoad)
                {
                    throw new InvalidOperationException(
                        "Scene loading failed.");
                }

                ActiveSceneName =
                    sceneName;

                return Task.CompletedTask;
            }
        }

        private sealed class FakeMissionSessionService :
            IMissionSessionService
        {
            public string ActiveMissionId { get; private set; } =
                string.Empty;

            public bool HasActiveSession =>
                !string.IsNullOrWhiteSpace(
                    ActiveMissionId);

            public bool AllowStart { get; set; } =
                true;

            public int StartCallCount { get; private set; }
            public int ExitCallCount { get; private set; }

            public bool Start(
                string missionId)
            {
                StartCallCount++;

                if (!AllowStart)
                {
                    return false;
                }

                if (HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    missionId;

                return true;
            }

            public bool Restart()
            {
                return HasActiveSession;
            }

            public bool Fail()
            {
                if (!HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    string.Empty;

                return true;
            }

            public bool Exit()
            {
                ExitCallCount++;

                if (!HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    string.Empty;

                return true;
            }
        }

        private sealed class FakeMissionResultService :
            IMissionResultService
        {
            public MissionResult CurrentResult { get; private set; }

            public bool HasResult =>
                CurrentResult != null;

            public int ClearCallCount { get; private set; }

            public bool TryGetResult(
                out MissionResult result)
            {
                result =
                    CurrentResult;

                return result != null;
            }

            public void Clear()
            {
                ClearCallCount++;

                CurrentResult =
                    null;
            }

            public void SetResult(
                string missionId)
            {
                CurrentResult =
                    new MissionResult(
                        missionId,
                        true);
            }
        }
    }
}