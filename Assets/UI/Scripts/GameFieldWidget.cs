using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using Leaderboard;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFieldWidget : BaseWidget
{
    private static readonly Dictionary<Operation, string> SIGNS = new()
    {
        {Operation.Addition, "+"},
        {Operation.Subtraction, "-"},
        {Operation.Multiplication, "*"},
        {Operation.Division, "/"}
    };
    
    [SerializeField] private Button _pause;
    [SerializeField] private Button _help;
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private TextMeshProUGUI _pauseSymbol;
    [SerializeField] private TextMeshProUGUI _example;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Transform _gameOverContainer;
    
    [Header("Answers initialisation")]
    [SerializeField] private RectTransform _container;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private Answer _answerPrefab;
    [SerializeField] private int _amountAnswersForScrolling;
    [SerializeField] private float _timeToHighlight;
    
    private ILeaderboardSystem _leaderboardSystem;
    private IExampleGenerator _exampleGenerator;
    private GameModel _model;

    private GameOver _gameOverInstance;
    private Example _currentExample;
    private Answer[] _answers;
    private int _score;
    private int _additionalScore;
    private int _step;
    private int _min;
    private int _max;
    private int _streak;
    private bool _isEasy;
    private bool _boosterUsed;
    private bool _endGame;
    private bool _paused;
    private float _time;
    private float _additionalTime;
    
    public void Init(ILeaderboardSystem leaderboardSystem, IExampleGenerator exampleGenerator, GameModel model)
    {
        _leaderboardSystem = leaderboardSystem;
        _exampleGenerator = exampleGenerator;
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

    private void NextExample()
    {
        _currentExample = _exampleGenerator.Generate(_isEasy, _min, _max);
        _example.text = $"{_currentExample.First} {SIGNS[_currentExample.Operation]} {_currentExample.Second} = ?";
        
        for (int i = 0; i < _answers.Length; i++)
            _answers[i].SetValue(_currentExample.Answers[i]);
    }
    
    private async void ChoseAnswerHandler(int index)
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
    
    private void CheckDifficulty()
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

    private void EndGame()
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
    
    private void PauseClickHandler()
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

    private void HelpClickHandler()
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

    private void SubmitHandler(string name)
    {
        Unsubscribe();
        
        _leaderboardSystem.AddUser(new LeaderboardUserModel {name = name, score = _score});
        Close();
    }

    private void CancelHandler()
    {
        Unsubscribe();
        Close();
    }

    private async void Timer()
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
    
    private void Unsubscribe()
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

    private void Close()
    {
        _pause.onClick.RemoveListener(PauseClickHandler);
        _help.onClick.RemoveListener(HelpClickHandler);
        Closed?.Invoke(this);
        Destroy(gameObject);
    }
}
