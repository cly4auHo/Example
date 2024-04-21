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
        [SerializeField] private ContentManager _contentManager;
        [SerializeField] private UIManager _uiManager;

        public override void InstallBindings()
        {
            Container.Bind<GameInstaller>().FromInstance(this);
            
            Container.Bind<IContentManager>().FromInstance(_contentManager);
            Container.Bind<IUIManager>().FromInstance(_uiManager);
            Container.Bind<BootManager>().FromInstance(_bootManager);
            
            BindSystems();
        }
        
        public void BindModel(GameModel model) => Container.Bind<GameModel>().FromInstance(model);
        
        private void BindSystems()
        {
            Container.Bind<IServerApi>().To<ServerApiStub>().AsSingle();
            Container.Bind<ILeaderboardSystem>().To<LeaderboardSystem>().AsSingle();
            Container.Bind<IBackgroundSystem>().To<BackgroundSystem>().AsSingle();
            Container.Bind<IExampleGenerator>().To<ExampleGenerator>().AsSingle();
            Container.Bind<IGameManager>().To<GameManager>().AsSingle();
        }
    }
}