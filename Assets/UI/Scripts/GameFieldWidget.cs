using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFieldWidget : BaseWidget
{
    static readonly Dictionary<Operation, string> SIGNS = new()
    {
        {Operation.Addition, "+"},
        {Operation.Subtraction, "-"},
        {Operation.Multiplication, "*"},
        {Operation.Division, "/"}
    };
    
    [SerializeField] Button _pause;
    [SerializeField] Button _help;
    [SerializeField] GameOver _gameOver;
    [SerializeField] TextMeshProUGUI _pauseSymbol;
    [SerializeField] TextMeshProUGUI _example;
    [SerializeField] TextMeshProUGUI _timer;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] Transform _gameOverContainer;
    
    [Header("Answers initialisation")]
    [SerializeField] RectTransform _container;
    [SerializeField] ScrollRect _scroll;
    [SerializeField] Answer _answerPrefab;
    [SerializeField] int _amountAnswersForScrolling;
    [SerializeField] float _timeToHighlight;
    
    ILeaderboardSystem _leaderboardSystem; 
    GameModel _model;

    GameOver _gameOverInstance;
    Example _currentExample;
    Answer[] _answers;
    int _score;
    int _additionalScore;
    int _step;
    int _min;
    int _max;
    int _streak;
    bool _isEasy;
    bool _boosterUsed;
    bool _endGame;
    bool _paused;
    float _time;
    float _additionalTime;
    
    public void Init(ILeaderboardSystem leaderboardSystem, GameModel model)
    {
        _leaderboardSystem = leaderboardSystem;
        _model = model;
        _isEasy = true;
        _step = 0;
        _score = 0;
        _time = _model.GameSessionTime;
        _min = _model.EasyMinValue;
        _max = _model.EasyMaxValue;
        _additionalScore = _model.ScoreEasy;
        _additionalTime = _model.AdditionalTimeEasy;
        _scroll.horizontal = _model.AmountOfAnswers > _amountAnswersForScrolling;
        _answers = new Answer[_model.AmountOfAnswers];
        _timer.text = $"{_time}";
        
        for (int i = 0; i < _model.AmountOfAnswers; i++)
        {
            _answers[i] = Instantiate(_answerPrefab, _container);
            _answers[i].Init(i);
            _answers[i].Pressed += ChoseAnswerHandler;
        }
        
        NextExample();
        Timer();
        _pause.onClick.AddListener(PauseClickHandler);
        _help.onClick.AddListener(HelpClickHandler);
    }

    void NextExample()
    {
        _currentExample = ExampleGenerator.Generate(_isEasy, _min, _max);
        _example.text = $"{_currentExample.First} {SIGNS[_currentExample.Operation]} {_currentExample.Second} = ?";
        
        for (int i = 0; i < _answers.Length; i++)
            _answers[i].SetValue(_currentExample.Answers[i]);
    }
    
    async void ChoseAnswerHandler(int index)
    {
        if (_paused)
            return;
        
        foreach (var answer in _answers)
            answer.SetUnInteractable();
        
        if (_currentExample.Answer == _currentExample.Answers[index])
        {
            _score += _additionalScore;
            _scoreText.text = $"{_score}";
            _step++;
            _streak++;
            _time += _additionalTime;
            _timer.text = $"{_time}";
            
            _answers[index].HighLight(_timeToHighlight, true);

            await Awaiters.Seconds(_timeToHighlight);
            
            CheckDifficulty();
            NextExample();

            if (_boosterUsed && _streak >= _model.BoosterAdditional)
            {
                _boosterUsed = false;
                _help.gameObject.SetActive(true);
            }
        }
        else
        {
            _answers[index].HighLight(_timeToHighlight, false);
            
            await Awaiters.Seconds(_timeToHighlight);

            EndGame();
        }
    }
    
    void CheckDifficulty()
    {
        if (_step < _model.ChangeDifficulty)
            return;

        if (_isEasy)
        {
            _isEasy = false;

            _min = _model.MediumMinValue;
            _max = _model.MediumMaxValue;
            _additionalScore = _model.ScoreMedium;
            _additionalTime = _model.AdditionalTimeMedium;
            _step = 0;
        }
        else
        {
            _min = _model.HardMinValue;
            _max = _model.HardMaxValue;
            _additionalScore = _model.ScoreHard;
            _additionalTime = _model.AdditionalTimeHard;
        }
    }

    void EndGame()
    {
        if (_endGame)
            return;
        
        _endGame = true;
        _help.gameObject.SetActive(false);
        _container.gameObject.SetActive(false);
            
        var newRecord = _leaderboardSystem.IsNewRecord(_score);

        _gameOverInstance = Instantiate(_gameOver, _gameOverContainer);
        _gameOverInstance.Init(newRecord);
        _gameOverInstance.Submit += SubmitHandler;
        _gameOverInstance.Cancel += CancelHandler;
    }
    
    void PauseClickHandler()
    {
        if (_endGame)
            return;
        
        if (_paused)
        {
            Time.timeScale = 1;
            _pauseSymbol.text = "||";
        }
        else
        {
            Time.timeScale = 0;
            _pauseSymbol.text = "▶";
        }

        _paused = !_paused;
    }

    void HelpClickHandler()
    {
        if (_paused)
            return;
        
        _streak = 0;
        _boosterUsed = true;
        _help.gameObject.SetActive(false);
        
        var incorrect = new List<Answer>();

        for (int i = 0; i < _answers.Length; i++)
        {
            if (_currentExample.Answer != _currentExample.Answers[i])
                incorrect.Add(_answers[i]);
        }

        incorrect = incorrect.OrderBy(_ => Guid.NewGuid()).Take(_model.AmountOfAnswers / 2).ToList();

        foreach (var answer in incorrect)
            answer.Inactive();
    }

    void SubmitHandler(string name)
    {
        Unsubscribe();
        
        _leaderboardSystem.AddUser(new LeaderboardUserModel {name = name, score = _score});
        Close();
    }

    void CancelHandler()
    {
        Unsubscribe();
        Close();
    }

    async void Timer()
    {
        while (_time > 0)
        {
            if (_endGame)
                break;
            
            await Awaiters.Seconds(1);
            
            _timer.text = $"{--_time}";
        }

        EndGame();
    }
    
    void Unsubscribe()
    {
        _gameOverInstance.Dispose();
        _gameOverInstance.Submit -= SubmitHandler;
        _gameOverInstance.Cancel -= CancelHandler;

        foreach (var answer in _answers)
        {
            answer.Dispose();
            answer.Pressed -= ChoseAnswerHandler;
        }
    }

    void Close()
    {
        _pause.onClick.RemoveListener(PauseClickHandler);
        _help.onClick.RemoveListener(HelpClickHandler);
        Closed?.Invoke(this);
        Destroy(gameObject);
    }
}
