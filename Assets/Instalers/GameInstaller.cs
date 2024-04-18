using Background;
using Boot;
using Content;
using Leaderboard;
using Network;
using UnityEngine;
using Zenject;

namespace Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] ContentManager _contentManager;
        [SerializeField] Widget _widget;

        public override void InstallBindings()
        {
            Container.Bind<IContentManager>().FromInstance(_contentManager);
            Container.Bind<GameManager>().FromInstance(_gameManager);
            Container.Bind<Widget>().FromInstance(_widget);

            Container.Bind<ILeaderboardSystem>().To<LeaderboardSystem>().AsSingle();
            Container.Bind<IBackgroundSystem>().To<BackgroundSystem>().AsSingle();
            Container.Bind<IServerApi>().To<ServerApiStub>().AsSingle();
        }
    }
}