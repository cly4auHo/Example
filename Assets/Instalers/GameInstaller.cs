using Background;
using Boot;
using Content;
using Generator;
using Leaderboard;
using Network;
using UnityEngine;
using Zenject;

namespace Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private ContentManager _contentManager;
        [SerializeField] private Widget _widget;

        public override void InstallBindings()
        {
            Container.Bind<IContentManager>().FromInstance(_contentManager);
            Container.Bind<GameManager>().FromInstance(_gameManager);
            Container.Bind<Widget>().FromInstance(_widget);

            Container.Bind<ILeaderboardSystem>().To<LeaderboardSystem>().AsSingle();
            Container.Bind<IBackgroundSystem>().To<BackgroundSystem>().AsSingle();
            Container.Bind<IServerApi>().To<ServerApiStub>().AsSingle();
            Container.Bind<IExampleGenerator>().To<ExampleGenerator>().AsSingle();
        }
    }
}