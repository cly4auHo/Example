using Background;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LobbyWidget : BaseWidget
    {
        [Header("Main view")] 
        [SerializeField] private Image _background;
        [SerializeField] private Button _newGame;
        [SerializeField] private Button _leaderBoard;
        [SerializeField] private Button _backGround;
        
        [Inject] private IBackgroundSystem _backgroundSystem;

        public override void OnCreated()
        {
            SetBackground();

            _backgroundSystem.NewBackground += NewBackground;
            _newGame.onClick.AddListener(NewGameClickHandler);
            _leaderBoard.onClick.AddListener(LeaderboardClickHandler);
            _backGround.onClick.AddListener(BackgroundClickHandler);
        }

        public override void OnClosed()
        {
            _backgroundSystem.NewBackground -= NewBackground;
            _newGame.onClick.RemoveListener(NewGameClickHandler);
            _leaderBoard.onClick.RemoveListener(LeaderboardClickHandler);
            _backGround.onClick.RemoveListener(BackgroundClickHandler);
        }
        
        private void NewBackground(Sprite sprite) => _background.sprite = sprite;
        
        private void NewGameClickHandler()
        {
            _gameManager.StartGame();
            Closed?.Invoke(this);
        }

        private void LeaderboardClickHandler() => _gameManager.OpenLeaderBoard();
        
        private void BackgroundClickHandler() => _gameManager.OpenBackground();
        
        private async void SetBackground()
        {
            await Awaiters.Until(() => _backgroundSystem.Initialized);

            if (_backgroundSystem.CurrentBackground)
                NewBackground(_backgroundSystem.CurrentBackground);
        }
    }
}