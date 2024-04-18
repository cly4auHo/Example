using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Widget : MonoBehaviour
{
    [Header("Main view")]
    [SerializeField] Image _background;
    [SerializeField] GameObject _newGameField;
    [SerializeField] Button _newGame;
    [SerializeField] Button _leaderBoard;
    [SerializeField] Button _backGround;
    
    [Header("Prefabs")]
    [SerializeField] Transform _container;
    [SerializeField] BackgroundWidget _backgroundWidget;
    [SerializeField] LeaderBoardWidget _leaderBoardWidget;
    [SerializeField] GameFieldWidget _gameFieldWidget;
    
    [Inject] IBackgroundSystem _backgroundSystem;
    [Inject] ILeaderboardSystem _leaderboardSystem;

    GameModel _model;
    
    public void Init(GameModel model)
    {
        _model = model;

        SetBackground();
        
        _newGame.onClick.AddListener(NewGameClickHandler);
        _leaderBoard.onClick.AddListener(LeaderboardClickHandler);
        _backGround.onClick.AddListener(BackgroundClickHandler);
    }

    void NewGameClickHandler()
    {
        _newGameField.SetActive(false);

        var gameField = Instantiate(_gameFieldWidget, _container);

        gameField.Closed += CloseGame;
        gameField.Init(_leaderboardSystem, _model);
    }

    void LeaderboardClickHandler()
    {
        DeactivateMainWidget();

        var leaderboardWidget = Instantiate(_leaderBoardWidget, _container);

        leaderboardWidget.Closed += CloseLeaderboard;
        leaderboardWidget.Init(_leaderboardSystem);
    }
    
    void BackgroundClickHandler()
    {
        DeactivateMainWidget();

        var backgroundWidget = Instantiate(_backgroundWidget, _container);

        backgroundWidget.Closed += ChoseNewBackground;
        backgroundWidget.Init(_backgroundSystem);
    }

    void ActivateMainWidget() => _newGameField.SetActive(true);

    void DeactivateMainWidget() => _newGameField.SetActive(false);

    void CloseGame(BaseWidget widget)
    {
        widget.Closed -= CloseGame;
        ActivateMainWidget();
    }
    
    void CloseLeaderboard(BaseWidget widget)
    {
        widget.Closed -= CloseLeaderboard;
        ActivateMainWidget();
    }
    
    void ChoseNewBackground(BaseWidget widget)
    {
        widget.Closed -= ChoseNewBackground;
        ActivateMainWidget();
        _background.sprite = _backgroundSystem.CurrentBackground;
    }

    async void SetBackground()
    {
        await Awaiters.Until(() => _backgroundSystem.Initialized);
        
        if (_backgroundSystem.CurrentBackground)
            _background.sprite = _backgroundSystem.CurrentBackground;
    }
    
    void OnDestroy()
    {
        _newGame.onClick.RemoveListener(NewGameClickHandler);
        _leaderBoard.onClick.RemoveListener(LeaderboardClickHandler);
        _backGround.onClick.RemoveListener(BackgroundClickHandler);
    }
}
