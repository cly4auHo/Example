using Background;
using Boot;
using Content;
using GameManagment;
using Generator;
using Leaderboard;
using Network;
using UI;
using UnityEngine;
using Zenject;

namespace Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private BootManager _bootManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private SplashScreen _splashScreen;
        
        public override void InstallBindings()
        {
            BindSystems();
            BindOnScene();
        }
        
        public void BindModel(GameModel model) => Container.Bind<GameModel>().FromInstance(model);

        private void BindOnScene()
        {
            Container.Bind<GameInstaller>().FromInstance(this);
            Container.Bind<BootManager>().FromInstance(_bootManager);
            Container.Bind<SplashScreen>().FromInstance(_splashScreen);
            Container.Bind<IUIManager>().FromInstance(_uiManager);
        }
        
        private void BindSystems()
        {
            Container.Bind<IServerApi>().To<ServerApiStub>().AsSingle();
            Container.Bind<ILeaderboardSystem>().To<LeaderboardSystem>().AsSingle();
            Container.Bind<IBackgroundSystem>().To<BackgroundSystem>().AsSingle();
            Container.Bind<IExampleGenerator>().To<ExampleGenerator>().AsSingle();
            Container.Bind<IGameManager>().To<GameManager>().AsSingle();
            Container.Bind<IContentManager>().To<ContentManager>().AsSingle();
        }
    }
}