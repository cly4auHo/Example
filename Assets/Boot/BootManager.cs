using Background;
using GameManagment;
using Generator;
using Injection;
using Leaderboard;
using Network;
using UI;
using UnityEngine;
using Zenject;

namespace Boot
{
    public class BootManager : MonoBehaviour
    {
        [Inject] private IBackgroundSystem _backgroundSystem;
        [Inject] private ILeaderboardSystem _leaderboardSystem;
        [Inject] private IServerApi _serverApi;
        [Inject] private IExampleGenerator _exampleGenerator;
        [Inject] private IUIManager _uiManager;
        [Inject] private IGameManager _gameManager;
        [Inject] private IGameInstaller _gameInstaller;
        
        private async void Start()
        {
            var model = await _serverApi.GetGameModel();
            
            _backgroundSystem.Init();
            _leaderboardSystem.Init();
            _gameInstaller.BindModel(model);
            _exampleGenerator.Init(model.AmountOfAnswers);
            _uiManager.Init();
            
            _gameManager.Restart();
        }
    }
}