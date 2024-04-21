using System;
using Leaderboard;
using Network;
using UI;
using Zenject;

namespace GameManagment
{
    public class GameManager : IGameManager
    {
        public event Action RestartGame;
        
        [Inject] private IUIManager _uiManager;
        [Inject] private ILeaderboardSystem _leaderboardSystem;

        private int _score;
        
        public void StartGame() => _uiManager.Present(WidgetName.Game);
        public void OpenLeaderBoard() => _uiManager.Present(WidgetName.Leaderboard);
        public void OpenBackground() => _uiManager.Present(WidgetName.Background);

        public void EndGame(in int score)
        {
            _score = score;
            _uiManager.PresentWithData(WidgetName.End, _leaderboardSystem.IsNewRecord(score));
        }

        public void Restart()
        {
            RestartGame?.Invoke();
            _uiManager.Present(WidgetName.Menu);
        }

        public void Submit(string name)
            => _leaderboardSystem.AddUser(new LeaderboardUserModel {name = name, score = _score});   
    }
}