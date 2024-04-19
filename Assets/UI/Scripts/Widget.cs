using Background;
using Generator;
using Leaderboard;
using Network;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Widget : MonoBehaviour
{
    [Header("Main view")]
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _newGameField;
    [SerializeField] private Button _newGame;
    [SerializeField] private Button _leaderBoard;
    [SerializeField] private Button _backGround;
    
    [Header("Prefabs")]
    [SerializeField] private Transform _container;
    [SerializeField] private BackgroundWidget _backgroundWidget;
    [SerializeField] private LeaderBoardWidget _leaderBoardWidget;
    [SerializeField] private GameFieldWidget _gameFieldWidget;
    
    [Inject] private IBackgroundSystem _backgroundSystem;
    [Inject] private ILeaderboardSystem _leaderboardSystem;
    [Inject] private IExampleGenerator _exampleGenerator;
    
    private GameModel _model;
    
    public void Init(GameModel model)
    {
        _model = model;

        SetBackground();
        
        _newGame.onClick.AddListener(NewGameClickHandler);
        _leaderBoard.onClick.AddListener(LeaderboardClickHandler);
        _backGround.onClick.AddListener(BackgroundClickHandler);
    }

    private void NewGameClickHandler()
    {
        _newGameField.SetActive(false);

        var gameField = Instantiate(_gameFieldWidget, _container);

        gameField.Closed += CloseGame;
        gameField.Init(_leaderboardSystem, _exampleGenerator, _model);
    }

    private void LeaderboardClickHandler()
    {
        DeactivateMainWidget();

        var leaderboardWidget = Instantiate(_leaderBoardWidget, _container);

        leaderboardWidget.Closed += CloseLeaderboard;
        leaderboardWidget.Init(_leaderboardSystem);
    }
    
    private void BackgroundClickHandler()
    {
        DeactivateMainWidget();

        var backgroundWidget = Instantiate(_backgroundWidget, _container);

        backgroundWidget.Closed += ChoseNewBackground;
        backgroundWidget.Init(_backgroundSystem);
    }

    private void ActivateMainWidget() => _newGameField.SetActive(true);

    private void DeactivateMainWidget() => _newGameField.SetActive(false);

    private void CloseGame(BaseWidget widget)
    {
        widget.Closed -= CloseGame;
        ActivateMainWidget();
    }
    
    private void CloseLeaderboard(BaseWidget widget)
    {
        widget.Closed -= CloseLeaderboard;
        ActivateMainWidget();
    }
    
    private void ChoseNewBackground(BaseWidget widget)
    {
        widget.Closed -= ChoseNewBackground;
        ActivateMainWidget();
        _background.sprite = _backgroundSystem.CurrentBackground;
    }

    private async void SetBackground()
    {
        await Awaiters.Until(() => _backgroundSystem.Initialized);
        
        if (_backgroundSystem.CurrentBackground)
            _background.sprite = _backgroundSystem.CurrentBackground;
    }
    
    private void OnDestroy()
    {
        _newGame.onClick.RemoveListener(NewGameClickHandler);
        _leaderBoard.onClick.RemoveListener(LeaderboardClickHandler);
        _backGround.onClick.RemoveListener(BackgroundClickHandler);
    }
}
