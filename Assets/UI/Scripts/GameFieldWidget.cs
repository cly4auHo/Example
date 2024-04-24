using System;
using System.Collections.Generic;
using System.Linq;
using Background;
using Generator;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameFieldWidget : BaseWidget
    {
        private static readonly Dictionary<Operation, string> SIGNS = new()
        {
            {Operation.Addition, "+"},
            {Operation.Subtraction, "-"},
            {Operation.Multiplication, "*"},
            {Operation.Division, "/"}
        };

        [SerializeField] private Image _background;
        [SerializeField] private Button _pause;
        [SerializeField] private Button _help;
        [SerializeField] private TextMeshProUGUI _pauseSymbol;
        [SerializeField] private TextMeshProUGUI _example;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("Answers initialisation")] 
        [SerializeField] private RectTransform _container;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private AnswerView _answerViewPrefab;
        [SerializeField] private int _amountAnswersForScrolling;
        [SerializeField] private float _timeToHighlight;
        
        [Inject] private IExampleGenerator _exampleGenerator;
        [Inject] private IBackgroundSystem _backgroundSystem;
        [Inject] private GameModel _model;
        
        private Example _currentExample;
        private AnswerView[] _answers;
        private List<AnswerView> _inactiveAnswers;
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

        public override void OnCreated()
        {
            _isEasy = true;
            _step = 0;
            _score = 0;
            _time = _model.GameSessionTime;
            _min = _model.EasyMinValue;
            _max = _model.EasyMaxValue;
            _additionalScore = _model.ScoreEasy;
            _additionalTime = _model.AdditionalTimeEasy;
            _scroll.horizontal = _model.AmountOfAnswers > _amountAnswersForScrolling;
            _answers = new AnswerView[_model.AmountOfAnswers];
            _timer.text = $"{_time}";
            _inactiveAnswers = new List<AnswerView>();
            
            for (int i = 0; i < _model.AmountOfAnswers; i++)
            {
                _answers[i] = Instantiate(_answerViewPrefab, _container);
                _answers[i].Init(i);
                _answers[i].Pressed += ChoseAnswerHandler;
            }

            SetBackground();
            NextExample();
            Timer();
            _gameManager.RestartGame += Close;
            _pause.onClick.AddListener(PauseClickHandler);
            _help.onClick.AddListener(HelpClickHandler);
        }

        public override void OnClosed()
        {
            foreach (var answer in _answers)
            {
                answer.Dispose();
                answer.Pressed -= ChoseAnswerHandler;
            }
            
            _gameManager.RestartGame -= Close;
            _pause.onClick.RemoveListener(PauseClickHandler);
            _help.onClick.RemoveListener(HelpClickHandler);
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

            _inactiveAnswers.Clear();
            
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
            
            _gameManager.EndGame(_score);
        }

        private void PauseClickHandler()
        {
            if (_endGame)
                return;

            if (_paused)
            {
                Time.timeScale = 1;
                _pauseSymbol.text = "||";
                _example.gameObject.SetActive(true);
                
                foreach (var answer in _answers)
                {
                    if (!_inactiveAnswers.Contains(answer))
                        answer.Active();
                }
            }
            else
            {
                Time.timeScale = 0;
                _pauseSymbol.text = "▶";
                _example.gameObject.SetActive(false);
                
                foreach (var answer in _answers)
                    answer.Inactive();
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
            
            for (int i = 0; i < _answers.Length; i++)
            {
                if (_currentExample.Answer != _currentExample.Answers[i])
                    _inactiveAnswers.Add(_answers[i]);
            }

            _inactiveAnswers = _inactiveAnswers.OrderBy(_ => Guid.NewGuid()).Take(_model.AmountOfAnswers / 2).ToList();

            foreach (var answer in _inactiveAnswers)
                answer.Inactive();
        }
        
        private async void SetBackground()
        {
            await Awaiters.Until(() => _backgroundSystem.Initialized);

            if (_backgroundSystem.CurrentBackground)
                _background.sprite = _backgroundSystem.CurrentBackground;
        }

        private void Close() => Closed?.Invoke(this);
    }
}